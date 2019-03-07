using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using TheGreatGroupModules.Models;

namespace TheGreatGroupModules.Modules
{
    public class SettingData
    {

        private string errMsg = "";

        public List<GoldData> GetPriceGold()
        {


            string jsonString = new WebClient().DownloadString(@"http://www.thaigold.info/RealTimeDataV2/gtdata_.txt");

            var list = JsonConvert.DeserializeObject<List<GoldData>>(jsonString);
            string GoldBuy_Text = list[4].bid;
            string GoldSale_Text = list[4].ask;

            List<GoldData> datanew = new List<GoldData>();
            GoldData datagold = new GoldData();
            datagold.name = "T1"; // ทองคำแท่ง 
            datagold.bid = Convert.ToDouble(GoldBuy_Text).ToString("#,##0.00"); //
            datagold.ask = Convert.ToDouble(GoldSale_Text).ToString("#,##0.00");
            datanew.Add(datagold);

            datagold = new GoldData();
            datagold.name = "T2"; // ทองคำรูปพรรณ 
            datagold.bid = (Math.Round((Convert.ToDouble(GoldBuy_Text) - (Convert.ToDouble(GoldBuy_Text) * 0.018)) / 15.16, 0) * 15.16).ToString("#,##0.00");
            datagold.ask = (Convert.ToDouble(GoldSale_Text) + 500).ToString("#,##0.00");
            datanew.Add(datagold);
            return datanew;
        }
        public List<Province> GetProvince()
        {

            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);

            try
            {

                string StrSql = @" SELECT
                                      ProvinceId,
                                      ProvinceName
                                   FROM province where 0=0";
                StrSql += @" Order by ProvinceName ASC";
                DataTable dt = DBHelper.List(StrSql, ObjConn);
                List<Province> listData = new List<Province>();
                if (dt != null && dt.Rows.Count > 0)
                {
                   listData = Province.ToObjectList(dt);
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

        public List<District> GetDistrict(int id)
        {

            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);

            try
            {

                string StrSql = @"  SELECT
                                      DistrictId,
                                      DistrictName,
                                      ProvinceId
                                    FROM district where 0=0";
                if (id > 0)
                {

                    StrSql += @" and ProvinceId=" + id;
                }

                StrSql += @" Order by DistrictName ASC";
                DataTable dt = DBHelper.List(StrSql, ObjConn);

                List<District> listData = new List<District>();
                if (dt != null && dt.Rows.Count > 0)
                {
                    listData = District.ToObjectList(dt);
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

        public List<SubDistrict> GetSubDistrict(int id)
        {

            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);

            try
            {

                string StrSql = @"  SELECT
                                      SubDistrictId,
                                      SubDistrictName,
                                      SubDistrictZipCode,
                                      DistrictId
                                    FROM subdistrict where 0=0 ";
                if (id > 0)
                {

                    StrSql += @" and DistrictId=" + id;
                }
                StrSql += @" Order by SubDistrictName ASC";
                DataTable dt = DBHelper.List(StrSql, ObjConn);
                List<SubDistrict> listData = new List<SubDistrict>();
                if (dt != null && dt.Rows.Count > 0)
                {
                    listData = SubDistrict.ToObjectList(dt);
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

        public List<SubDistrict> GetZipCode(int id)
        {

            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);

            try
            {

                string StrSql = @"  SELECT
                                      SubDistrictId,
                                      SubDistrictName,
                                      SubDistrictZipCode,
                                      DistrictId
                                    FROM subdistrict where 0=0 ";
                if (id > 0)
                {

                    StrSql += @" and SubDistrictId=" + id;
                }
                StrSql += @" Order by SubDistrictName ASC";
                DataTable dt = DBHelper.List(StrSql, ObjConn);
                List<SubDistrict> listData = new List<SubDistrict>();
                if (dt != null && dt.Rows.Count > 0)
                {
                    listData = SubDistrict.ToObjectList(dt);
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


        public void AddZone(Zone zone  )
        {

            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);
            zone.ZoneID = Utility.GetMaxID("zone", "ZoneID");
            try
            {

                string strSql = @"
                    INSERT INTO  zone
                                (ZoneID,
                                 ZoneCode,
                                 ZoneName,
                                 Activated,
                                 Deleted)
                    VALUES ({0},
                            {1},
                            {2},
                            {3},
                            {4});";

                strSql = string.Format(strSql,
                    zone.ZoneID, 
                     Utility.ReplaceString(zone.ZoneCode),
                     Utility.ReplaceString(zone.ZoneName), 
                     1,
                     0);
                DBHelper.Execute(strSql, ObjConn);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            finally {
                ObjConn.Close();
            
            }
            }




        public void DetleteZone(int ZoneID)
        {

            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);
            try
            {
                string strSql = @"Update zone set Deleted=1 
                                where  ZoneID=" + ZoneID;
               
                DBHelper.Execute(strSql, ObjConn);
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
        public void AddStaffZone(StaffZone staffzone)
        {

            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);
            try
            {

                DataTable dt = DBHelper.List(string.Format("Select * FROM staff_zone where StaffID={0} ", staffzone.StaffID), ObjConn);
                if (dt.Rows.Count > 0)
                    throw new Exception("เลือกพนักงานซ้ำ");

                string strSql = @"
                  INSERT INTO staff_zone
                (StaffID,
                 ZoneID)
                VALUES ({0},
                        {1});";

                strSql = string.Format(strSql,staffzone.StaffID, staffzone.ZoneID);
                DBHelper.Execute(strSql, ObjConn);
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

        public void DeleteStaffZone(StaffZone staffzone)
        {

            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);
            try
            {
                string strSql = @" Delete from staff_zone where StaffID={0} and ZoneID={1} ;";
                strSql = string.Format(strSql, staffzone.StaffID, staffzone.ZoneID);
                DBHelper.Execute(strSql, ObjConn);
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
        public void EditZone(Zone zone)
        {

            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);
       
            try
            {
                string strSql = @"
                    Update   zone set
                                 ZoneCode={1},
                                 ZoneName={2},
                                 Activated={3},
                                 Deleted={4}
                    where  ZoneID={0} ;";

                strSql = string.Format(strSql,
                    zone.ZoneID,
                     Utility.ReplaceString(zone.ZoneCode),
                     Utility.ReplaceString(zone.ZoneName),
                     zone.Activated,
                     0);
                DBHelper.Execute(strSql, ObjConn);
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


        public void AddHoliday(Holidays item)
        {
            DateTime dateAsOf = DateTime.ParseExact(item.Date_str, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);
            item.ID = Utility.GetMaxID("holidays", "ID");
            try
            {

                string strSql = @"
                    INSERT INTO  holidays
                                (ID,
                                 Date,
                                 HolidayName,
                                 Activated,
                                 Deleted)
                    VALUES ({0},
                            {1},
                            {2},
                            {3},
                            {4});";

                strSql = string.Format(strSql,
                    item.ID,
                     Utility.FormateDate(dateAsOf),
                     Utility.ReplaceString(item.HolidayName),
                     1,
                     0);
                DBHelper.Execute(strSql, ObjConn);
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


        public void EditHoliday(Holidays item)
        {
            DateTime dateAsOf = DateTime.ParseExact(item.Date_str, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);
           
            try
            {

                string strSql = @"
                    Update   holidays Set 
                                 Date= {1},
                                 HolidayName= {2}
                    Where ID={0}";
                   

                strSql = string.Format(strSql,
                    item.ID,
                     Utility.FormateDate(dateAsOf),
                     Utility.ReplaceString(item.HolidayName)
                 );
                DBHelper.Execute(strSql, ObjConn);
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


        public void DeleteHoliday(int holidayId)
        {
            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);
           
            try
            {

                string strSql = @"  Update   holidays  Set   Deleted=1
                                    Where ID={0}";
                   

                strSql = string.Format(strSql,holidayId);
                
                DBHelper.Execute(strSql, ObjConn);
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


        public StaffZone GetStaffZone(int StaffID)
        {

            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);
            try
            {
                string strSql = @"Select * From Staff s 
                    left join staff_zone sz on s.StaffID=sz.StaffID
                    LEFT JOIN zone z on sz.ZoneID=z.ZoneID
                    where s.StaffID=" + StaffID;


    
             DataTable dt=   DBHelper.List(strSql, ObjConn);
             StaffZone sz = new StaffZone();
             if (dt.Rows.Count > 0) {
                 for (int i = 0; i < dt.Rows.Count; i++)
                 {
                     sz = new StaffZone();
                     sz.StaffID = Convert.ToInt32(dt.Rows[i]["StaffID"]);
                     sz.StaffName = dt.Rows[i]["StaffFirstName"].ToString();
                     sz.ZoneName = dt.Rows[i]["ZoneCode"].ToString()+ "-" + dt.Rows[i]["ZoneName"].ToString();
                  }

             }
             return sz;
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

        public IList<Customers> GetListCustomersByZone(int id)
        {

            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);

            try
            {

                string StrSql = @" 
                     SELECT c.* , ct.ContractNumber,ct.ContractID,
                                                       s.SubDistrictName AS CustomerSubDistrict,
                                                       d.DistrictName AS CustomerDistrict,
                                                       p.ProvinceName AS CustomerProvince
                                                       FROM contract ct 
                                                      LEFT OUTER JOIN customer c ON ct.ContractCustomerID =  c.CustomerID
                                                      LEFT OUTER JOIN province p ON c.CustomerProvinceId = p.ProvinceId
                                                      LEFT OUTER JOIN district d ON c.CustomerDistrictId = d.DistrictId
                                                      LEFT OUTER JOIN subDistrict s ON c.CustomerSubDistrictId = s.SubDistrictId
                                                      WHERE c.Deleted=0  AND ct.Deleted=0  AND c.saleID IN (
                    SELECT s.StaffID  FROM  staff s 
                    LEFT JOIN staff_zone sz ON s.StaffID=sz.StaffID
                    WHERE zoneID="+ id + ")";

               
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
    }
}