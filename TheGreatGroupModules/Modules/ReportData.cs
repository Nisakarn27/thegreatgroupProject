using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using TheGreatGroupModules.Models;

namespace TheGreatGroupModules.Modules
{
    public class ReportData
    {

        private string errMsg = "";


        public List<OpenAccountReport> OpenAccountReports(SearchCriteria search)
        {


            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);

            try
            {

                DateTime StartDate = DateTime.ParseExact(search.FromDateStr, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime EndDate = DateTime.ParseExact(search.ToDateStr, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                if (search.TypeDate == 2)
                {
                    StartDate = new DateTime(search.Year, search.Month, 1);

                    EndDate = new DateTime(search.Year, search.Month, DateTime.DaysInMonth(search.Year, search.Month));
                }
                else if (search.TypeDate == 3)
                {
                    StartDate = new DateTime(search.Year, 1, 1);

                    EndDate = new DateTime(search.Year, 12, 31);

                }


                List<OpenAccountReport> listData = new List<OpenAccountReport>();


                string StrSql = @"   SELECT DISTINCT ct.ContractID,
                    CONCAT(c.CustomerTitleName,c.CustomerFirstName, '  ', c.CustomerLastName)AS  CustomerName ,
                    ct.ContractCustomerID,
                    ct.ContractNumber,
                    ct.ContractCreateDate,
                    ct.ContractExpDate,
                    ct.ContractCloseDate,
                    ct.ContractPayment ,
                    (ct.ContractPayment- (((pc.PriceGold/15.20)*p.UnitAmount )+300) )AS diff ,
                      (((pc.PriceGold/15.20)*p.UnitAmount )+300) AS PriceCost,
                    p.UnitAmount 
                    FROM contract ct 
                    LEFT OUTER JOIN customer c ON ct.ContractCustomerID=c.CustomerId
                    LEFT OUTER JOIN staff_zone sz ON c.SaleID=sz.StaffID
                    LEFT OUTER JOIN product_customer pc ON pc.ContractID=ct.ContractID AND pc.CustomerID=ct.ContractCustomerID
                    LEFT OUTER JOIN products p ON pc.ProductID=p.ProductID
                    WHERE   ct.Activated=1  and sz.ZoneID=" + search.ZoneID + " and ct.ContractStatus=1  ";

                if (StartDate != null & EndDate != null)
                    StrSql += " AND Date(ct.ContractCreateDate) BETWEEN " + Utility.FormateDate(StartDate) + " AND " + Utility.FormateDate(EndDate);


                StrSql += " ORDER BY ct.ContractCreateDate ASC";


                DataTable dt = DBHelper.List(StrSql, ObjConn);
                if (dt != null && dt.Rows.Count > 0)
                {
                    listData = OpenAccountReport.ToObjectList(dt);
                }
                return listData;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                ObjConn.Close();
            }


        }
        public void SaveActivateDailyReceipts(int staffId, string date_str)
        {

            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);

            try
            {
                DateTime dateAsOf = DateTime.ParseExact(date_str, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                string StrSql = @"   UPDATE daily_receipts SET Activated=1
                                     WHERE  Deleted=0 AND DATE(DateAsOf)='" + dateAsOf.ToString("yyyy-MM-dd") + "'";


                DBHelper.Execute(StrSql, ObjConn);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                ObjConn.Close();
            }
        }
        public List<DailyReceiptsReport> GetDailyReceiptsReport(int staffId, string date_str)
        {
            //DateTime dateAsOf = DateTime.ParseExact(date_str, "dd/MM/yyyy", null);

            DateTime[] HolidaysArr = Utility.Holidays(1);
            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);
            DateTime dateAsOf = DateTime.ParseExact(date_str, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            try
            {

                string StrSql = @" SELECT ctm.CustomerID,c.ContractID,ctm.SaleID as StaffID,
                c.ContractNumber,
                CONCAT(ctm.CustomerTitleName,ctm.CustomerFirstName, '  ', ctm.CustomerLastName)AS  CustomerName ,
                ctm.CustomerNickName, 
                c.ContractCreateDate,
                c.ContractExpDate,
                c.ContractStartDate,
                c.ContractPayment,
                c.ContractAmount,
                c.ContractAmountLast,
                c.ContractPayEveryDay,
                c.ContractSpecialholiday,
                (SELECT  CASE WHEN SUM(PriceReceipts) IS NULL  THEN 0 ELSE SUM(PriceReceipts) END AS PriceReceipts
                 FROM daily_totalreceipts d WHERE c.ContractID=d.ContractID
                 AND Date(DateAsOf)='" + dateAsOf.ToString("yyyy-MM-dd") + @"'
                 ) AS PriceReceipts , -- รวมเงินที่ได้รับ
                 (SELECT  CASE WHEN SUM(PriceReceipts) IS NULL  THEN c.ContractPayment-0 ELSE c.ContractPayment-SUM(PriceReceipts) END AS balance
                 FROM daily_receipts d WHERE c.ContractID=d.ContractID
                  GROUP BY d.ContractID
                 ) AS Balance , --  ยอดคงเหลือ
                 (SELECT  CASE WHEN Remark IS NULL  THEN '' ELSE Remark END AS Remark
                 FROM daily_remark r WHERE c.ContractID=r.ContractID
                 AND Date(DateAsOf)='" + dateAsOf.ToString("yyyy-MM-dd") + @"'
                   limit 1
                 )AS Remark,
                 (SELECT MAX(DATE(dd.DateAsOf)) FROM daily_receipts dd 
                WHERE c.ContractID=dd.ContractID AND dd.deleted=0  and PriceReceipts>0
                GROUP BY ContractID) AS LastPayment,
                (select CASE WHEN SUM(Discount) IS NULL  THEN 0 ELSE SUM(Discount) END AS Discount  From discount dc 
                Where c.ContractID=dc.ContractID
                 ) as ContractDiscount
                FROM Contract c
                 LEFT JOIN customer ctm ON c.ContractCustomerID=ctm.CustomerId
                WHERE ctm.SaleID=" + staffId + "  and c.Activated=1 and c.Deleted=0";


                DataTable dt = DBHelper.List(StrSql, ObjConn);
                List<DailyReceiptsReport> listData = new List<DailyReceiptsReport>();
                DateTime lastpay = new DateTime();
                int statusdate = 0;
                int IsFirstTime = 0;
                if (dt != null && dt.Rows.Count > 0)
                {
                    DailyReceiptsReport dr = new DailyReceiptsReport();

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dr = new DailyReceiptsReport();
                        if (dt.Rows[i]["CustomerID"] != DBNull.Value)
                            dr.CustomerID = dt.Rows[i]["CustomerID"].ToInt();
                        dr.ContractID = dt.Rows[i]["ContractID"].ToInt();
                        dr.ContractNumber = dt.Rows[i]["ContractNumber"].ToString();
                        dr.CustomerName = dt.Rows[i]["CustomerName"].ToString();

                        if (dt.Rows[i]["ContractCreateDate"] != DBNull.Value)
                            dr.ContractCreateDate = Convert.ToDateTime(dt.Rows[i]["ContractCreateDate"].ToString());
                        if (dt.Rows[i]["ContractCreateDate"] != DBNull.Value)
                            dr.ContractExpDate = Convert.ToDateTime(dt.Rows[i]["ContractExpDate"].ToString());
                        if (dt.Rows[i]["ContractStartDate"] != DBNull.Value)
                            dr.ContractStartDate = Convert.ToDateTime(dt.Rows[i]["ContractStartDate"].ToString());
                        // ทุกวัน =2 / จ-ศ =1
                        if (dt.Rows[i]["ContractPayEveryDay"] != DBNull.Value)
                            dr.ContractPayEveryDay = Convert.ToInt32(dt.Rows[i]["ContractPayEveryDay"].ToString());

                        // 1 เว้นวันหยุด  / 0 ไม่เว้นไม่หยุด
                        if (dt.Rows[i]["ContractSpecialholiday"] != DBNull.Value)
                            dr.ContractSpecialholiday = Convert.ToBoolean(dt.Rows[i]["ContractSpecialholiday"].ToString());

                        dr.ContractAmount = dt.Rows[i]["ContractAmount"].ToDecimal() ?? 0;
                        dr.PriceReceipts = dt.Rows[i]["PriceReceipts"].ToDecimal() ?? 0;
                        dr.Balance = dt.Rows[i]["Balance"].ToDecimal() ?? 0;
                        dr.ContractAmountLast = dt.Rows[i]["ContractAmountLast"].ToDecimal() ?? 0;
                        dr.ContractDiscount = dt.Rows[i]["ContractDiscount"].ToDecimal() ?? 0;

                        if (dt.Rows[i]["LastPayment"] != DBNull.Value)
                            dr.LastPayment = Convert.ToDateTime(dt.Rows[i]["LastPayment"].ToString());

                        lastpay = dt.Rows[i]["LastPayment"] == DBNull.Value ? dr.ContractStartDate : Convert.ToDateTime(dt.Rows[i]["LastPayment"].ToString());
                        IsFirstTime = dt.Rows[i]["LastPayment"] == DBNull.Value ? 1 : 0;
                        if (dt.Rows[i]["LastPayment"]==DBNull.Value)
                        {
                            if (dr.PriceReceipts > 0)
                            {
                                statusdate = ListCustomerOnMobile.DiffLastTransaction(lastpay, dr.ContractPayEveryDay, dr.ContractSpecialholiday, HolidaysArr, IsFirstTime);
                            }
                            else {
                                statusdate = -1;

                            }
                        }
                        else {
                            statusdate = ListCustomerOnMobile.DiffLastTransaction(lastpay, dr.ContractPayEveryDay, dr.ContractSpecialholiday, HolidaysArr, IsFirstTime);

                        }

                        dr.Status = statusdate.ToString();

                        if (dt.Rows[i]["Remark"] != DBNull.Value)
                            dr.Remark = dt.Rows[i]["Remark"].ToString();

                        if (dt.Rows[i]["StaffID"] != DBNull.Value)
                            dr.StaffID = dt.Rows[i]["StaffID"].ToInt();
                        listData.Add(dr);
                    }
                }
                return listData;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                ObjConn.Close();
            }
        }

        public List<DailyReceiptsReport> GetCustomerDetailOnCard(string staffId, string CustomerID, string ContractID)
        {

            StaffData st0 = new StaffData();
            List<Staffs> item = new List<Staffs>();
            int sid = Convert.ToInt32(staffId);
            item = st0.GetStaff(sid);
            Staffs staff = new Staffs();
            staff = item.FirstOrDefault();

            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);


            try
            {
                List<DailyReceiptsReport> listData = new List<DailyReceiptsReport>();

                string StrSql = @"    SELECT  c.CustomerID, ct.ContractID ,c.CustomerMobile,c.CustomerTelephone, ct.ContractPayment,
                                       CONCAT(c.CustomerTitleName,c.CustomerFirstName, '  ', c.CustomerLastName)AS  CustomerName 
                                    ,c.CustomerNickName,c.SaleID As StaffID   ,                         
                                     ct.ContractNumber,ct.ContractCreateDate,ct.ContractExpDate ,ct.ContractAmount,
                                             SUM( a.PriceReceipts ) AS PriceReceipts,ct.ContractAmountLast,
                                    ( SELECT  ct.ContractPayment- IFNULL( SUM(d.PriceReceipts),0 )
                                    FROM  daily_receipts d
                                    WHERE  d.Deleted=0 AND d.ContractID=ct.ContractID  )AS Balance,
                                      ( SELECT  ct.ContractAmountLast- IFNULL(SUM(d.Diff) ,0)
                                    FROM  daily_receipts d
                                    WHERE  d.Deleted=0 AND d.ContractID=ct.ContractID  )AS Diff
                                    FROM daily_receipts a
                                    LEFT JOIN Customer c ON  a.CustomerID= c.CustomerId
                                    LEFT JOIN contract ct ON  a.ContractID= ct.ContractID
                                    WHERE 0=0     and ct.ContractStatus=1
                                    AND a.Deleted=0 and c.Deleted=0  ";


                DataTable dt1 = DBHelper.List("select * FROM staffrolepermission where StaffPermissionID=22 AND StaffRoleID=" + staff.StaffRoleID, ObjConn);

                if (dt1.Rows.Count != 1)
                    StrSql += "  AND  c.SaleID=" + staffId;
                if (ContractID.Trim() != "0".Trim())
                    StrSql += @"  AND a.ContractID=" + ContractID;
                if (CustomerID.Trim() != "0".Trim())
                    StrSql += "  AND a.CustomerID=" + CustomerID;

                if (staffId.Trim() != "0".Trim())
                    StrSql += "  AND c.SaleID=" + staffId;

                StrSql += "  GROUP BY  a.CustomerID ORDER BY a.DateAsOf ";

                DataTable dt = DBHelper.List(StrSql, ObjConn);
                if (dt != null && dt.Rows.Count > 0)
                {
                    listData = DailyReceiptsReport.ToObjectList(dt);
                }
                return listData;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                ObjConn.Close();
            }

        }

        public IList<LastTransaction> GetTransaction(string staffId, string CustomerID, string ContractID)
        {

            IList<LastTransaction> listData = new List<LastTransaction>();
            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);
            try
            {
                string StrSql = @" 
                    SELECT tb.* FROM (
                    SELECT DATE(dr.DateAsOf)  AS DateAsOf ,
                    SUM(dr.PriceReceipts) AS Amount 
                    FROM daily_totalreceipts dr
                    LEFT JOIN contract c ON dr.ContractID=c.ContractID
                    WHERE dr.Deleted=0 AND
                    dr.customerID={0} AND 
                    dr.ContractID={1}
                    GROUP BY dr.customerID,dr.ContractID,DATE(dr.DateAsOf)
                    ) tb where tb.Amount>0";
                StrSql = string.Format(StrSql, CustomerID, ContractID);
                DataTable dt = DBHelper.List(StrSql, ObjConn);
                if (dt != null && dt.Rows.Count > 0)
                {
                    listData = dt.AsEnumerable().Select(dr => new LastTransaction()
                    {
                        DateAsOf = dr.Field<DateTime>("DateAsOf"),
                        Amount = dr.Field<decimal>("Amount"),

                    }).ToList();
                }

                return listData;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                ObjConn.Close();
            }



        }


        public IList<DailyReceiptsReport> GetDiscountReport(SearchCriteria search)
        {



            DateTime StartDate = DateTime.ParseExact(search.FromDateStr, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime EndDate = DateTime.ParseExact(search.ToDateStr, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            if (search.TypeDate == 2)
            {
                StartDate = new DateTime(search.Year, search.Month, 1);

                EndDate = new DateTime(search.Year, search.Month, DateTime.DaysInMonth(search.Year, search.Month));
            }
            else if (search.TypeDate == 3)
            {
                StartDate = new DateTime(search.Year, 1, 1);

                EndDate = new DateTime(search.Year, 12, 31);

            }




            IList<DailyReceiptsReport> listData = new List<DailyReceiptsReport>();
            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);
            try
            {
                string StrSql = @" SELECT d.ContractID,d.CustomerID,ct.ContractNumber,d.ApproveDate ,d.ApproveBy,s.StaffFirstName,
                CONCAT(c.CustomerTitleName,c.CustomerFirstName, ' ',c.CustomerLastName) AS CustomerName
                ,ct.ContractPayment,(ct.ContractPayment-d.Discount) AS PriceReceipts,d.Discount
                ,ct.ContractExpDate
                ,ct.ContractCloseDate
                ,ct.ContractTimes
                ,ct.ContractAccountStatus
                FROM discount d
                LEFT JOIN customer c ON d.CustomerID=c .CustomerID
                LEFT JOIN contract ct ON d.ContractID=ct.ContractID AND ct.ContractCustomerID=d.CustomerID
                LEFT JOIN staff s ON d.ApproveBy=s.StaffID
                LEFT OUTER JOIN staff_zone sz ON c.SaleID=sz.StaffID
                WHERE   ct.Deleted=0 AND ct.ContractStatus=0  and sz.ZoneID=" + search.ZoneID;

                if (search.TypeDate == 1)
                    StrSql += " AND DATE(d.ApproveDate) = {0}";
                if (search.TypeDate == 2)
                    StrSql += " AND DATE(d.ApproveDate) Between {0}  and {1} ";
                if (search.TypeDate == 3)
                    StrSql += " AND DATE(d.ApproveDate) Between {0}  and {1} ";
                if (search.TypeDate == 4)
                    StrSql += " AND DATE(d.ApproveDate) Between {0}  and {1} ";


                StrSql = string.Format(StrSql, Utility.FormateDate(StartDate), Utility.FormateDate(EndDate));
                DataTable dt = DBHelper.List(StrSql, ObjConn);
                if (dt != null && dt.Rows.Count > 0)
                {
                    listData = dt.AsEnumerable().Select(dr => new DailyReceiptsReport()
                    {
                        ContractCloseDate = dr.Field<DateTime>("ContractCloseDate"), //วันที่ให้ส่วนลด
                        ContractExpDate = dr.Field<DateTime>("ContractExpDate"),
                        StaffID = dr.Field<int>("ApproveBy"), //ให้โดย
                        StaffName = dr.Field<string>("StaffFirstName"),
                        ContractNumber = dr.Field<string>("ContractNumber"),
                        CustomerName = dr.Field<string>("CustomerName"),
                        TotalSales = dr.Field<decimal>("ContractPayment"),
                        PriceReceipts = dr.Field<decimal>("PriceReceipts"),
                        ContractDiscount = dr.Field<decimal>("Discount"),
                        ContractTimes = dr.Field<int>("ContractTimes"),
                        
                }).ToList();
                }

                return listData;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                ObjConn.Close();
            }

        }


        public IList<NPLReport> GetNPLReport(NPLReport  search) {


            IList<NPLReport> listData = new List<NPLReport>();
            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);
            try
            {
                string StrSql = @" SELECT ct.ContractCreateDate 
                                    ,ct.ContractCustomerID
                                    , CONCAT(c.CustomerTitleName,c.CustomerFirstName,' ',c.CustomerLastName) AS CustomerName
                                    ,ct.ContractNumber
                                    ,ct.ContractStartDate
                                    ,ct.ContractExpDate
                                    ,ct.ContractPayment
                                    ,IFNULL((ct.ContractPayment - (SELECT SUM(PriceReceipts) FROM daily_totalreceipts WHERE ContractID=ct.ContractID)) ,0)AS NPL 
                                    ,"+search.NPLDay+@" AS NPLDay
                                    ,IFNULL((SELECT  MAX(DateAsOf) FROM daily_totalreceipts WHERE ContractID=ct.ContractID) ,ct.ContractStartDate )AS LastpayDate
                                    FROM contract ct 
                                    LEFT JOIN customer c ON ct.ContractCustomerID=c.CustomerId
                                    WHERE 0=0  and ct.ContractStatus=1 AND c.SaleID={1}
                                    AND  ct.Activated=1 and ct.Deleted=0 and c.Deleted=0 
                                    HAVING NPLDay>={0} ";

              

                StrSql = string.Format(StrSql, search.NPLDay,search.StaffID);
                DataTable dt = DBHelper.List(StrSql, ObjConn);
                if (dt != null && dt.Rows.Count > 0)
                {
                    listData = dt.AsEnumerable().Select(dr => new NPLReport()
                    {
                        ContractCreateDate = dr.Field<DateTime>("ContractCreateDate"), //วันที่ให้ส่วนลด
                        ContractNumber = dr.Field<string>("ContractNumber"), //ให้โดย
                        ContractCustomerID = dr.Field<int>("ContractCustomerID"),
                        CustomerName = dr.Field<string>("CustomerName"), //ให้โดย
                        ContractStartDate = dr.Field<DateTime>("ContractStartDate"),
                        ContractExpDate = dr.Field<DateTime>("ContractExpDate"),
                        ContractPayment = dr.Field<decimal>("ContractPayment"),
                        LastpayDate = dr.Field<DateTime>("LastpayDate"),
                        //NPLDay = dr.Field<int>("NPLDay"),
                        //NPL = dr.Field<decimal>("NPL"),
                    }).ToList();
                }

                return listData;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                ObjConn.Close();
            }


        }
    }
}