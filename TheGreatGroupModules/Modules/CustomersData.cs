
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using TheGreatGroupModules.Models;

namespace TheGreatGroupModules.Modules
{
    public class CustomersData
    {

        private string errMsg = "";
        public IList<Customers> Get(int id)
        {

            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);

            try
            {

                string StrSql = @"  SELECT  c.* ,
                                   s.SubDistrictName as CustomerSubDistrict,
                                   d.DistrictName as CustomerDistrict,
                                   p.ProvinceName as CustomerProvince
                                   FROM customer c 
                                  LEFT OUTER JOIN province p ON c.CustomerProvinceId = p.ProvinceId
                                  LEFT OUTER JOIN district d ON c.CustomerDistrictId = d.DistrictId
                                  LEFT OUTER JOIN subDistrict s ON c.CustomerSubDistrictId = s.SubDistrictId
                                where Deleted=0 ";

                if (id > 0) { 
                
                   StrSql +=  @" and c.CustomerID="+id ;
                }
                DataTable dt = DBHelper.List(StrSql, ObjConn);

                IList<Customers> listData = new List<Customers>();
                if (dt != null && dt.Rows.Count > 0)
                {
                    listData = Customers.ToObjectList2(dt);
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
        public IList<Customers> GetListCustomers(int id)
        {

            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);

            try
            {

                string StrSql = @" SELECT c.* , ct.ContractNumber,ct.ContractID,
                                   s.SubDistrictName as CustomerSubDistrict,
                                   d.DistrictName as CustomerDistrict,
                                   p.ProvinceName as CustomerProvince
                                   FROM contract ct 
                                  LEFT OUTER JOIN customer c ON ct.ContractCustomerID =  c.CustomerID
                                  LEFT OUTER JOIN province p ON c.CustomerProvinceId = p.ProvinceId
                                  LEFT OUTER JOIN district d ON c.CustomerDistrictId = d.DistrictId
                                  LEFT OUTER JOIN subDistrict s ON c.CustomerSubDistrictId = s.SubDistrictId
                                  where c.Deleted=0  and ct.Deleted=0 ";

                if (id > 0) { 
                
                   StrSql +=  @" and c.CustomerID="+id ;
                }
                DataTable dt = DBHelper.List(StrSql, ObjConn);

                IList<Customers> listData = new List<Customers>();
                if (dt != null && dt.Rows.Count > 0)
                {
                    listData = Customers.ToObjectList3(dt);
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

        public IList<Customers> GetCustomerByZone(int zoneId)
        {

            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);

            try
            {

                string StrSql = @"  	SELECT c.*  ,sz.ZoneID,
				s.SubDistrictName AS CustomerSubDistrict,
                                   d.DistrictName AS CustomerDistrict,
                                   p.ProvinceName AS CustomerProvince
                                   FROM customer c 
                                  LEFT OUTER JOIN province p ON c.CustomerProvinceId = p.ProvinceId
                                  LEFT OUTER JOIN district d ON c.CustomerDistrictId = d.DistrictId
                                  LEFT OUTER JOIN subDistrict s ON c.CustomerSubDistrictId = s.SubDistrictId
				LEFT JOIN staff_zone  sz ON  c.SaleID=sz.StaffID
				WHERE 0=0 ";

                if (zoneId > 0)
                {

                    StrSql += @" AND sz.ZoneID=" + zoneId;
                }
                DataTable dt = DBHelper.List(StrSql, ObjConn);

                IList<Customers> listData = new List<Customers>();
                if (dt != null && dt.Rows.Count > 0)
                {
                    listData = Customers.ToObjectList2(dt);
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
        public IList<Customers> Get(Customers item)
        {

            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);

            try
            {
                string StrSql = @"  SELECT c.* ,
                                   s.SubDistrictName as CustomerSubDistrict,
                                   d.DistrictName as CustomerDistrict,
                                   p.ProvinceName as CustomerProvince
                                   FROM customer c 
                                  LEFT OUTER JOIN province p ON c.CustomerProvinceId = p.ProvinceId
                                  LEFT OUTER JOIN district d ON c.CustomerDistrictId = d.DistrictId
                                  LEFT OUTER JOIN subDistrict s ON c.CustomerSubDistrictId = s.SubDistrictId
                                where Deleted=0 ";

                if (!String.IsNullOrEmpty(item.CustomerFirstName))
                {

                    StrSql += @" and c.CustomerFirstname like '%" + item.CustomerFirstName + "%' ";
                }

                if (!String.IsNullOrEmpty(item.CustomerLastName))
                {

                    StrSql += @" and c.CustomerLastname like '%" + item.CustomerLastName + "%' ";
                }

                if (!String.IsNullOrEmpty(item.CustomerMobile))
                {

                    StrSql += @" and c.CustomerMobile like '%" + item.CustomerMobile + "%' ";
                }
                if (!String.IsNullOrEmpty(item.CustomerIdCard))
                {

                    StrSql += @" and c.CustomerIdCard like '%" + item.CustomerIdCard + "%' ";
                }

                StrSql += " Order By c.InsertDate,c.UpdateDate DESC";

                DataTable dt = DBHelper.List(StrSql, ObjConn);

                IList<Customers> listData = new List<Customers>();
                if (dt != null && dt.Rows.Count > 0)
                {
                    listData = Customers.ToObjectList2(dt);
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

        public void AddCustomer(ref Customers item)
        {
            item.CustomerID = Utility.GetMaxID("Customer", "CustomerID");
            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);
            try
            {
                item.CustomerCode = "MB" + item.CustomerID.ToString("000000000");
                string StrSql = @" INSERT INTO customer
            (CustomerId,
             CustomerCode,
             CustomerTitleName,
             CustomerFirstName,
             CustomerLastName,
             CustomerNickName,
             CustomerIdCard,
             CustomerAddress1,
             CustomerSubDistrictId,
             CustomerDistrictId,
             CustomerProvinceId,
             CustomerZipCode,
             CustomerMobile, 
             CustomerTelephone,
             CustomerStatus,
             CustomerEmail,
             CustomerCareer,
             CustomerJob,
             CustomerJobYear,
             CustomerSalary,
             CustomerJobAddress,
             CustomerJobSubDistrictId,
             CustomerJobDistrictId,
             CustomerJobProvinceId,
             CustomerJobZipCode,
             CustomerSpouseTitle,
             CustomerSpouseFirstName,
             CustomerSpouseLastName,
             CustomerSpouseNickName,
             CustomerSpouseAddress,
             CustomerSpouseSubDistrictId,
             CustomerSpouseDistrictId,
             CustomerSpouseProvinceId,
             CustomerSpouseZipCode,
             CustomerSpouseMobile,
             CustomerSpouseTelephone,
            CustomerEmergencyTitle,
            CustomerEmergencyFirstName,
            CustomerEmergencyLastName,
            CustomerEmergencyRelation,
            CustomerEmergencyMobile,
            CustomerEmergencyTelephone,
             SaleID,
            CustomerPartner,
            InsertDate,
            InsertBy,
             Activated,
             Deleted)values("
             + item.CustomerID+ ","
             + Utility.ReplaceString(item.CustomerCode)+ ","
             + Utility.ReplaceString(item.CustomerTitleName)+ ","
             +Utility.ReplaceString(item.CustomerFirstName)+ ","
             +Utility.ReplaceString(item.CustomerLastName)+ ","
             +Utility.ReplaceString(item.CustomerNickName)+ ","
             +Utility.ReplaceString(item.CustomerIdCard)+ ","
             +Utility.ReplaceString(item.CustomerAddress1)+ ","
             + item.CustomerSubDistrictId+ ","
             +  item.CustomerDistrictId+ ","
             +  item.CustomerProvinceId+ ","
             + Utility.ReplaceString(item.CustomerZipCode) + ","
             +Utility.ReplaceString(item.CustomerMobile)+ ","
              +Utility.ReplaceString(item.CustomerTelephone)+ ","
              +Utility.ReplaceString(item.CustomerStatus)+ ","
           + Utility.ReplaceString(item.CustomerEmail) + ","
           + Utility.ReplaceString(item.CustomerCareer) + ","
           + Utility.ReplaceString(item.CustomerJob) + ","
            + Utility.ReplaceString(item.CustomerJobYear) + ","
           + Utility.ReplaceString(item.CustomerSalary) + ","
           + Utility.ReplaceString(item.CustomerJobAddress) + ","
           + item.CustomerJobSubDistrictId + ","
           + item.CustomerJobDistrictId + ","
           + item.CustomerJobProvinceId + ","
           + Utility.ReplaceString(item.CustomerJobZipCode) + ","
            + Utility.ReplaceString(item.CustomerSpouseTitle) + ","
           + Utility.ReplaceString(item.CustomerSpouseFirstName) + ","
           + Utility.ReplaceString(item.CustomerSpouseLastName) + ","
           + Utility.ReplaceString(item.CustomerSpouseNickName) + ","
           + Utility.ReplaceString(item.CustomerSpouseAddress) + ","
           + item.CustomerSpouseSubDistrictId + ","
           + item.CustomerSpouseDistrictId + ","
           + item.CustomerSpouseProvinceId + ","
           + Utility.ReplaceString(item.CustomerSpouseZipCode) + ","
           + Utility.ReplaceString(item.CustomerSpouseMobile) + ","
           + Utility.ReplaceString(item.CustomerSpouseTelephone) + ","
           + Utility.ReplaceString(item.CustomerEmergencyTitle) + ","
           + Utility.ReplaceString(item.CustomerEmergencyFirstName) + ","
           + Utility.ReplaceString(item.CustomerEmergencyLastName) + ","
           + Utility.ReplaceString(item.CustomerEmergencyRelation) + ","
           + Utility.ReplaceString(item.CustomerEmergencyMobile) + ","
           + Utility.ReplaceString(item.CustomerEmergencyTelephone) + ","
           + item.SaleID + ","
           + item.CustomerPartner + ","
           + Utility.FormateDateTime(DateTime.Now) + ","
           + item.CustomerInsertBy + ","
           + 1 + ","
           +  0+ ")";

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
        
        public void EditCustomer(ref Customers item)
        {
            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);
            try
            {
               
                string StrSql = @" Update customer Set
             CustomerTitleName={1},
             CustomerFirstName={2},
             CustomerLastName={3},
             CustomerNickName={4},
             CustomerIdCard={5},
             CustomerAddress1={6},
             CustomerSubDistrictId={7},
             CustomerDistrictId={8},
             CustomerProvinceId={9},
             CustomerZipCode={10},
             CustomerMobile={11}, 
             CustomerTelephone={12},
             CustomerStatus={13},
             CustomerEmail={14},
             CustomerCareer={15},
             CustomerJob={16},
             CustomerJobYear={17},
             CustomerSalary={18},
             CustomerJobAddress={19},
             CustomerJobSubDistrictId={20},
             CustomerJobDistrictId={21},
             CustomerJobProvinceId={22},
             CustomerJobZipCode={23},
             CustomerSpouseTitle={24},
             CustomerSpouseFirstName={25},
             CustomerSpouseLastName={26},
             CustomerSpouseNickName={27},
             CustomerSpouseAddress={28},
             CustomerSpouseSubDistrictId={29},
             CustomerSpouseDistrictId={30},
             CustomerSpouseProvinceId={31},
             CustomerSpouseZipCode={32},
             CustomerSpouseMobile={33},
             CustomerSpouseTelephone={34},
            CustomerEmergencyTitle={35},
            CustomerEmergencyFirstName={36},
            CustomerEmergencyLastName={37},
            CustomerEmergencyRelation={38},
            CustomerEmergencyMobile={39},
            CustomerEmergencyTelephone={40},
            SaleID={41},
            UpdateBy={42},
            UpdateDate={43}
            where  CustomerId={0} ";
              StrSql= String.Format(StrSql,
              item.CustomerID  ,
              Utility.ReplaceString(item.CustomerTitleName)  ,
              Utility.ReplaceString(item.CustomerFirstName)  ,
              Utility.ReplaceString(item.CustomerLastName)  ,
              Utility.ReplaceString(item.CustomerNickName)  ,
              Utility.ReplaceString(item.CustomerIdCard)  ,
              Utility.ReplaceString(item.CustomerAddress1)  ,
              item.CustomerSubDistrictId  ,
              item.CustomerDistrictId  ,
              item.CustomerProvinceId  ,//10
              Utility.ReplaceString(item.CustomerZipCode)  ,
              Utility.ReplaceString(item.CustomerMobile)  ,
               Utility.ReplaceString(item.CustomerTelephone)  ,
               Utility.ReplaceString(item.CustomerStatus)  ,
            Utility.ReplaceString(item.CustomerEmail),
            Utility.ReplaceString(item.CustomerCareer),
            Utility.ReplaceString(item.CustomerJob),
             Utility.ReplaceString(item.CustomerJobYear),
            Utility.ReplaceString(item.CustomerSalary),
            Utility.ReplaceString(item.CustomerJobAddress),//20
            item.CustomerJobSubDistrictId,
            item.CustomerJobDistrictId,
            item.CustomerJobProvinceId,
            Utility.ReplaceString(item.CustomerJobZipCode),
             Utility.ReplaceString(item.CustomerSpouseTitle),
            Utility.ReplaceString(item.CustomerSpouseFirstName),
            Utility.ReplaceString(item.CustomerSpouseLastName),
            Utility.ReplaceString(item.CustomerSpouseNickName),
            Utility.ReplaceString(item.CustomerSpouseAddress),
            item.CustomerSpouseSubDistrictId,//30
            item.CustomerSpouseDistrictId,
            item.CustomerSpouseProvinceId,
            Utility.ReplaceString(item.CustomerSpouseZipCode),
            Utility.ReplaceString(item.CustomerSpouseMobile),
            Utility.ReplaceString(item.CustomerSpouseTelephone),
            Utility.ReplaceString(item.CustomerEmergencyTitle),
            Utility.ReplaceString(item.CustomerEmergencyFirstName),
            Utility.ReplaceString(item.CustomerEmergencyLastName),
            Utility.ReplaceString(item.CustomerEmergencyRelation),
            Utility.ReplaceString(item.CustomerEmergencyMobile),//40
            Utility.ReplaceString(item.CustomerEmergencyTelephone),
            item.SaleID,
            item.CustomerUpdateBy,
            Utility.FormateDateTime(DateTime.Now)
         );

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

        public void DeleteCustomer(int CustomerID)
        {
            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);
            try
            {
               
                string StrSql = @" Update customer Set
                Deleted=1
                where  CustomerId={0} ";

                  StrSql= String.Format(StrSql, CustomerID   );

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
        public void PaymentDailyReceipts(DailyReceiptsReport item)
        {
            if (item.PriceReceipts <= 0)
            {
                throw new Exception("จำนวนเงินต้องมากว่า 0 บาท");
            }


            string StrSql = string.Empty;
            ContractData ct = new ContractData();
            IList<Contract> list = new List<Contract>();
            list = ct.GetContract(item.CustomerID, item.ContractID);
            Contract contract = new Contract();
            contract = list.FirstOrDefault();

            decimal totalsales = contract.ContractPayment; // ยอดเงินทั้งหมด
            decimal rate = contract.ContractInterest; //ดอกเบี้ย
            decimal amount = contract.ContractAmount; //จำนวนที่จ่ายรายวัน
            decimal priceReciept = item.PriceReceipts;

            decimal interest = item.PriceReceipts * (rate / 100); //ดอกเบี้ย
            decimal Priciple = item.PriceReceipts - interest; // เงินต้น
          

            DataTable dt = new DataTable();

            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);
            dt= DBHelper.List("select * FROM daily_receipts WHERE ContractID="+ item.ContractID +
                "  and PriceReceipts!=Principle  order by PriceReceipts desc",
                ObjConn);

            if (dt.Rows.Count > 0) {

                // ข้อมูลการจ่ายทั้งหมด 
                int TotalReceiptID = Utility.GetMaxID("daily_totalreceipts", "TotalReceiptID");
                StrSql = @" INSERT INTO daily_totalreceipts(TotalReceiptID,CustomerID,ContractID,DateAsOf,
                TotalSales,PriceReceipts,Principle,Interest,StaffID,Activated,Deleted)
                VALUES("
                + TotalReceiptID + ","
                + item.CustomerID + ","
                + item.ContractID + ","
                + Utility.FormateDateTime(DateTime.Now) + ","
                + totalsales + ","
                + item.PriceReceipts + ","
                + (item.PriceReceipts - (item.PriceReceipts * (rate / 100))) + ","
                + (item.PriceReceipts * (rate / 100)) + ","
                + item.StaffID + ","
                + 1 + ","
                + 0 +
                ");";

                DBHelper.Execute(StrSql, ObjConn);

                decimal price = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (priceReciept > 0)
                    {

                        //ครั้งก่อนยังจ่ายไม่เต็ม
                        if (Convert.ToDecimal(dt.Rows[i]["PriceReceipts"]) < amount)
                        {

                            //จำนวนเงินที่ขาด 
                            price = (amount - Convert.ToDecimal(dt.Rows[i]["PriceReceipts"]));
                            if (priceReciept < price)
                            {
                                DBHelper.List("UPDATE daily_receipts SET " +
                           "  PriceReceipts = PriceReceipts+" + priceReciept +
                           "  ,StaffID =" + item.StaffID +
                           "  ,Interest =" + interest +
                           "   WHERE ContractID =" + item.ContractID + " and  DateAsOf='" + Convert.ToDateTime(dt.Rows[i]["DateAsOf"]).ToString(@"yyyy/MM/dd") + "'", ObjConn);
                                priceReciept = priceReciept - price;
                            }
                            else if (priceReciept >= price)
                            {

                                DBHelper.List("UPDATE daily_receipts SET " +
                           "  PriceReceipts = PriceReceipts+" + price +
                           "  ,StaffID =" + item.StaffID +
                           "  ,Interest =" + interest +
                           "   WHERE ContractID =" + item.ContractID + " and  DateAsOf='" + Convert.ToDateTime(dt.Rows[i]["DateAsOf"]).ToString(@"yyyy/MM/dd") + "'", ObjConn);
                                priceReciept = priceReciept - price;
                                

                            }
                          
                            //if (priceReciept >= amount)
                            //{
                            //    price = (amount - Convert.ToDecimal(dt.Rows[i]["PriceReceipts"]));
                            //    priceReciept = priceReciept - price;
                            //    DBHelper.List("UPDATE daily_receipts SET " +
                            //     "  PriceReceipts = PriceReceipts+" + price +
                            //     "  ,StaffID =" + item.StaffID +
                            //     "  ,Interest =" + interest +
                            //     "   WHERE ContractID =" + item.ContractID + " and  DateAsOf='" + Convert.ToDateTime(dt.Rows[i]["DateAsOf"]).ToString(@"yyyy/MM/dd") + "'", ObjConn);

                            //}
                            //else if (priceReciept< amount)   //เงินไม่พอ
                            //{
                            //    // เช็ค ตัวที่ยังไม่ครบ
                            //    if (Convert.ToDecimal(dt.Rows[i]["PriceReceipts"]) < amount){
                            //        if (priceReciept < amount) {
                            //            price = (amount - Convert.ToDecimal(dt.Rows[i]["PriceReceipts"]);

                            //        }
                            //        priceReciept = priceReciept - Convert.ToDecimal(dt.Rows[i]["PriceReceipts"]);

                            //        DBHelper.List("UPDATE daily_receipts SET " +
                            //         "  PriceReceipts = PriceReceipts + " + priceReciept +
                            //         "  ,StaffID =" + item.StaffID +
                            //         "  ,Interest =" + interest +
                            //         "   WHERE ContractID =" + item.ContractID + " and  DateAsOf='" + Convert.ToDateTime(dt.Rows[i]["DateAsOf"]).ToString(@"yyyy/MM/dd") + "'", ObjConn);

                            //    }

                            //    DBHelper.List("UPDATE daily_receipts SET " +
                            //     "  PriceReceipts = PriceReceipts + " + priceReciept +
                            //     "  ,StaffID =" + item.StaffID +
                            //     "  ,Interest =" + interest +
                            //     "   WHERE ContractID =" + item.ContractID + " and  DateAsOf='" + Convert.ToDateTime(dt.Rows[i]["DateAsOf"]).ToString(@"yyyy/MM/dd") + "'", ObjConn);
                            //    priceReciept = priceReciept - priceReciept;
                            //}


                        }//ปัจจุบัน

                    }
                    else {
                        i = dt.Rows.Count;

                    }
                } // loop for


            }

        }




            public void PaymentDailyReceipts2(DailyReceiptsReport item)
        {
            if (item.PriceReceipts <= 0)
            {
                throw new Exception("จำนวนเงินต้องมากว่า 0 บาท");
            }
            
            string StrSql = string.Empty;
            ContractData ct = new ContractData();
            IList<Contract> list = new List<Contract>();
            list = ct.GetContract(item.CustomerID, item.ContractID);
            Contract contract = new Contract();
            contract = list.FirstOrDefault();


            //get วันที่จ่ายล่าสุด
            ReportData data = new ReportData();
            IList<LastTransaction> listData1 = new List<LastTransaction>();
            listData1 = data.GetTransaction(item.StaffID.ToString(), item.CustomerID.ToString(), item.ContractID.ToString());
            listData1 = listData1.OrderByDescending(c => c.DateAsOf).ToArray();
            
            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);
            try
            {
                DateTime LastDate = listData1.FirstOrDefault().DateAsOf;//วันที่จ่านล่าสุด
                decimal LastPay = listData1.FirstOrDefault().Amount;//วันที่จ่านล่าสุด

                decimal totalsales = contract.ContractPayment; // ยอดเงินทั้งหมด
                decimal rate = contract.ContractInterest; //ดอกเบี้ย
                decimal amount = contract.ContractAmount; //จำนวนที่จ่ายรายวัน
                decimal interest = item.PriceReceipts * (rate / 100); //ดอกเบี้ย
                decimal Priciple = item.PriceReceipts - interest; // เงินต้น


                if (item.PriceReceipts <= amount)   // จ่ายพอดี
                {
                    item.ID = Utility.GetMaxID("daily_receipts", "ID");
                    StrSql = @" INSERT INTO daily_receipts(ID,CustomerID,ContractID,DateAsOf,
            TotalSales,PriceReceipts,Principle,Interest,StaffID,Latitude,Longitude,Activated,Deleted)
            VALUES("

                + item.ID + ","
                + item.CustomerID + ","
                + item.ContractID + ","
                + Utility.FormateDateTime(DateTime.Now) + ","
                + totalsales + ","
                + item.PriceReceipts + ","
                + Priciple + ","
                + interest + ","
                + item.StaffID + ","
                + item.Latitude + ","
                + item.Longitude + ","
                + 0 + ","
                + 0 + ");";

                    DBHelper.Execute(StrSql, ObjConn);
                
                }
                else if (item.PriceReceipts > amount)   // จ่ายเกิน
                {


                    decimal price = item.PriceReceipts;
                    int k = 0;
                    while (price >= amount)
                    {

                        item.ID = Utility.GetMaxID("daily_receipts", "ID");
                        StrSql = @" INSERT INTO daily_receipts(ID,CustomerID,ContractID,DateAsOf,
            TotalSales,PriceReceipts,Principle,Interest,StaffID,Latitude,Longitude,Activated,Deleted)
            VALUES("

                    + item.ID + ","
                    + item.CustomerID + ","
                    + item.ContractID + ","
                    + Utility.FormateDateTime(DateTime.Now.AddDays(k)) + ","
                    + totalsales + ","
                    + amount + ","
                    + (amount - (amount * (rate / 100))) + ","
                    + (amount * (rate / 100)) + ","
                    + item.StaffID + ","
                    + item.Latitude + ","
                    + item.Longitude + ","
                    + 0 + ","
                    + 0 + ");";

                        DBHelper.Execute(StrSql, ObjConn);

                        price = price - amount;
                        k = k + 1;


                        //เหลือเศษ
                        if (price < amount)
                        {

                            item.ID = Utility.GetMaxID("daily_receipts", "ID");
                            StrSql = @" INSERT INTO daily_receipts(ID,CustomerID,ContractID,DateAsOf,
            TotalSales,PriceReceipts,Principle,Interest,StaffID,Latitude,Longitude,Activated,Deleted)
            VALUES("

                        + item.ID + ","
                        + item.CustomerID + ","
                        + item.ContractID + ","
                        + Utility.FormateDateTime(DateTime.Now.AddDays(k)) + "," // Get วัน อนาคต
                         + totalsales + ","
                        + price + ","
                        + (price - (price * (rate / 100))) + ","
                        + (price * (rate / 100)) + ","
                        + item.StaffID + ","
                        + item.Latitude + ","
                        + item.Longitude + ","
                        + 0 + ","
                        + 0 + ");";
                            DBHelper.Execute(StrSql, ObjConn);
                        }
                    }
                 

                }


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally {
                ObjConn.Close();
            
            }

        }

        public DateTime GetNextDate(bool ContractSpecialholiday, int ContractPayEveryDay, DateTime ContractStartDate)
        {
            DateTime NextDate=DateTime.Now;
            bool valid = true;

            DateTime[] HolidaysArr = Utility.Holidays(1);
          
            //  เว้นวันหยุด 
            if (ContractSpecialholiday)
            {

                if (ContractPayEveryDay == 1) //ทุกวัน =2 
                {
                    for (int h = 0; h < 2; h++) {
                        if (valid)
                        {
                        // วันที่จะจ่ายเป็นวันหยดหรือไม่
                        if (!Utility.IsHolidays(ContractStartDate, HolidaysArr))
                          NextDate=ContractStartDate;

                           valid = false;
                        }
                        ContractStartDate = ContractStartDate.AddDays(1);
                    }

                    
                }
                else if (ContractPayEveryDay == 2) // จ-ศ =1
                {
                    for (int h = 0; h < 2; h++)
                    {

                        // วันที่จะจ่ายเป็นวันหยุดหรือไม่
                        if (valid)
                        {
                            if (!Utility.IsHolidays(ContractStartDate, HolidaysArr)){
                                NextDate = ContractStartDate;
                                valid = false;
                            }
                            else if (ContractStartDate.DayOfWeek == DayOfWeek.Saturday)
                                NextDate = ContractStartDate.AddDays(2);
                            else if (ContractStartDate.DayOfWeek == DayOfWeek.Sunday)
                                NextDate = ContractStartDate.AddDays(1);

                         
                        }
                        ContractStartDate = ContractStartDate.AddDays(1);
                    }


                }
                else
                {

                    NextDate = ContractStartDate;
                     
                }
            }
            else
            {

                if (ContractPayEveryDay == 1) //ทุกวัน =2 
                {


                    for (int h = 0; h < 2; h++)
                    { 

                        // วันที่จะจ่ายเป็นวันหยุดหรือไม่
                        if (valid)
                        {
                            NextDate = ContractStartDate;
                            valid = false;
                        }
                      
                    }

                }
                else if (ContractPayEveryDay == 2) // จ-ศ =1
                {

                    for (int h = 0; h < 2; h++)
                    {
                        if (valid) {
                            if (ContractStartDate.DayOfWeek < DayOfWeek.Saturday && ContractStartDate.DayOfWeek > DayOfWeek.Sunday)
                                NextDate = ContractStartDate;
                            else if (ContractStartDate.DayOfWeek == DayOfWeek.Saturday)
                                NextDate = ContractStartDate.AddDays(2);
                            else if (ContractStartDate.DayOfWeek== DayOfWeek.Sunday)
                                NextDate  = ContractStartDate.AddDays(1);

                            valid = false;
                        }
                   



                        // ไม่ใช่เสาร์อาทิตย์
                        ContractStartDate = ContractStartDate.AddDays(1);

                    }
                }else{


                    NextDate = ContractStartDate;
                }

            }
        return   NextDate;
        }

        public void AddDailyReceipts(DailyReceiptsReport item) { 
        
        
        
        }
        public void GetContractID(  
              out  decimal totalsales, // ยอดเงินทั้งหมด
              out  decimal rate , //ดอกเบี้ย
               int CustomerID ,
               int ContractID,
             MySqlConnection ObjConn
            ) {

                     totalsales = 0;
                    rate = 0;
                string StrSql = @" Select * From contract where Deleted=0 and Activated=1 ";

                if (CustomerID > 0)
                {

                    StrSql += @" and ContractCustomerID=" + CustomerID;
                }

                if (ContractID > 0)
                {

                    StrSql += @" and ContractID=" + ContractID;
                }
                DataTable dt = DBHelper.List(StrSql, ObjConn);
                if (dt.Rows.Count>0) {

                    totalsales = (decimal)dt.Rows[0]["ContractPayment"];
                    rate = (decimal)dt.Rows[0]["ContractInterest"];
                }

        }


        public Customers GetCustomerInfo_ByID(int id)
        {

            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);

            try
            {

                string StrSql = @"  SELECT c.* ,
                                   s.SubDistrictName as CustomerSubDistrict,
                                   d.DistrictName as CustomerDistrict,
                                   p.ProvinceName as CustomerProvince
                                   FROM customer c 
                                  LEFT OUTER JOIN province p ON c.CustomerProvinceId = p.ProvinceId
                                  LEFT OUTER JOIN district d ON c.CustomerDistrictId = d.DistrictId
                                  LEFT OUTER JOIN subDistrict s ON c.CustomerSubDistrictId = s.SubDistrictId
                                where Deleted=0 ";

                if (id >= 0)
                {

                    StrSql += @" and c.CustomerID=" + id;
                }
                DataTable dt = DBHelper.List(StrSql, ObjConn);
                Customers cust=new Customers();
                IList<Customers> listData = new List<Customers>();
                if (dt != null && dt.Rows.Count > 0)
                {
                    listData = Customers.ToObjectList(dt);
                    cust=listData[0];
                   
                }


                return cust;
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

        public void GetChangeMobilePhone(Customers item) {
               MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);
               try
               {
                   string sqlStr = @"update customer set 
                    CustomerMobile={0},
                    CustomerTelephone={1}
                    where CustomerID={2}";

                   sqlStr = String.Format(sqlStr,
                       Utility.ReplaceString(item.CustomerMobile),
                       Utility.ReplaceString(item.CustomerTelephone),
                      item.CustomerID);


                   DBHelper.Execute(sqlStr, ObjConn);

               }
               catch (Exception ex )
               {

                   throw new Exception(ex.Message);
               }
               finally {
                   ObjConn.Close();
               }
        }

        public List<ListCustomerOnMobile> GetListCustomerOnMobile(int StaffID)
        {

            StaffData st = new StaffData();
            List<Staffs> item = new List<Staffs>();
            item = st.GetStaff(StaffID);
            Staffs staff = new Staffs();
            staff = item.FirstOrDefault();


            List<ListCustomerOnMobile> list = new List<ListCustomerOnMobile>(); 
            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);
            try
            {
                string sqlStr = @"  SELECT  c.CustomerId,ct.ContractID,CONCAT(c.CustomerTitleName,c.CustomerFirstName,' ',c.CustomerLastName) AS CustomerName
                ,ct.ContractStartDate,ct.ContractPeriod,ct.ContractPayEveryDay,ct.ContractSpecialholiday
                ,ct.ContractNumber,ct.ContractExpDate,ct.ContractAmount,ct.ContractPayment,
                 (ct.ContractPayment-d.TotalPay ) AS TotalPay
                 ,(       SELECT  DATE(DateAsOf) 
                 FROM  daily_receipts  WHERE ContractID=ct.ContractID AND CustomerID=c.CustomerId AND  PriceReceipts>0
                 ORDER BY DateAsOf DESC LIMIT 1) AS lastDate
                 FROM customer c 
                LEFT JOIN contract ct ON c.CustomerId=ct.contractCustomerID
                 LEFT JOIN (SELECT SUM(PriceReceipts)AS TotalPay , ContractID ,CustomerID  
                 FROM  daily_receipts
                 WHERE   Deleted=0
                GROUP BY   ContractID ,CustomerID ) d ON 
                d.ContractID=ct.ContractID AND d.CustomerID=ct.ContractCustomerID 
                WHERE  ct.ContractID IS NOT NULL
                 AND ct.contractstatus=1 and ct.Activated=1 and ct.Deleted=0 and  c.Deleted=0
                 ";


                DataTable dt1 = DBHelper.List("select * FROM staffrolepermission where StaffPermissionID=23 AND StaffRoleID=" + staff.StaffRoleID, ObjConn);

                if(dt1.Rows.Count!=1)
                sqlStr += "AND c.SaleID=" + StaffID;


              DataTable dt=   DBHelper.List(sqlStr, ObjConn);
              if (dt != null && dt.Rows.Count > 0)
              {
                  list= ListCustomerOnMobile.ToObjectList(dt);
              }

              return list;

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

        public StaffLogin GetStaffLogin(StaffLogin login)
        {
             MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);
            try
            {

                string strSQL = "select * FROM staff Where Staffcode={0} and staffpassword={1} and Deleted=0 and Activated=1 ";
                strSQL = string.Format(strSQL, Utility.ReplaceString(login.StaffCode), Utility.ReplaceString(Utility.HashPassword(login.StaffPassword)));
                DataTable dt = DBHelper.List(strSQL, ObjConn);
                if (dt != null && dt.Rows.Count > 0)
                {
                    login.StaffID = Convert.ToInt32(dt.Rows[0]["StaffID"].ToString());
                    login.StaffName = dt.Rows[0]["StaffFirstName"].ToString();
                    login.ImageUrl = dt.Rows[0]["StaffImagePath"].ToString();
                    login.StaffRoleID = Convert.ToInt32(dt.Rows[0]["StaffRoleID"].ToString());
                }
                else {

                    string strSQL2 = @"   SELECT s.StaffID,s.StaffCode,s.StaffFirstName,s.StaffImagePath
                                ,s.StaffRoleID,so.Password FROM staff s 
                                LEFT JOIN staff_otp so ON s.StaffID=so.StaffID
                                WHERE s.StaffRoleID=5 AND so.Deleted=0 AND s.Staffcode=" + Utility.ReplaceString(login.StaffCode.Trim()) +
                                "  AND so.Password=" + Utility.ReplaceString(login.StaffPassword.Trim());

                    DataTable dt2 = DBHelper.List(strSQL2, ObjConn);
                    if (dt2.Rows.Count > 0)
                    {
                        login.StaffID = Convert.ToInt32(dt2.Rows[0]["StaffID"].ToString());
                        login.StaffName = dt2.Rows[0]["StaffFirstName"].ToString();
                        login.ImageUrl = dt2.Rows[0]["StaffImagePath"].ToString();
                        login.StaffRoleID = Convert.ToInt32(dt2.Rows[0]["StaffRoleID"].ToString());
                    }
                    else
                    {

                        throw new Exception("รหัสพนักงาน หรือ รหัสผ่าน ไม่สิทธิ์เข้าใช้งานแอพพลิเคชั่นนี้ ,กรุณาตรวจสอบ");

                    }
                    
                }
                return login;
            }
            catch (Exception ex)
            {
                
                throw;
            }
        }

        public StaffLogin GetStaffLoginOnMobile(StaffLogin login)
        {
            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);
            try
            {

               

                string strSQL = @"SELECT * FROM staff s 
                                WHERE s.Staffcode={0}
                                AND s.staffpassword={1} 
                                AND s.Deleted=0 
                                AND s.Activated=1  ";
                strSQL = string.Format(strSQL, Utility.ReplaceString(login.StaffCode.Trim()), Utility.ReplaceString(Utility.HashPassword(login.StaffPassword.Trim())));
                DataTable dt = DBHelper.List(strSQL, ObjConn);
                if (dt != null && dt.Rows.Count > 0)
                {
                    string strSQL1 = @"   SELECT * FROM staffrolepermission  sp
                                    WHERE sp.StaffPermissionID=22
                                    AND staffRoleID=" + Convert.ToInt32(dt.Rows[0]["StaffRoleID"].ToString());

                    DataTable dt1 = DBHelper.List(strSQL1, ObjConn);
                    if (dt1.Rows.Count > 0)
                    {
                        login.StaffID = Convert.ToInt32(dt.Rows[0]["StaffID"].ToString());
                        login.StaffName = dt.Rows[0]["StaffFirstName"].ToString();
                        login.ImageUrl = dt.Rows[0]["StaffImagePath"].ToString();
                        login.StaffRoleID = Convert.ToInt32(dt.Rows[0]["StaffRoleID"].ToString());
                    }
                    
                }
                else
                {
                    string strSQL2 = @"   SELECT s.StaffID,s.StaffCode,s.StaffFirstName,s.StaffImagePath
                                ,s.StaffRoleID,so.Password FROM staff s 
                                LEFT JOIN staff_otp so ON s.StaffID=so.StaffID
                                WHERE s.StaffRoleID=5 AND so.Deleted=0 AND s.Staffcode=" + Utility.ReplaceString(login.StaffCode.Trim()) +
                                "  AND so.Password=" + Utility.ReplaceString(login.StaffPassword.Trim());

                    DataTable dt2 = DBHelper.List(strSQL2, ObjConn);
                    if (dt2.Rows.Count > 0)
                    {
                        login.StaffID = Convert.ToInt32(dt2.Rows[0]["StaffID"].ToString());
                        login.StaffName = dt2.Rows[0]["StaffFirstName"].ToString();
                        login.ImageUrl = dt2.Rows[0]["StaffImagePath"].ToString();
                        login.StaffRoleID = Convert.ToInt32(dt2.Rows[0]["StaffRoleID"].ToString());
                    }
                    else
                    {
                        throw new Exception("รหัสพนักงาน หรือ รหัสผ่าน ไม่ถูกต้อง,กรุณาติดต่อผู้ดูแลระบบ");
                    }
                }
                return login;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}