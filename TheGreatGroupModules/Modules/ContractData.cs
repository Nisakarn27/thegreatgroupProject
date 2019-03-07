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
    public class ContractData
    {
        private string errMsg = "";



        public IList<Contract> GetContract(int CustomerID, int ContractID)
        {

            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);

            try
            {

                string StrSql = @" select * FROM Contract 
                                    where 0=0
                                    and Activated=1
                                   
                                ";
                if (CustomerID > 0)
                {

                    StrSql += "  and ContractCustomerID=" + CustomerID;
                }
                if (ContractID > 0)
                {

                    StrSql += "  and ContractID=" + ContractID;
                }

                StrSql += @" Order by ContractCreateDate ASC";
                DataTable dt = DBHelper.List(StrSql, ObjConn);
                IList<Contract> listData = new List<Contract>();
                if (dt != null && dt.Rows.Count > 0)
                {
                    Contract con = new Contract();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        con = new Contract();

                        if (dt.Rows[i]["ContractID"] != DBNull.Value)
                            con.ContractID = Convert.ToInt32(dt.Rows[i]["ContractID"].ToString());
                        if (dt.Rows[i]["ContractCustomerID"] != DBNull.Value)
                            con.ContractCustomerID = Convert.ToInt32(dt.Rows[i]["ContractCustomerID"].ToString());
                        if (dt.Rows[i]["ContractCreateDate"] != DBNull.Value)
                            con.ContractCreateDate = Convert.ToDateTime(dt.Rows[i]["ContractCreateDate"].ToString());
                        if (dt.Rows[i]["ContractStartDate"] != DBNull.Value)
                            con.ContractStartDate = Convert.ToDateTime(dt.Rows[i]["ContractStartDate"].ToString());
                        if (dt.Rows[i]["ContractExpDate"] != DBNull.Value)
                            con.ContractExpDate = Convert.ToDateTime(dt.Rows[i]["ContractExpDate"].ToString());
                        if (dt.Rows[i]["ContractNumber"] != DBNull.Value)
                            con.ContractNumber = dt.Rows[i]["ContractNumber"].ToString();
                        if (dt.Rows[i]["ContractReward"] != DBNull.Value)
                            con.ContractReward = Convert.ToDecimal(dt.Rows[i]["ContractReward"].ToString());
                        if (dt.Rows[i]["ContractInterest"] != DBNull.Value)
                            con.ContractInterest = Convert.ToDecimal(dt.Rows[i]["ContractInterest"].ToString());
                        if (dt.Rows[i]["ContractAmount"] != DBNull.Value)
                            con.ContractAmount = Convert.ToDecimal(dt.Rows[i]["ContractAmount"].ToString());
                        if (dt.Rows[i]["ContractAmountLast"] != DBNull.Value)
                            con.ContractAmountLast = Convert.ToDecimal(dt.Rows[i]["ContractAmountLast"].ToString());
                        if (dt.Rows[i]["ContractPeriod"] != DBNull.Value)
                            con.ContractPeriod = Convert.ToInt32(dt.Rows[i]["ContractPeriod"].ToString());
                        if (dt.Rows[i]["ContractPayment"] != DBNull.Value)
                            con.ContractPayment = Convert.ToDecimal(dt.Rows[i]["ContractPayment"].ToString());
                        if (dt.Rows[i]["ContractRefNumber"] != DBNull.Value)
                            con.ContractRefNumber = dt.Rows[i]["ContractRefNumber"].ToString();
                        if (dt.Rows[i]["ContractInsertBy"] != DBNull.Value)
                            con.ContractInsertBy = Convert.ToInt32(dt.Rows[i]["ContractInsertBy"].ToString());
                        if (dt.Rows[i]["ContractInsertDate"] != DBNull.Value)
                            con.ContractInsertDate = Convert.ToDateTime(dt.Rows[i]["ContractInsertDate"].ToString());

                        if (dt.Rows[i]["ContractType"] != DBNull.Value)
                            con.ContractType = dt.Rows[i]["ContractType"].ToString();
                        if (dt.Rows[i]["ContractStatus"] != DBNull.Value)
                            con.ContractStatus = Convert.ToInt32(dt.Rows[i]["ContractStatus"].ToString());
                        if (dt.Rows[i]["ContractRemark"] != DBNull.Value)
                            con.ContractRemark = dt.Rows[i]["ContractRemark"].ToString();
                        if (dt.Rows[i]["Activated"] != DBNull.Value)
                            con.Activated = Convert.ToInt32(dt.Rows[i]["Activated"].ToString());
                        if (dt.Rows[i]["Deleted"] != DBNull.Value)
                            con.Deleted = Convert.ToInt32(dt.Rows[i]["Deleted"].ToString());
                        
                        if (dt.Rows[i]["ContractAccountStatus"] != DBNull.Value)
                            con.ContractAccountStatus = Convert.ToInt32(dt.Rows[i]["ContractAccountStatus"].ToString());

                            con.ContractAccountStatusName = Utility.CheckAccountStatus(con.ContractAccountStatus);

                        if (dt.Rows[i]["ContractPayEveryDay"] != DBNull.Value)
                            con.ContractPayEveryDay = Convert.ToInt32(dt.Rows[i]["ContractPayEveryDay"].ToString());
                        if (dt.Rows[i]["ContractSpecialholiday"] != DBNull.Value)
                            con.ContractSpecialholiday = Convert.ToBoolean(dt.Rows[i]["ContractSpecialholiday"].ToString());
                        if (dt.Rows[i]["ContractSuretyID1"] != DBNull.Value)
                            con.CustomerSurety1 = Convert.ToInt32(dt.Rows[i]["ContractSuretyID1"].ToString());
                        if (dt.Rows[i]["ContractSuretyID2"] != DBNull.Value)
                            con.CustomerSurety2 = Convert.ToInt32(dt.Rows[i]["ContractSuretyID2"].ToString());
                        if (dt.Rows[i]["ContractPartner"] != DBNull.Value)
                            con.CustomerPartner = Convert.ToInt32(dt.Rows[i]["ContractPartner"].ToString());
                        if (dt.Rows[i]["IsContractAmountLast"] != DBNull.Value)
                            con.IsContractAmountLast = Convert.ToInt32(dt.Rows[i]["IsContractAmountLast"].ToString());

                        listData.Add(con);
                    }





                }

                //หาส่วนของผู้ค้ำประกัน
                if (listData.Count > 0)
                {
                    CustomerSurety Surety = new CustomerSurety();
                    for (int i = 0; i < listData.Count; i++)
                    {

                        #region ::  ผู้ค้ำคนที่ 1  ::
                        if (listData[i].CustomerSurety1 > 0)
                        {

                            StrSql = @" select cs.*,sd.SubDistrictName,d.DistrictName,p.ProvinceName FROM customer_surety cs
                                        left Join subdistrict sd on  cs.CustomerSuretySubDistrict=sd.SubDistrictId
                                        left Join district d on  cs.CustomerSuretyDistrict=d.DistrictId
                                        LEFT JOIN province p ON  cs.CustomerSuretyProvince=p.ProvinceId
                                         where cs.CustomerSuretyID=" + listData[i].CustomerSurety1;
                            DataTable dt1 = DBHelper.List(StrSql, ObjConn);
                            var dataSurety = new CustomerSurety();

                            if (dt1.Rows.Count > 0)
                            {

                                for (int j = 0; j < dt1.Rows.Count; j++)
                                {
                                    dataSurety = new CustomerSurety();
                                    dataSurety.CustomerSuretyID = Convert.ToInt32(dt1.Rows[j]["CustomerSuretyID"].ToString());
                                    dataSurety.CustomerSuretyTitle = dt1.Rows[j]["CustomerSuretyTitle"].ToString();
                                    dataSurety.CustomerSuretyFirstName = dt1.Rows[j]["CustomerSuretyFirstName"].ToString();
                                    dataSurety.CustomerSuretyLastName = dt1.Rows[j]["CustomerSuretyLastName"].ToString();
                                    dataSurety.CustomerSuretyAddress = dt1.Rows[j]["CustomerSuretyAddress"].ToString();
                                    dataSurety.CustomerSuretySubDistrictId = Convert.ToInt32(dt1.Rows[j]["CustomerSuretySubDistrict"].ToString());
                                    dataSurety.CustomerSuretyDistrictId = Convert.ToInt32(dt1.Rows[j]["CustomerSuretyDistrict"].ToString());
                                    dataSurety.CustomerSuretyProvinceId = Convert.ToInt32(dt1.Rows[j]["CustomerSuretyProvince"].ToString());
                                    dataSurety.CustomerSuretyZipCode = dt1.Rows[j]["CustomerSuretyZipCode"].ToString();
                                    dataSurety.CustomerSuretyIdCard = dt1.Rows[j]["CustomerSuretyIdCard"].ToString();
                                    dataSurety.CustomerSuretyMobile = dt1.Rows[j]["CustomerSuretyMobile"].ToString();
                                    dataSurety.CustomerSuretyTelephone = dt1.Rows[j]["CustomerSuretyTelephone"].ToString();
                                    dataSurety.CustomerSuretySubDistrictName = dt1.Rows[j]["SubDistrictName"].ToString();
                                    dataSurety.CustomerSuretyDistrictName = dt1.Rows[j]["DistrictName"].ToString();
                                    dataSurety.CustomerSuretyProvinceName = dt1.Rows[j]["ProvinceName"].ToString();
                                    listData[i].CustomerSuretyData1 = dataSurety;
                                }
                            }
                        }

                        #endregion ::  ผู้ค้ำคนที่ 1  ::

                        #region ::  ผู้ค้ำคนที่ 2  ::
                        if (listData[i].CustomerSurety2 > 0)
                        {

                            StrSql = @"select cs.*,sd.SubDistrictName,d.DistrictName,p.ProvinceName FROM customer_surety cs
                                        left Join subdistrict sd on  cs.CustomerSuretySubDistrict=sd.SubDistrictId
                                        left Join district d on  cs.CustomerSuretyDistrict=d.DistrictId
                                        LEFT JOIN province p ON  cs.CustomerSuretyProvince=p.ProvinceId
                                         where cs.CustomerSuretyID=" + listData[i].CustomerSurety2;
                            DataTable dt2 = DBHelper.List(StrSql, ObjConn);
                            var dataSurety2 = new CustomerSurety();

                            if (dt2.Rows.Count > 0)
                            {

                                for (int k = 0; k < dt2.Rows.Count; k++)
                                {
                                    dataSurety2 = new CustomerSurety();
                                    dataSurety2.CustomerSuretyID = Convert.ToInt32(dt2.Rows[k]["CustomerSuretyID"].ToString());
                                    dataSurety2.CustomerSuretyTitle = dt2.Rows[k]["CustomerSuretyTitle"].ToString();
                                    dataSurety2.CustomerSuretyFirstName = dt2.Rows[k]["CustomerSuretyFirstName"].ToString();
                                    dataSurety2.CustomerSuretyLastName = dt2.Rows[k]["CustomerSuretyLastName"].ToString();
                                    dataSurety2.CustomerSuretyAddress = dt2.Rows[k]["CustomerSuretyAddress"].ToString();
                                    dataSurety2.CustomerSuretySubDistrictId = Convert.ToInt32(dt2.Rows[k]["CustomerSuretySubDistrict"].ToString());
                                    dataSurety2.CustomerSuretyDistrictId = Convert.ToInt32(dt2.Rows[k]["CustomerSuretyDistrict"].ToString());
                                    dataSurety2.CustomerSuretyProvinceId = Convert.ToInt32(dt2.Rows[k]["CustomerSuretyProvince"].ToString());
                                    dataSurety2.CustomerSuretyZipCode = dt2.Rows[k]["CustomerSuretyZipCode"].ToString();
                                    dataSurety2.CustomerSuretyIdCard = dt2.Rows[k]["CustomerSuretyIdCard"].ToString();
                                    dataSurety2.CustomerSuretyMobile = dt2.Rows[k]["CustomerSuretyMobile"].ToString();
                                    dataSurety2.CustomerSuretyTelephone = dt2.Rows[k]["CustomerSuretyTelephone"].ToString();
                                    dataSurety2.CustomerSuretySubDistrictName = dt2.Rows[k]["SubDistrictName"].ToString();
                                    dataSurety2.CustomerSuretyDistrictName = dt2.Rows[k]["DistrictName"].ToString();
                                    dataSurety2.CustomerSuretyProvinceName = dt2.Rows[k]["ProvinceName"].ToString();
                                    listData[i].CustomerSuretyData2 = dataSurety2;
                                }
                            }
                        }
                        #endregion ::  ผู้ค้ำคนที่ 2  ::


                        #region ::  ข้อมูลผู้ซื้อร่วม   ::
                        if (listData[i].CustomerPartner > 0)
                        {

                            StrSql = @"select * FROM customer_partner where CustomerPartnerID=" + listData[i].CustomerPartner;
                            DataTable dt3 = DBHelper.List(StrSql, ObjConn);
                            var dataPartner = new CustomerPartner();

                            if (dt3.Rows.Count > 0)
                            {

                                for (int k = 0; k < dt3.Rows.Count; k++)
                                {
                                    dataPartner = new CustomerPartner();
                                    dataPartner.CustomerPartnerID = Convert.ToInt32(dt3.Rows[k]["CustomerPartnerID"].ToString());
                                    dataPartner.CustomerPartnerTitle = dt3.Rows[k]["CustomerPartnerTitle"].ToString();
                                    dataPartner.CustomerPartnerFirstName = dt3.Rows[k]["CustomerPartnerFirstName"].ToString();
                                    dataPartner.CustomerPartnerLastName = dt3.Rows[k]["CustomerPartnerLastName"].ToString();
                                    dataPartner.CustomerPartnerAddress = dt3.Rows[k]["CustomerPartnerAddress"].ToString();
                                    dataPartner.CustomerPartnerSubDistrictId = Convert.ToInt32(dt3.Rows[k]["CustomerPartnerSubDistrictId"].ToString());
                                    dataPartner.CustomerPartnerDistrictId = Convert.ToInt32(dt3.Rows[k]["CustomerPartnerDistrictId"].ToString());
                                    dataPartner.CustomerPartnerProvinceId = Convert.ToInt32(dt3.Rows[k]["CustomerPartnerProvinceId"].ToString());
                                    dataPartner.CustomerPartnerZipCode = dt3.Rows[k]["CustomerPartnerZipCode"].ToString();
                                    dataPartner.CustomerPartnerIdCard = dt3.Rows[k]["CustomerPartnerIdCard"].ToString();
                                    dataPartner.CustomerPartnerMobile = dt3.Rows[k]["CustomerPartnerMobile"].ToString();
                                    dataPartner.CustomerPartnerTelephone = dt3.Rows[k]["CustomerPartnerTelephone"].ToString();
                                    listData[i].CustomerPartnerData = dataPartner;
                                }
                            }
                        }
                        #endregion ::   ข้อมูลผู้ซื้อร่วม   ::
                    }

                }
                else
                {
                    Contract con = new Contract();
                    var dataSurety = new CustomerSurety();
                    dataSurety.CustomerSuretyID = con.CustomerSurety1;
                    con.CustomerSuretyData1 = dataSurety;
                    dataSurety = new CustomerSurety();
                    dataSurety.CustomerSuretyID = con.CustomerSurety2;
                    con.CustomerSuretyData2 = dataSurety;
                    var dataPartner1 = new CustomerPartner();
                    con.CustomerPartner = con.CustomerPartner;
                    con.CustomerPartnerData = dataPartner1;


                    listData.Add(con);
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

        public IList<Contract> GetListContract(int CustomerID, int ContractID)
        {

            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);

            try
            {

                string StrSql = @" SELECT ct.* ,CONCAT(c.CustomerTitleName,c.CustomerFirstName,' ',c.CustomerLastName) AS CustomerName ,c.CustomerIdCard
                                    FROM Contract ct
                                    LEFT JOIN customer c ON ct.ContractCustomerID=c.CustomerId 
                                    WHERE 0=0
                                ";
                if (CustomerID > 0)
                {

                    StrSql += "  and ct.ContractCustomerID=" + CustomerID;
                }
                if (ContractID > 0)
                {

                    StrSql += "  and ct.ContractID=" + ContractID;
                }

                StrSql += @" Order by ct.ContractCreateDate ASC";
                DataTable dt = DBHelper.List(StrSql, ObjConn);
                IList<Contract> listData = new List<Contract>();
                if (dt != null && dt.Rows.Count > 0)
                {
                    Contract con = new Contract();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        con = new Contract();
                        if (dt.Rows[i]["CustomerName"] != DBNull.Value)
                            con.ContractCustomerName = dt.Rows[i]["CustomerName"].ToString() + " ( " + dt.Rows[i]["CustomerIdCard"].ToString() + " )";
                        if (dt.Rows[i]["ContractID"] != DBNull.Value)
                            con.ContractID = Convert.ToInt32(dt.Rows[i]["ContractID"].ToString());
                        if (dt.Rows[i]["ContractCustomerID"] != DBNull.Value)
                            con.ContractCustomerID = Convert.ToInt32(dt.Rows[i]["ContractCustomerID"].ToString());
                        if (dt.Rows[i]["ContractCreateDate"] != DBNull.Value)
                            con.ContractCreateDate = Convert.ToDateTime(dt.Rows[i]["ContractCreateDate"].ToString());
                        if (dt.Rows[i]["ContractStartDate"] != DBNull.Value)
                            con.ContractStartDate = Convert.ToDateTime(dt.Rows[i]["ContractStartDate"].ToString());
                        if (dt.Rows[i]["ContractExpDate"] != DBNull.Value)
                            con.ContractExpDate = Convert.ToDateTime(dt.Rows[i]["ContractExpDate"].ToString());
                        if (dt.Rows[i]["ContractNumber"] != DBNull.Value)
                            con.ContractNumber = dt.Rows[i]["ContractNumber"].ToString();
                        if (dt.Rows[i]["ContractReward"] != DBNull.Value)
                            con.ContractReward = Convert.ToDecimal(dt.Rows[i]["ContractReward"].ToString());
                        if (dt.Rows[i]["ContractInterest"] != DBNull.Value)
                            con.ContractInterest = Convert.ToDecimal(dt.Rows[i]["ContractInterest"].ToString());
                        if (dt.Rows[i]["ContractAmount"] != DBNull.Value)
                            con.ContractAmount = Convert.ToDecimal(dt.Rows[i]["ContractAmount"].ToString());
                        if (dt.Rows[i]["ContractAmountLast"] != DBNull.Value)
                            con.ContractAmountLast = Convert.ToDecimal(dt.Rows[i]["ContractAmountLast"].ToString());
                        if (dt.Rows[i]["ContractPeriod"] != DBNull.Value)
                            con.ContractPeriod = Convert.ToInt32(dt.Rows[i]["ContractPeriod"].ToString());
                        if (dt.Rows[i]["ContractPayment"] != DBNull.Value)
                            con.ContractPayment = Convert.ToDecimal(dt.Rows[i]["ContractPayment"].ToString());
                        if (dt.Rows[i]["ContractRefNumber"] != DBNull.Value)
                            con.ContractRefNumber = dt.Rows[i]["ContractRefNumber"].ToString();
                        if (dt.Rows[i]["ContractInsertBy"] != DBNull.Value)
                            con.ContractInsertBy = Convert.ToInt32(dt.Rows[i]["ContractInsertBy"].ToString());
                        if (dt.Rows[i]["ContractInsertDate"] != DBNull.Value)
                            con.ContractInsertDate = Convert.ToDateTime(dt.Rows[i]["ContractInsertDate"].ToString());
                      
                        if (dt.Rows[i]["ContractType"] != DBNull.Value)
                            con.ContractType = dt.Rows[i]["ContractType"].ToString();
                        if (dt.Rows[i]["ContractStatus"] != DBNull.Value)
                            con.ContractStatus = Convert.ToInt32(dt.Rows[i]["ContractStatus"].ToString());
                        if (dt.Rows[i]["ContractRemark"] != DBNull.Value)
                            con.ContractRemark = dt.Rows[i]["ContractRemark"].ToString();
                        if (dt.Rows[i]["Activated"] != DBNull.Value)
                            con.Activated = Convert.ToInt32(dt.Rows[i]["Activated"].ToString());
                        if (dt.Rows[i]["Deleted"] != DBNull.Value)
                            con.Deleted = Convert.ToInt32(dt.Rows[i]["Deleted"].ToString());
                        if (dt.Rows[i]["ContractAccountStatus"] != DBNull.Value)
                            con.ContractAccountStatus = Convert.ToInt32(dt.Rows[i]["ContractAccountStatus"].ToString());

                        con.ContractAccountStatusName = Utility.CheckAccountStatus(con.ContractAccountStatus);

                        if (dt.Rows[i]["ContractPayEveryDay"] != DBNull.Value)
                            con.ContractPayEveryDay = Convert.ToInt32(dt.Rows[i]["ContractPayEveryDay"].ToString());
                        if (dt.Rows[i]["ContractSpecialholiday"] != DBNull.Value)
                            con.ContractSpecialholiday = Convert.ToBoolean(dt.Rows[i]["ContractSpecialholiday"].ToString());
                        if (dt.Rows[i]["ContractSuretyID1"] != DBNull.Value)
                            con.CustomerSurety1 = Convert.ToInt32(dt.Rows[i]["ContractSuretyID1"].ToString());
                        if (dt.Rows[i]["ContractSuretyID2"] != DBNull.Value)
                            con.CustomerSurety2 = Convert.ToInt32(dt.Rows[i]["ContractSuretyID2"].ToString());

                        if (dt.Rows[i]["ContractTimes"] != DBNull.Value)
                            con.ContractTimes = Convert.ToInt32(dt.Rows[i]["ContractTimes"].ToString());
                        if (dt.Rows[i]["ContractCloseDate"] != DBNull.Value)
                            con.ContractCloseDate = Convert.ToDateTime(dt.Rows[i]["ContractCloseDate"].ToString());

                        
                        listData.Add(con);
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

      


        public void ActivatedContract(int ContractID, int UpdateBy)
        {
            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);

            try
            {
                DataTable dt = DBHelper.List("select Activated From Contract Where ContractID=" + ContractID, ObjConn);
                string strSql = @"Update Contract  set Activated={3},ContractUpdateBy={1},ContractUpdateDate={2}
                                where  ContractID={0}";

                if (Convert.ToInt32(dt.Rows[0]["Activated"].ToString()) == 1)
                {
                    strSql = string.Format(strSql, ContractID, UpdateBy, Utility.FormateDateTime(DateTime.Now), 0);

                    DBHelper.Execute(strSql, ObjConn);
                }
                else if (Convert.ToInt32(dt.Rows[0]["Activated"].ToString()) == 0)
                {

                    strSql = string.Format(strSql, ContractID, UpdateBy, Utility.FormateDateTime(DateTime.Now), 1);

                    DBHelper.Execute(strSql, ObjConn);

                }

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


        public int Add_NewContract(Contract item)
        {

            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);
            int ConT_ID = 0;

            try
            {

            DataTable dt =    DBHelper.List("select * FROM  contract where ContractCustomerID="+item.ContractCustomerID , ObjConn); 
                item.ContractID = Utility.GetMaxID("contract", "ContractID");
                string StrSql = @" INSERT INTO  contract
            (ContractID,
             ContractCustomerID,
             ContractCreateDate,
             ContractStartDate,
             ContractExpDate,
             ContractNumber,
             ContractReward,
             ContractInterest,
             ContractAmount,
             ContractAmountLast,
             ContractPeriod,
             ContractPayment,
             ContractRefNumber,
             ContractInsertBy,
             ContractInsertDate,
             ContractType,
             ContractStatus,
             ContractPayEveryDay,
             ContractSpecialholiday,
             ContractRemark,
             ContractTimes,
             Activated,
             Deleted)
VALUES ({0},{1},{2}, {3}, {4},{5}, {6},{7}, {8}, {9},{10},{11},{12}, {13}, {14}, {15},{16}, {17}, {18}, {19}, {20}, {21}, {22} );";


                StrSql = String.Format(StrSql,
                     item.ContractID,
                     item.ContractCustomerID,
                     Utility.FormateDate(item.ContractCreateDate),
                     Utility.FormateDate(item.ContractStartDate),
                     Utility.FormateDate(item.ContractExpDate),
                     Utility.ReplaceString(item.ContractNumber),
                     item.ContractReward,
                     item.ContractInterest,
                     item.ContractAmount,
                     item.ContractAmountLast,
                     item.ContractPeriod,
                     item.ContractPayment,
                      Utility.ReplaceString(item.ContractRefNumber),
                      item.ContractInsertBy,
                       Utility.FormateDateTime(DateTime.Now),
                       Utility.ReplaceString(item.ContractType),
                      1,
                      item.ContractPayEveryDay,
                      item.ContractSpecialholiday,
                      Utility.ReplaceString(item.ContractRemark),
                      (dt.Rows.Count+1),
                      1,
                      0);

                DBHelper.Execute(StrSql, ObjConn);
                ConT_ID = item.ContractID;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            finally
            {


                ObjConn.Close();

            }
            return ConT_ID;

        }

        public void Edit_NewContract(Contract item)
        {

            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);

            try
            {

                string StrSql = @" Update contract Set
             ContractCustomerID={1},
             ContractCreateDate={2},
             ContractStartDate={3},
             ContractExpDate={4},
             ContractNumber={5},
             ContractReward={6},
             ContractInterest={7},
             ContractAmount={8},
             ContractAmountLast={9},
             ContractPeriod={10},
             ContractPayment={11},
             ContractRefNumber={12},
             ContractInsertBy={13},
             ContractInsertDate={14},
             ContractType={15},
             ContractStatus={16},
             ContractPayEveryDay={17},
             ContractSpecialholiday={18},
             ContractRemark={19},
             ContractUpdateDate={20},
             ContractUpdateBy={21},
             Activated={22},
             Deleted={23}
            Where ContractID={0}";


                StrSql = String.Format(StrSql,
                      item.ContractID,
                     item.ContractCustomerID,
                   Utility.FormateDate(item.ContractCreateDate),
                       Utility.FormateDate(item.ContractStartDate),
                       Utility.FormateDate(item.ContractExpDate),
                      Utility.ReplaceString(item.ContractNumber),
                      item.ContractReward,
                     item.ContractInterest,
                      item.ContractAmount,
                      item.ContractAmountLast,
                      item.ContractPeriod,
                      item.ContractPayment,
                      Utility.ReplaceString(item.ContractRefNumber),
                      item.ContractUpdateBy,
                        Utility.FormateDate(DateTime.Now),
                       Utility.ReplaceString(item.ContractType),
                     item.ContractStatus,
                      item.ContractPayEveryDay,
                      item.ContractSpecialholiday,
                      Utility.ReplaceString(item.ContractRemark),
                      Utility.FormateDateTime(DateTime.Now),
                      item.ContractUpdateBy,
                      1,
                      0
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


        public int Add_Surety(CustomerSurety item)
        {

            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);
            item.CustomerSuretyID = Utility.GetMaxID("customer_surety", "CustomerSuretyID");
            try
            {

                string StrSql = @" INSERT INTO  customer_surety
            ( CustomerSuretyID,
             CustomerSuretyTitle,
             CustomerSuretyFirstName,
             CustomerSuretyLastName,
             CustomerSuretyAddress,
             CustomerSuretySubDistrict,
             CustomerSuretyDistrict,
             CustomerSuretyProvince,
             CustomerSuretyZipCode,
             CustomerSuretyIdCard,
             CustomerSuretyMobile,
             CustomerSuretyTelephone)
VALUES ({0},{1},{2}, {3}, {4},{5}, {6},{7}, {8}, {9},{10},{11});";


                StrSql = String.Format(StrSql,
                        item.CustomerSuretyID,
               Utility.ReplaceString(item.CustomerSuretyTitle),
               Utility.ReplaceString(item.CustomerSuretyFirstName),
               Utility.ReplaceString(item.CustomerSuretyLastName),
               Utility.ReplaceString(item.CustomerSuretyAddress),
             item.CustomerSuretySubDistrictId,
             item.CustomerSuretyDistrictId,
             item.CustomerSuretyProvinceId,
               Utility.ReplaceString(item.CustomerSuretyZipCode),
               Utility.ReplaceString(item.CustomerSuretyIdCard),
               Utility.ReplaceString(item.CustomerSuretyMobile),
               Utility.ReplaceString(item.CustomerSuretyTelephone)
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
            return item.CustomerSuretyID;

        }

        public int Update_Surety(CustomerSurety item)
        {

            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);
            try
            {

                string StrSql = @" Update customer_surety set
             CustomerSuretyTitle={1},
             CustomerSuretyFirstName={2},
             CustomerSuretyLastName={3},
             CustomerSuretyAddress={4},
             CustomerSuretySubDistrict={5},
             CustomerSuretyDistrict={6},
             CustomerSuretyProvince={7},
             CustomerSuretyZipCode={8},
             CustomerSuretyIdCard={9},
             CustomerSuretyMobile={10},
             CustomerSuretyTelephone={11}
where CustomerSuretyID= {0};";


                StrSql = String.Format(StrSql,
                        item.CustomerSuretyID,
               Utility.ReplaceString(item.CustomerSuretyTitle),
               Utility.ReplaceString(item.CustomerSuretyFirstName),
               Utility.ReplaceString(item.CustomerSuretyLastName),
               Utility.ReplaceString(item.CustomerSuretyAddress),
             item.CustomerSuretySubDistrictId,
             item.CustomerSuretyDistrictId,
             item.CustomerSuretyProvinceId,
               Utility.ReplaceString(item.CustomerSuretyZipCode),
               Utility.ReplaceString(item.CustomerSuretyIdCard),
               Utility.ReplaceString(item.CustomerSuretyMobile),
               Utility.ReplaceString(item.CustomerSuretyTelephone)
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
            return item.CustomerSuretyID;

        }
        public int Add_Partner(CustomerPartner item)
        {

            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);
            item.CustomerPartnerID = Utility.GetMaxID("customer_partner", "CustomerPartnerID");
            try
            {

                string StrSql = @" INSERT INTO  customer_Partner
            ( CustomerPartnerID,
             CustomerPartnerTitle,
             CustomerPartnerFirstName,
             CustomerPartnerLastName,
             CustomerPartnerAddress,
             CustomerPartnerSubDistrictId,
             CustomerPartnerDistrictId,
             CustomerPartnerProvinceId,
             CustomerPartnerZipCode,
             CustomerPartnerIdCard,
             CustomerPartnerMobile,
             CustomerPartnerTelephone)
VALUES ({0},{1},{2}, {3}, {4},{5}, {6},{7}, {8}, {9},{10},{11});";


                StrSql = String.Format(StrSql,
                        item.CustomerPartnerID,
               Utility.ReplaceString(item.CustomerPartnerTitle),
               Utility.ReplaceString(item.CustomerPartnerFirstName),
               Utility.ReplaceString(item.CustomerPartnerLastName),
               Utility.ReplaceString(item.CustomerPartnerAddress),
             item.CustomerPartnerSubDistrictId,
             item.CustomerPartnerDistrictId,
             item.CustomerPartnerProvinceId,
               Utility.ReplaceString(item.CustomerPartnerZipCode),
               Utility.ReplaceString(item.CustomerPartnerIdCard),
               Utility.ReplaceString(item.CustomerPartnerMobile),
               Utility.ReplaceString(item.CustomerPartnerTelephone)
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
            return item.CustomerPartnerID;

        }
        public int Update_Partner(CustomerPartner item)
        {

            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);
            try
            {

                string StrSql = @" Update customer_Partner  Set
             CustomerPartnerTitle={1},
             CustomerPartnerFirstName={2},
             CustomerPartnerLastName={3},
             CustomerPartnerAddress={4},
             CustomerPartnerSubDistrictId={5},
             CustomerPartnerDistrictId={6},
             CustomerPartnerProvinceId={7},
             CustomerPartnerZipCode={8},
             CustomerPartnerIdCard={9},
             CustomerPartnerMobile={10},
             CustomerPartnerTelephone={11}
            Where   CustomerPartnerID={0}";


                StrSql = String.Format(StrSql,
                        item.CustomerPartnerID,
               Utility.ReplaceString(item.CustomerPartnerTitle),
               Utility.ReplaceString(item.CustomerPartnerFirstName),
               Utility.ReplaceString(item.CustomerPartnerLastName),
               Utility.ReplaceString(item.CustomerPartnerAddress),
             item.CustomerPartnerSubDistrictId,
             item.CustomerPartnerDistrictId,
             item.CustomerPartnerProvinceId,
               Utility.ReplaceString(item.CustomerPartnerZipCode),
               Utility.ReplaceString(item.CustomerPartnerIdCard),
               Utility.ReplaceString(item.CustomerPartnerMobile),
               Utility.ReplaceString(item.CustomerPartnerTelephone)
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
            return item.CustomerPartnerID;

        }
        public void UpdateSurety_In_Contract(Contract item)
        {

            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);
            string StrSql = "";
            try
            {
                StrSql = @" Update contract set 
                ContractSuretyID1={0}
                ,ContractSuretyID2={1}
                ,ContractPartner={2}
                Where ContractID={3}";

                StrSql = String.Format(StrSql, item.CustomerSurety1, item.CustomerSurety2, item.CustomerPartner, item.ContractID);

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
        public void UpdateContractPayment(int ContractID, int CustomerID, double ContractPayment)
        {

            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);
            string StrSql = "";
            try
            {
                StrSql = @" Update contract set 
                ContractPayment={0}
                Where ContractID={1} and ContractCustomerID={2}";

                StrSql = String.Format(StrSql, ContractPayment, ContractID, CustomerID);

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

        public void Update_Product_customer(Contract item)
        {

            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);
            string StrSql = "";
            try
            {
                StrSql = @" Update product_customer set 
                        ContractID={0}
                        Where CustomerID={1} And  ContractID=0";

                StrSql = String.Format(StrSql, item.ContractID, item.ContractCustomerID);

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

        public void Deleted_Product_customer(int CustomerID, int ContractID)
        {

            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);
            string StrSql = "";
            try
            {
                StrSql = @" Delete From  product_customer  
                                  Where CustomerID={1} And  ContractID={0} ;";

                StrSql += @" Delete From  daily_receipts  
                                  Where CustomerID={1} And  ContractID={0} ;";

                StrSql += @"Update contract set ContractPayment=0
                               Where ContractID={0} and ContractCustomerID={1};";

                StrSql = String.Format(StrSql, ContractID, CustomerID);

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
        public void UpdateContractAmount_ContractExpDate(int CustomerID, int ContractID)
        {

            DateTime[] HolidaysArr = Utility.Holidays(1);

            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);
            try
            {

                string StrSql = "";

                StrSql = @" Select * From Contract
                                  Where ContractCustomerID={1} And  ContractID={0} 
                                  and  Activated=1 and Deleted=0 ;";
                StrSql = String.Format(StrSql, ContractID, CustomerID);

                DataTable dt = DBHelper.List(StrSql, ObjConn);


                DBHelper.Execute(@"
                    delete from daily_receipts where CustomerID=" + CustomerID + "  and ContractID=" + ContractID
                    , ObjConn);

                Contract cont = new Contract();
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        //วันที่เริ่มจ่าย
                        if (dt.Rows[i]["ContractStartDate"] != DBNull.Value)
                            cont.ContractStartDate = Convert.ToDateTime(dt.Rows[i]["ContractStartDate"].ToString());

                        // จำนวนงวด
                        if (dt.Rows[i]["ContractPeriod"] != DBNull.Value)
                            cont.ContractPeriod = Convert.ToInt32(dt.Rows[i]["ContractPeriod"].ToString());

                        // เงินต้น
                        if (dt.Rows[i]["ContractPayment"] != DBNull.Value)
                            cont.ContractPayment = Convert.ToDecimal(dt.Rows[i]["ContractPayment"].ToString());

                        // งวดละ
                        if (dt.Rows[i]["ContractAmount"] != DBNull.Value)
                            cont.ContractAmount = Convert.ToDecimal(dt.Rows[i]["ContractAmount"].ToString());

                        // ทุกวัน =2 / จ-ศ =1
                        if (dt.Rows[i]["ContractPayEveryDay"] != DBNull.Value)
                            cont.ContractPayEveryDay = Convert.ToInt32(dt.Rows[i]["ContractPayEveryDay"].ToString());

                        // 1 เว้นวันหยุด  / 0 ไม่เว้นไม่หยุด
                        if (dt.Rows[i]["ContractSpecialholiday"] != DBNull.Value)
                            cont.ContractSpecialholiday = Convert.ToBoolean(dt.Rows[i]["ContractSpecialholiday"].ToString());
                    }

                }

                cont.ContractPeriod = Convert.ToInt32(Math.Ceiling(cont.ContractPayment / cont.ContractAmount));
                int Period = cont.ContractPeriod;
                cont.ContractAmountLast = Convert.ToDecimal(cont.ContractPayment % cont.ContractAmount);
                cont.ContractStartDate = cont.ContractStartDate.AddDays(-1);

                if (cont.ContractPeriod > 0)
                {


                    //  เว้นวันหยุด 
                    if (cont.ContractSpecialholiday)
                    {

                        if (cont.ContractPayEveryDay == 1) //ทุกวัน =2 
                        {
                            while (cont.ContractPeriod > 0)
                            {

                                cont.ContractStartDate = cont.ContractStartDate.AddDays(1);
                                if (!Utility.IsHolidays(cont.ContractStartDate, HolidaysArr)) {
                                    StrSql = @" INSERT INTO daily_receipts(ID,CustomerID,ContractID,DateAsOf,
                                    TotalSales,PriceReceipts,Principle,Interest,StaffID,Activated,Deleted)
                                    VALUES("
                               + Utility.GetMaxID("daily_receipts", "ID") + ","
                               + CustomerID + ","
                               + ContractID + ","
                               + Utility.FormateDateTime(cont.ContractStartDate) + ","
                               + cont.ContractPayment + ","
                               + 0 + ","
                               + cont.ContractAmount + ","
                               + 0 + ","
                               + 0 + ","
                               + 0 + ","
                               + 0 + ")";

                                    DBHelper.Execute(StrSql, ObjConn);

                                    cont.ContractPeriod--;
                                }
                                   

                            }
                            cont.ContractExpDate = cont.ContractStartDate;
                        }
                        else if (cont.ContractPayEveryDay == 2) // จ-ศ =1
                        {
                            while (cont.ContractPeriod > 0)
                            {

                                cont.ContractStartDate = cont.ContractStartDate.AddDays(1);
                                if (cont.ContractStartDate.DayOfWeek < DayOfWeek.Saturday && cont.ContractStartDate.DayOfWeek > DayOfWeek.Sunday && !Utility.IsHolidays(cont.ContractStartDate, HolidaysArr))
                                {
                                            StrSql = @" INSERT INTO daily_receipts(ID,CustomerID,ContractID,DateAsOf,
                                            TotalSales,PriceReceipts,Principle,Interest,StaffID,Activated,Deleted)
                                            VALUES("
                                     + Utility.GetMaxID("daily_receipts", "ID") + ","
                                     + CustomerID + ","
                                     + ContractID + ","
                                     + Utility.FormateDateTime(cont.ContractStartDate) + ","
                                     + cont.ContractPayment + ","
                                     + 0 + ","
                                     + cont.ContractAmount + ","
                                     + 0 + ","
                                     + 0 + ","
                                     + 0 + ","
                                     + 0 + ")";

                                    DBHelper.Execute(StrSql, ObjConn);

                                    cont.ContractPeriod--;
                                }
                            }
                            cont.ContractExpDate = cont.ContractStartDate;
                        }
                    }
                    else
                    {

                        if (cont.ContractPayEveryDay == 1) //ทุกวัน =2 
                        {
                            while (cont.ContractPeriod > 0)
                            {

                                cont.ContractStartDate = cont.ContractStartDate.AddDays(1);

                                StrSql = @" INSERT INTO daily_receipts(ID,CustomerID,ContractID,DateAsOf,
                                            TotalSales,PriceReceipts,Principle,Interest,StaffID,Activated,Deleted)
                                            VALUES("
                                + Utility.GetMaxID("daily_receipts", "ID") + ","
                                + CustomerID + ","
                                + ContractID + ","
                                + Utility.FormateDateTime(cont.ContractStartDate) + ","
                                + cont.ContractPayment + ","
                                + 0 + ","
                                + cont.ContractAmount + ","
                                + 0 + ","
                                + 0 + ","
                                + 0 + ","
                                + 0 + ")";

                                DBHelper.Execute(StrSql, ObjConn);

                                cont.ContractPeriod--;

                            }
                            cont.ContractExpDate = cont.ContractStartDate;
                        }
                        else if (cont.ContractPayEveryDay == 2) // จ-ศ =1
                        {
                            while (cont.ContractPeriod > 0)
                            {

                                cont.ContractStartDate = cont.ContractStartDate.AddDays(1);
                                if (cont.ContractStartDate.DayOfWeek < DayOfWeek.Saturday && cont.ContractStartDate.DayOfWeek > DayOfWeek.Sunday)
                                {
                                    StrSql = @" INSERT INTO daily_receipts(ID,CustomerID,ContractID,DateAsOf,
                                            TotalSales,PriceReceipts,Principle,Interest,StaffID,Activated,Deleted)
                                            VALUES("
                                    + Utility.GetMaxID("daily_receipts", "ID") + ","
                                    + CustomerID + ","
                                    + ContractID + ","
                                    + Utility.FormateDateTime(cont.ContractStartDate) + ","
                                    + cont.ContractPayment + ","
                                    + 0 + ","
                                    + cont.ContractAmount + ","
                                    + 0 + ","
                                    + 0 + ","
                                    + 0 + ","
                                    + 0 + ")";

                                    DBHelper.Execute(StrSql, ObjConn);
                                    cont.ContractPeriod--;
                                }
                            }
                            cont.ContractExpDate = cont.ContractStartDate;
                        }

                    }
                }





                StrSql = @"Update contract set ContractExpDate={2},
                                ContractAmountLast={3},
                                ContractPeriod={4}
                               Where ContractID={0} and ContractCustomerID={1};";

                StrSql = String.Format(StrSql, ContractID, CustomerID, Utility.FormateDate(cont.ContractExpDate), cont.ContractAmountLast, Period);

                DBHelper.Execute(StrSql, ObjConn);



            }
            catch (Exception ex)
            {

                throw;
            }

        }
 

        public List<Holidays> ListHolidays(int id)
        {


            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);
            string StrSql = "";
            try
            {



                StrSql = @" SELECT * FROM holidays WHERE  deleted=0 ";


                StrSql += " and activated=1  ";

                if (id > 0)
                    StrSql += " and ID=" + id;


                DataTable dt = DBHelper.List(StrSql, ObjConn);

                List<Holidays> Holidays = new List<Holidays>(); ;
                Holidays obj = new Holidays();
                if (dt.Rows.Count > 0)
                {

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        obj = new Holidays();
                        obj.ID = Convert.ToInt32(dt.Rows[i]["ID"].ToString());
                        obj.HolidayName = dt.Rows[i]["HolidayName"].ToString();
                        obj.Date = Convert.ToDateTime(dt.Rows[i]["Date"].ToString());
                        obj.Activated = Convert.ToInt32(dt.Rows[i]["Activated"].ToString());
                        obj.Deleted = Convert.ToInt32(dt.Rows[i]["Deleted"].ToString());
                        Holidays.Add(obj);
                    }

                }

                return Holidays;

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
        public List<DailyReceiptsReport> GetApproveOpen_CloseContract(string custpmerIDCard, string ContractStatus)
        {
            DateTime dateAsOf = DateTime.Now;
            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);

            try
            {

                string StrSql = @"    SELECT c.CustomerID,c.CustomerMobile, ct.ContractID ,CONCAT(c.CustomerTitleName,c.CustomerFirstName, '  ', c.CustomerLastName)AS  CustomerName ,c.CustomerNickName,                                  
                                     ct.ContractNumber,ct.ContractCreateDate,ct.ContractExpDate ,ct.ContractAmount,
                                     ct.ContractPayment,SUM( a.PriceReceipts ) AS PriceReceipts,ct.ContractAmountLast,
                                    Case When a.Activated>0 then 'ตรวจสอบแล้ว' else 'รอการตรวจสอบ' end as Status, 
                                    Case When a.Remark  IS NULL then '' else a.Remark end as Remark, 
                                    ( SELECT  ct.ContractPayment- SUM(d.PriceReceipts)  FROM  daily_receipts d
                                    WHERE  d.Deleted=0 AND d.ContractID=ct.ContractID
                                    AND DATE(d.DateAsOf)<='" + dateAsOf.ToString("yyyy-MM-dd") + "')AS Balance " +
                                   @" FROM daily_receipts a
                                    LEFT JOIN Customer c ON  a.CustomerID= c.CustomerId
                                    LEFT JOIN contract ct ON  a.ContractID= ct.ContractID
                                    WHERE 0=0   
                                    AND a.Deleted=0 and ct.Deleted=0  ";

                if (!string.IsNullOrEmpty(custpmerIDCard))
                    StrSql += "  AND  c.CustomerIdCard=" + Utility.ReplaceString(custpmerIDCard);

                if (!string.IsNullOrEmpty(ContractStatus))
                    StrSql += "  AND  ct.ContractStatus=" + ContractStatus;

                StrSql += @"  GROUP BY  a.CustomerID ORDER BY ct.ContractCreateDate  ";


                DataTable dt = DBHelper.List(StrSql, ObjConn);
                List<DailyReceiptsReport> listData = new List<DailyReceiptsReport>();
                if (dt != null && dt.Rows.Count > 0)
                {
                    listData = dt.AsEnumerable().Select(dr => new DailyReceiptsReport()
                    {
                        CustomerID = dr.Field<int>("CustomerID"),
                        ContractID = dr.Field<int>("ContractID"),
                        ContractNumber = dr.Field<string>("ContractNumber"),
                        CustomerName = dr.Field<string>("CustomerName"),
                        ContractCreateDate = dr.Field<DateTime>("ContractCreateDate"),
                        ContractExpDate = dr.Field<DateTime>("ContractExpDate"),
                        ContractAmount = dr.Field<decimal>("ContractAmount"),
                        PriceReceipts = dr.Field<decimal>("PriceReceipts"),
                        Balance = dr.Field<decimal>("Balance"),
                        TotalSales = dr.Field<decimal>("ContractPayment"),
                        ContractAmountLast = dr.Field<decimal>("ContractAmountLast"),
                        Status = dr.Field<string>("Status"),
                        Remark = dr.Field<string>("Remark"),
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

        public int Add_DailyRemark(DailyRemark item)
        {

            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);
            item.ID = Utility.GetMaxID("daily_remark", "ID");
            try
            {

                string StrSql = @"INSERT INTO daily_remark
                        (ID,
                         DateAsOf,
                         ContractID,
                         CustomerID,
                         Remark)
                        VALUES ({0},{1},{2},{3},{4})";
                StrSql = string.Format(StrSql, item.ID, Utility.FormateDateTime(DateTime.Now),
                    item.ContractID, item.CustomerID, item.Remark);
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
            return item.ID;

        }

        public List<DailyRemark> GetListDailyRemark(int CustomerID, int ContractID)
        {


            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);
            string StrSql = "";
            try
            {



                StrSql = @" SELECT * FROM daily_remark where  CustomerID=" + CustomerID + " and ContractID=" + ContractID;
                StrSql += " Order By DateAsOf DESC";


                DataTable dt = DBHelper.List(StrSql, ObjConn);

                List<DailyRemark> list = new List<DailyRemark>(); ;
                DailyRemark obj = new DailyRemark();
                if (dt.Rows.Count > 0)
                {

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        obj = new DailyRemark();
                        obj.ID = Convert.ToInt32(dt.Rows[i]["ID"].ToString());
                        obj.Remark = dt.Rows[i]["Remark"].ToString();
                        obj.DateAsOf = Convert.ToDateTime(dt.Rows[i]["DateAsOf"].ToString());
                        list.Add(obj);
                    }

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

        public void AddDiscount(DailyReceiptsReport item)
        {



            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);
            item.ID = Utility.GetMaxID("discount", "ID");
            try
            {

                string StrSql = @"UPDATE contract SET ContractStatus=0 
                                                     ,ContractCloseDate={5}
                                                        WHERE ContractCustomerID={1} AND ContractID={2} ;

                    INSERT INTO discount
                                (ID,
                                 CustomerID,
                                 ContractID,
                                 Discount,
                                 ApproveBy,
                                 ApproveDate,
                                 Deleted)
                        VALUES ({0},{1},{2},{3},{4},{5},{6});";
                StrSql = string.Format(StrSql,

                    item.ID, item.CustomerID, item.ContractID, Math.Round(item.PriceReceipts, 2), item.StaffID,
                    Utility.FormateDateTime(DateTime.Now),
                     0);
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


        public List<ReportCustomer> GetPaymentReportByCustomer(SearchCriteria search) 
        {
     

            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);
            try
            {
                List<ReportCustomer> list = new List<ReportCustomer>();

                string StrSql = @" SELECT CustomerID,CustomerID,DateAsOf,DAY(DateAsOf) AS days ,
                        MONTH(DateAsOf) AS months ,YEAR(DateAsOf) AS years,Sum(PriceReceipts) as PriceReceipts
                        FROM daily_totalreceipts 
                        WHERE  ContractID=" + search.ContractID+ @" AND PriceReceipts > 0
                            Group by CustomerID,CustomerID,DAY(DateAsOf) ";
                StrSql += " ORDER BY DAY(DateAsOf) ASC";

                DataTable dt1 = DBHelper.List(StrSql, ObjConn);
                DataTable dt = new DataTable();
                DateTime dtHoliday;
                int lastDay = 0;
                for (int i = 0; i < 13; i++)
                {

                    if (i == 0)
                        dt.Columns.Add(("Day").ToString(), typeof(int));
                    else
                    {
                        lastDay = DateTime.DaysInMonth(DateTime.Now.Year, i);
                        dt.Columns.Add("Month" + (i).ToString(), typeof(string));
                    }






                    for (int j = 1; j <= 31; j++)
                    {
                        //column วันที่
                        if (i == 0)
                        {
                            dt.Rows.Add(j);
                        }
                        else
                        {
                            if (j > lastDay)
                            {
                                dt.Rows[j - 1][i] = "-99";
                            }
                            else if (j <= lastDay)
                            {

                                if (dt1.Rows.Count > 0) {


                                
                                //เช็คเสาร์อาทิตย์
                                dtHoliday = new DateTime(Convert.ToInt32(dt1.Rows[0]["years"].ToString()), i, j);

                                

                                        if (dtHoliday.DayOfWeek == DayOfWeek.Saturday)
                                        {


                                            dt.Rows[j - 1][i] = "ส.";

                                        }

                                        if (dtHoliday.DayOfWeek == DayOfWeek.Sunday)
                                        {


                                            dt.Rows[j - 1][i] = "อา.";

                                        }

                                }

                            }

                        }
                    }
                }


                if (dt1.Rows.Count>0) {


                    ReportCustomer obj = new ReportCustomer();

                    for (int j = 0; j< dt1.Rows.Count; j++)
                    {

                        dt.Rows[Convert.ToInt32(dt1.Rows[j]["days"].ToString()) - 1][Convert.ToInt32(dt1.Rows[j]["months"].ToString())] = Convert.ToDecimal(dt1.Rows[j]["PriceReceipts"].ToString());
                   
                    }

                
                }


                if (dt.Rows.Count > 0)
                {

                    ReportCustomer obj = new ReportCustomer();
                    decimal n = 0;
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        obj = new ReportCustomer();
                        obj.Day = Convert.ToInt32(dt.Rows[j]["Day"].ToString());
                        if (dt.Rows[j]["Month1"]!=DBNull.Value)
                        obj.Month1 = decimal.TryParse(dt.Rows[j]["Month1"].ToString(), out n) ?Convert.ToDecimal(dt.Rows[j]["Month1"].ToString()):0;
                        if (dt.Rows[j]["Month2"] != DBNull.Value)
                        obj.Month2 = decimal.TryParse(dt.Rows[j]["Month2"].ToString(), out n) ? Convert.ToDecimal(dt.Rows[j]["Month2"].ToString()) : 0;
                        if (dt.Rows[j]["Month3"] != DBNull.Value)
                        obj.Month3 = decimal.TryParse(dt.Rows[j]["Month3"].ToString(), out n) ? Convert.ToDecimal(dt.Rows[j]["Month3"].ToString()) : 0;
                        if (dt.Rows[j]["Month4"] != DBNull.Value)
                        obj.Month4 = decimal.TryParse(dt.Rows[j]["Month4"].ToString(), out n) ? Convert.ToDecimal(dt.Rows[j]["Month4"].ToString()) : 0;
                        if (dt.Rows[j]["Month5"] != DBNull.Value)
                        obj.Month5 = decimal.TryParse(dt.Rows[j]["Month5"].ToString(), out n) ? Convert.ToDecimal(dt.Rows[j]["Month5"].ToString()) : 0;
                        if (dt.Rows[j]["Month6"] != DBNull.Value)
                        obj.Month6 = decimal.TryParse(dt.Rows[j]["Month6"].ToString(), out n) ? Convert.ToDecimal(dt.Rows[j]["Month6"].ToString()) : 0;
                        if (dt.Rows[j]["Month7"] != DBNull.Value)
                        obj.Month7 = decimal.TryParse(dt.Rows[j]["Month7"].ToString(), out n) ? Convert.ToDecimal(dt.Rows[j]["Month7"].ToString()) : 0;
                        if (dt.Rows[j]["Month8"] != DBNull.Value)
                        obj.Month8 = decimal.TryParse(dt.Rows[j]["Month8"].ToString(), out n) ? Convert.ToDecimal(dt.Rows[j]["Month8"].ToString()) : 0;
                        if (dt.Rows[j]["Month9"] != DBNull.Value)
                        obj.Month9 = decimal.TryParse(dt.Rows[j]["Month9"].ToString(), out n) ? Convert.ToDecimal(dt.Rows[j]["Month9"].ToString()) : 0;
                        if (dt.Rows[j]["Month10"] != DBNull.Value)
                        obj.Month10 = decimal.TryParse(dt.Rows[j]["Month10"].ToString(), out n) ? Convert.ToDecimal(dt.Rows[j]["Month10"].ToString()) : 0;
                        if (dt.Rows[j]["Month11"] != DBNull.Value)
                        obj.Month11 = decimal.TryParse(dt.Rows[j]["Month11"].ToString(), out n) ? Convert.ToDecimal(dt.Rows[j]["Month11"].ToString()) : 0;
                        if (dt.Rows[j]["Month12"] != DBNull.Value)
                        obj.Month12 = decimal.TryParse(dt.Rows[j]["Month12"].ToString(), out n) ? Convert.ToDecimal(dt.Rows[j]["Month12"].ToString()) : 0;
                        list.Add(obj);

                    }

                }





                return list;


            }
            catch (Exception ex)
            {

                throw;
            }
            finally {

                ObjConn.Close();
            
            }
        
        }

        public List<ReportCustomerOnCard> GetPayment_OnCard(int ContractID)
        {
      

            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);
            try
            {
                List<ReportCustomerOnCard> list = new List<ReportCustomerOnCard>();

                string StrSql = @" SELECT ContractCustomerID,ContractID,ContractStartDate,
                        DAY(ContractStartDate) AS days ,
                        MONTH(ContractStartDate) AS months ,
                        YEAR(ContractStartDate) AS years,
                        DAY(ContractExpDate) AS Expdays ,
                        MONTH(ContractExpDate) AS Expmonths ,
                        YEAR(ContractExpDate) AS Expyears,
                        ContractExpDate ,
                        ContractAmount,
                        ContractAmountLast,
                        ContractPayEveryDay
                        FROM contract 
                        WHERE  ContractID=" + ContractID;

                StrSql += " ORDER BY ContractStartDate ASC";

                DataTable dt1 = DBHelper.List(StrSql, ObjConn);
                DataTable dt = new DataTable();
                int days = 0;
                int lastDay = 0;
                DateTime dtHoliday;
                for (int i = 0; i < 13; i++)
                {

                    if (i == 0)
                        dt.Columns.Add(("Day").ToString(), typeof(int));
                    else{
                           lastDay = DateTime.DaysInMonth(DateTime.Now.Year, i); 
                           dt.Columns.Add("Month" + (i).ToString(), typeof(string));
                    }
                     
                    
                    for (int j = 1; j <= 31; j++)
                    {
                        //column วันที่ 
                        if (i == 0)
                        {
                            dt.Rows.Add(j);
                        }
                        else
                        {
                            // เช็ควันสุดท้ายของเดือน
                            if (j > lastDay)
                            {
                                dt.Rows[j - 1][i] = "*";
                            }
                            else if (j <= lastDay)
                            {

                                //เช็คเสาร์อาทิตย์
                                dtHoliday = new DateTime(Convert.ToInt32(dt1.Rows[0]["years"].ToString()), i , j);


                                if (dtHoliday >= Convert.ToDateTime(dt1.Rows[0]["ContractStartDate"].ToString()))
                                {
                                    if (dtHoliday <= Convert.ToDateTime(dt1.Rows[0]["ContractExpDate"].ToString())) {

                                        if (Convert.ToInt32(dt1.Rows[0]["ContractPayEveryDay"].ToString()) != 1) { 
                                        if (dtHoliday.DayOfWeek == DayOfWeek.Saturday)
                                        {


                                            dt.Rows[j - 1][i] = "ส.";

                                        }

                                        if (dtHoliday.DayOfWeek == DayOfWeek.Sunday)
                                        {


                                            dt.Rows[j - 1][i] = "อา.";

                                        }
                                        }
                                    }
                                  
                                }
                              
                            }

                        }

                   



                    }
                }

                int startDate = 0;
                int lastDate = 0;
                int year = 0;
                int month = 0;
                if (dt1.Rows.Count > 0)
                {


                    ReportCustomerOnCard obj = new ReportCustomerOnCard();

                    for (int j = 0; j < dt1.Rows.Count; j++)
                    {
                        startDate = Convert.ToInt32(dt1.Rows[j]["days"]);
                        lastDate = Convert.ToInt32(dt1.Rows[j]["Expdays"]);
                        year = Convert.ToInt32(dt1.Rows[j]["Expyears"]);
                        month = Convert.ToInt32(dt1.Rows[j]["Expmonths"]);
                        if (DateTime.DaysInMonth(year, month)== lastDate) {
                            dt.Rows[0][month+1] = /* Convert.ToDecimal(dt1.Rows[j]["ContractAmountLast"]).ToString("#,##0.00")+ */"<<<";
                         }
                        dt.Rows[startDate-2][Convert.ToInt32(dt1.Rows[j]["months"].ToString())] = ">>>";//+Convert.ToDecimal(dt1.Rows[j]["ContractAmount"]).ToString("#,##0.00");

                        //dt.Rows[lastDate - 1][Convert.ToInt32(dt1.Rows[j]["Expmonths"].ToString())] = /* Convert.ToDecimal(dt1.Rows[j]["ContractAmountLast"]).ToString("#,##0.00")+ */"<<<";

                        //dt.Rows[Convert.ToInt32(dt1.Rows[j]["days"].ToString()) -2][Convert.ToInt32(dt1.Rows[j]["months"].ToString())] = ">>>";//+Convert.ToDecimal(dt1.Rows[j]["ContractAmount"]).ToString("#,##0.00");
                        //dt.Rows[Convert.ToInt32(dt1.Rows[j]["Expdays"].ToString())-1][Convert.ToInt32(dt1.Rows[j]["Expmonths"].ToString())] = /* Convert.ToDecimal(dt1.Rows[j]["ContractAmountLast"]).ToString("#,##0.00")+ */"<<<";

                    
                    }


                }


                if (dt.Rows.Count > 0)
                {

                    ReportCustomerOnCard obj = new ReportCustomerOnCard();

                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        obj = new ReportCustomerOnCard();
                        obj.Day = Convert.ToInt32(dt.Rows[j]["Day"].ToString());
                        if (dt.Rows[j]["Month1"] != DBNull.Value)
                            obj.Month1 = dt.Rows[j]["Month1"].ToString();
                        if (dt.Rows[j]["Month2"] != DBNull.Value)
                            obj.Month2 = dt.Rows[j]["Month2"].ToString();
                        if (dt.Rows[j]["Month3"] != DBNull.Value)
                            obj.Month3 = dt.Rows[j]["Month3"].ToString();
                        if (dt.Rows[j]["Month4"] != DBNull.Value)
                            obj.Month4 = dt.Rows[j]["Month4"].ToString();
                        if (dt.Rows[j]["Month5"] != DBNull.Value)
                            obj.Month5 = dt.Rows[j]["Month5"].ToString();
                        if (dt.Rows[j]["Month6"] != DBNull.Value)
                            obj.Month6 = dt.Rows[j]["Month6"].ToString();
                        if (dt.Rows[j]["Month7"] != DBNull.Value)
                            obj.Month7 =dt.Rows[j]["Month7"].ToString();
                        if (dt.Rows[j]["Month8"] != DBNull.Value)
                            obj.Month8 = dt.Rows[j]["Month8"].ToString();
                        if (dt.Rows[j]["Month9"] != DBNull.Value)
                            obj.Month9 =dt.Rows[j]["Month9"].ToString();
                        if (dt.Rows[j]["Month10"] != DBNull.Value)
                            obj.Month10 = dt.Rows[j]["Month10"].ToString();
                        if (dt.Rows[j]["Month11"] != DBNull.Value)
                            obj.Month11 = dt.Rows[j]["Month11"].ToString();
                        if (dt.Rows[j]["Month12"] != DBNull.Value)
                            obj.Month12 = dt.Rows[j]["Month12"].ToString();
                        list.Add(obj);

                    }

                }





                return list;


            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {

                ObjConn.Close();

            }

        }

    }
}