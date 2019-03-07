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
    public class StaffData
    {
        private string errMsg = "";

        public DataTable GetZone()
        {

            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);

            try
            {

                string StrSql = @"  SELECT zoneid ,zonecode,CONCAT(zonecode,'-',zonename) AS zonename 
                                    FROM zone
                                    WHERE Activated=1 AND Deleted=0 
                                    Order By zonecode ASC";


                DataTable dt = DBHelper.List(StrSql, ObjConn);

                return dt;
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
        public DataTable GetZoneName()
        {

            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);

            try
            {

                string StrSql = @"  SELECT zoneid ,zonecode,zonename
                                    FROM zone
                                    WHERE Activated=1 AND Deleted=0 
                                    Order By zonecode ASC ";


                DataTable dt = DBHelper.List(StrSql, ObjConn);

                return dt;
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
        public DataTable GetStaffRole(int staffid, int staffroleid)
        {

            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);

            try
            {

                string StrSql = @" SELECT s.*,sr.StaffRoleName FROM  Staff  s
            LEFT JOIN  staffrole sr ON s.StaffRoleID = sr.StaffRoleID
            WHERE 0=0 AND s.deleted=0 AND sr.Deleted=0 ";

                if (staffid > 0)
                    StrSql += " and s.StaffID=" + staffid;

                if (staffroleid > 0)
                    StrSql += " and s.StaffRoleID=" + staffroleid;
                DataTable dt = DBHelper.List(StrSql, ObjConn);

                return dt;
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

        public DataTable GetStaffRolePermissionSale(int staffid, int staffroleid)
        {

            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);

            try
            {

                string StrSql = @" SELECT s.*,sr.StaffRoleName,srp.StaffPermissionID FROM  Staff  s
            LEFT JOIN  staffrole sr ON s.StaffRoleID = sr.StaffRoleID 
            LEFT JOIN  staffrolepermission srp ON srp.staffRoleID=sr.staffRoleID AND srp.StaffPermissionID=22
            WHERE 0=0 AND s.deleted=0 AND sr.Deleted=0 ";

                if (staffid > 0)
                    StrSql += " and s.StaffID=" + staffid;

                if (staffroleid > 0)
                    StrSql += " and s.StaffRoleID=" + staffroleid;
                DataTable dt = DBHelper.List(StrSql, ObjConn);

                return dt;
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
        public DataTable GetStaff(int staffroleId, int zoneId)
        {

            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);

            try
            {

                string StrSql = @"  SELECT s.*,z.ZoneName FROM  staff_zone sz 
            LEFT JOIN  Staff s ON sz.StaffID = s.StaffID
            LEFT JOIN  zone z ON sz.ZoneID = z.ZoneID
            WHERE 0=0 ";

                if (staffroleId != 0)
                    StrSql += " AND s.StaffRoleID=" + staffroleId;
                if (zoneId != 0)
                    StrSql += " AND sz.ZoneID=" + zoneId;


                DataTable dt = DBHelper.List(StrSql, ObjConn);

                return dt;
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


        public void AddStaffRole(StaffRole role)
        {


            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);
            role.StaffRoleID = Utility.GetMaxID("staffrole", "StaffRoleID");
            try
            {
                string strSql = @"INSERT INTO staffrole
                                (StaffRoleID,
                                 StaffRoleName,
                                 Activated,
                                 Deleted)
                                 VALUES ({0},
                                        {1},
                                        1,
                                        0);";

                strSql = string.Format(strSql, role.StaffRoleID, Utility.ReplaceString(role.StaffRoleName));
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
        public void EditStaffRole(StaffRole role)
        {


            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);
            try
            {
                string strSql = @"Update staffrole set
                                 StaffRoleName={1}
                                where  StaffRoleID={0}";

                strSql = string.Format(strSql, role.StaffRoleID, Utility.ReplaceString(role.StaffRoleName));
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

        public void DeletedStaffRole(int staffroleId)
        {
            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);
            try
            {
                string strSql =@"Update staffrole set Deleted=1 
                                where  StaffRoleID={0}";

                strSql = string.Format(strSql, staffroleId);
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



        public void AddStaff(Staffs staff)
        {


            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);
            staff.StaffID = Utility.GetMaxID("staff", "StaffID");
            staff.StaffPassword = Utility.HashPassword(staff.StaffPassword);
            try
            {
                string strSql = @"INSERT INTO staff
            (StaffID,
             StaffRoleID,
             StaffCode,
             StaffPassword,
             StaffTitleName,
             StaffFirstName,
             StaffLastName,
             StaffAddress1,
             StaffAddress2,
             StaffSubDistrictId,
             StaffDistrictId,
             StaffProvinceId,
             StaffZipCode,
             StaffImagePath,
             StaffMobile,
             StaffTelephone,
             StaffEmail,
             InsertBy,
             InsertDate,
             Activated,
             Deleted)
             VALUES ({0},{1}, {2}, {3}, {4}, {5}, {6}, {7},{8},{9},{10}, {11}, {12}, {13}, {14}, {15},{16}, {17},{18},1, 0);";

                strSql = string.Format(strSql,
                     staff.StaffID,
                     staff.StaffRoleID,
                     Utility.ReplaceString(staff.StaffCode),
                     Utility.ReplaceString(staff.StaffPassword),
                     Utility.ReplaceString(staff.StaffTitleName),
                     Utility.ReplaceString(staff.StaffFirstName),
                     Utility.ReplaceString(staff.StaffLastName),
                     Utility.ReplaceString(staff.StaffAddress1),
                     Utility.ReplaceString(staff.StaffAddress2),
                     staff.StaffSubDistrictId,
                     staff.StaffDistrictId,
                     staff.StaffProvinceId,
                      Utility.ReplaceString(staff.StaffZipCode),
                      Utility.ReplaceString("Content/default-user.png"),
                      Utility.ReplaceString(staff.StaffMobile),
                      Utility.ReplaceString(staff.StaffTelephone),
                      Utility.ReplaceString(staff.StaffEmail),
                      staff.InsertBy,
                      Utility.FormateDateTime(DateTime.Now)
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
        public void EditStaff(Staffs staff)
        {

            List<Staffs> data = GetStaff(staff.StaffID);
            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);
            
          
            try
            {
                
                string strSql = @"Update staff Set
             StaffRoleID={1},
             StaffCode={2},
             StaffEmail={3},
             StaffTitleName={4},
             StaffFirstName={5},
             StaffLastName={6},
             StaffAddress1={7},
             StaffAddress2={8},
             StaffSubDistrictId={9},
             StaffDistrictId={10},
             StaffProvinceId={11},
             StaffZipCode={12},
             StaffMobile={13},
             StaffTelephone={14},
             UpdateBy={15},
             UpdateDate={16}
            Where  StaffID = {0} ";

                    strSql = string.Format(strSql,
                         staff.StaffID,
                         staff.StaffRoleID,
                         Utility.ReplaceString(staff.StaffCode),
                          Utility.ReplaceString(staff.StaffEmail),
                         Utility.ReplaceString(staff.StaffTitleName),
                         Utility.ReplaceString(staff.StaffFirstName),
                         Utility.ReplaceString(staff.StaffLastName),
                         Utility.ReplaceString(staff.StaffAddress1),
                         Utility.ReplaceString(staff.StaffAddress2),
                         staff.StaffSubDistrictId,
                         staff.StaffDistrictId,
                         staff.StaffProvinceId,
                          Utility.ReplaceString(staff.StaffZipCode),
                          Utility.ReplaceString(staff.StaffMobile),
                          Utility.ReplaceString(staff.StaffTelephone),
                          staff.UpdateBy,
                          Utility.FormateDateTime(DateTime.Now)
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
        public void EditStaffPassword(Staffs staff)
        {

            List<Staffs> data = GetStaff(staff.StaffID);
            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);


            try
            {

                string strSql = @"Update staff Set
             StaffPassword='{1}',
             UpdateBy={2},
             UpdateDate={3}
            Where  StaffID = {0} ";

                strSql = string.Format(strSql,
                     staff.StaffID,
                     Utility.HashPassword(staff.StaffPassword),
                      staff.UpdateBy,
                      Utility.FormateDateTime(DateTime.Now)
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
        public void DeletedStaff(int StaffID,int UpdateBy)
        {
            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);
            try
            {
                string strSql =@"Update staff  set Deleted=1,UpdateBy={1},UpdateDate={2}
                                where  StaffID={0}";

                strSql = string.Format(strSql, StaffID,UpdateBy,Utility.FormateDateTime(DateTime.Now));
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

        public void ActivatedStaffs(int StaffID, int UpdateBy)
        {
            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);
           
            try
            {
                DataTable dt = DBHelper.List("select Activated From staff Where StaffID=" + StaffID, ObjConn);
                string strSql =@"Update staff  set Activated={3},UpdateBy={1},UpdateDate={2}
                                where  StaffID={0}";

                if (Convert.ToInt32(dt.Rows[0]["Activated"].ToString()) == 1)
                {
                    strSql = string.Format(strSql, StaffID, UpdateBy, Utility.FormateDateTime(DateTime.Now), 0);

                    DBHelper.Execute(strSql, ObjConn);
                }
                else if (Convert.ToInt32(dt.Rows[0]["Activated"].ToString()) == 0)
                {

                    strSql = string.Format(strSql, StaffID, UpdateBy, Utility.FormateDateTime(DateTime.Now), 1);

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
        public List<StaffRole> GetListStaffRole( int id)
        {

            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);
            try
            {
                List<StaffRole> data = new List<StaffRole>();
                StaffRole sr = new StaffRole();
                string StrSql = @"  Select * FROM  staffrole where Activated=1 and deleted=0  ";
                    
                    if(id>0)
                        StrSql+=" and StaffRoleID="+id;

                DataTable dt = DBHelper.List(StrSql, ObjConn);

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        sr = new StaffRole();
                        sr.StaffRoleID = Convert.ToInt32(dt.Rows[i]["StaffRoleID"].ToString());
                        sr.StaffRoleName = dt.Rows[i]["StaffRoleName"].ToString();
                        sr.Activated = Convert.ToInt32(dt.Rows[i]["Activated"].ToString());
                        sr.Deleted = Convert.ToInt32(dt.Rows[i]["Deleted"].ToString());

                        data.Add(sr);
                    }
                }
                return data;
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

        public List<Staffs> GetStaff(int id)
        {

            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);
            try
            {
                List<Staffs> data = new List<Staffs>();
                string StrSql = @"  Select * FROM  staff where Activated=1 and deleted=0  ";

                StrSql += " and StaffID=" + id;

                DataTable dt = DBHelper.List(StrSql, ObjConn);

                Staffs sr = new Staffs();
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        sr = new Staffs();
                        if (dt.Rows[i]["StaffID"] != DBNull.Value)
                            sr.StaffID = Convert.ToInt32(dt.Rows[i]["StaffID"].ToString());
                        if (dt.Rows[i]["StaffRoleID"] != DBNull.Value)
                            sr.StaffRoleID = Convert.ToInt32(dt.Rows[i]["StaffRoleID"].ToString());
                        if (dt.Rows[i]["StaffCode"] != DBNull.Value)
                            sr.StaffCode = dt.Rows[i]["StaffCode"].ToString();
                        if (dt.Rows[i]["StaffPassword"] != DBNull.Value)
                            sr.StaffPassword = dt.Rows[i]["StaffPassword"].ToString();
                        if (dt.Rows[i]["StaffTitleName"] != DBNull.Value)
                            sr.StaffTitleName = dt.Rows[i]["StaffTitleName"].ToString();
                        if (dt.Rows[i]["StaffFirstName"] != DBNull.Value)
                            sr.StaffFirstName = dt.Rows[i]["StaffFirstName"].ToString();
                        if (dt.Rows[i]["StaffLastName"] != DBNull.Value)
                            sr.StaffLastName = dt.Rows[i]["StaffLastName"].ToString();
                        if (dt.Rows[i]["StaffAddress1"] != DBNull.Value)
                            sr.StaffAddress1 = dt.Rows[i]["StaffAddress1"].ToString();
                        if (dt.Rows[i]["StaffAddress2"] != DBNull.Value)
                            sr.StaffAddress2 = dt.Rows[i]["StaffAddress2"].ToString();
                        if (dt.Rows[i]["StaffSubDistrictId"] != DBNull.Value)
                            sr.StaffSubDistrictId = Convert.ToInt32(dt.Rows[i]["StaffSubDistrictId"].ToString());
                        if (dt.Rows[i]["StaffDistrictId"] != DBNull.Value)
                            sr.StaffDistrictId = Convert.ToInt32(dt.Rows[i]["StaffDistrictId"].ToString());
                        if (dt.Rows[i]["StaffProvinceId"] != DBNull.Value)
                            sr.StaffProvinceId = Convert.ToInt32(dt.Rows[i]["StaffProvinceId"].ToString());
                        if (dt.Rows[i]["StaffZipCode"] != DBNull.Value)
                            sr.StaffZipCode = dt.Rows[i]["StaffZipCode"].ToString();
                        if (dt.Rows[i]["StaffMobile"] != DBNull.Value)
                            sr.StaffMobile = dt.Rows[i]["StaffMobile"].ToString();
                        if (dt.Rows[i]["StaffTelephone"] != DBNull.Value)
                            sr.StaffTelephone = dt.Rows[i]["StaffTelephone"].ToString();
                        if (dt.Rows[i]["StaffEmail"] != DBNull.Value)
                            sr.StaffEmail = dt.Rows[i]["StaffEmail"].ToString();
                        if (dt.Rows[i]["Activated"] != DBNull.Value)
                            sr.Activated = Convert.ToInt32(dt.Rows[i]["Activated"].ToString());
                        if (dt.Rows[i]["Deleted"] != DBNull.Value)
                            sr.Deleted = Convert.ToInt32(dt.Rows[i]["Deleted"].ToString());

                        data.Add(sr);
                    }
                }
                return data;
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

        public List<StaffPermission> GetStaffPermission()
        {

            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);
            try
            {
                List<StaffPermission> data = new List<StaffPermission>();
                string StrSql = @"  SELECT p.StaffPermissionID,p.StaffPermissionName ,p.StaffPermissionIcon,p.StaffPermissionUrl ,pg.StaffPermissionGroupID,pg.StaffPermissionGroupName  ,p.IsMenu  FROM staffpermission  p 
      LEFT JOIN staffpermissiongroup  pg ON  p.StaffPermissionGroup=pg.StaffPermissionGroupID
      WHERE p.Activated=1 AND p.Deleted=0 
       Order by pg.Ordering,p.Ordering   asc  ";

                DataTable dt = DBHelper.List(StrSql, ObjConn);

                StaffPermission sr = new StaffPermission();
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        sr = new StaffPermission();
                        if (dt.Rows[i]["StaffPermissionID"] != DBNull.Value)
                            sr.StaffPermissionID = Convert.ToInt32(dt.Rows[i]["StaffPermissionID"].ToString());
                        if (dt.Rows[i]["StaffPermissionGroupID"] != DBNull.Value)
                            sr.StaffPermissionGroupID = Convert.ToInt32(dt.Rows[i]["StaffPermissionGroupID"].ToString());
                        if (dt.Rows[i]["StaffPermissionName"] != DBNull.Value)
                            sr.StaffPermissionName = dt.Rows[i]["StaffPermissionName"].ToString();
                        if (dt.Rows[i]["StaffPermissionIcon"] != DBNull.Value)
                            sr.StaffPermissionIcon = dt.Rows[i]["StaffPermissionIcon"].ToString();
                        if (dt.Rows[i]["StaffPermissionGroupName"] != DBNull.Value)
                            sr.StaffPermissionGroupName = dt.Rows[i]["StaffPermissionGroupName"].ToString();
                        if (dt.Rows[i]["StaffPermissionUrl"] != DBNull.Value)
                            sr.StaffPermissionUrl = dt.Rows[i]["StaffPermissionUrl"].ToString();
                        if (dt.Rows[i]["IsMenu"] != DBNull.Value)
                            sr.IsMenu = Convert.ToInt32(dt.Rows[i]["IsMenu"].ToString());

                        data.Add(sr);
                    }
                }
                return data;
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
        public List<StaffPermissionGroup> GetStaffMenu(int staffroleID)
        {

            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);
            try
            {
                List<StaffPermissionGroup> data = new List<StaffPermissionGroup>();
                string StrSql = @"   SELECT * FROM staffpermissiongroup pg   WHERE pg.Activated=1 AND pg.Deleted=0   Order by pg.Ordering  asc ";

                DataTable dt = DBHelper.List(StrSql, ObjConn);

                StaffPermissionGroup sr = new StaffPermissionGroup();

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        sr = new StaffPermissionGroup();
                      
                        if (dt.Rows[i]["StaffPermissionGroupID"] != DBNull.Value)
                            sr.StaffPermissionGroupID = Convert.ToInt32(dt.Rows[i]["StaffPermissionGroupID"].ToString());
                        if (dt.Rows[i]["StaffPermissionGroupName"] != DBNull.Value)
                            sr.StaffPermissionGroupName = dt.Rows[i]["StaffPermissionGroupName"].ToString();
                        if (dt.Rows[i]["StaffPermissionGroupIcon"] != DBNull.Value)
                            sr.StaffPermissionGroupIcon = dt.Rows[i]["StaffPermissionGroupIcon"].ToString();

                        sr.ListPermission = GetStaffPermissionMenu(ObjConn, staffroleID, sr.StaffPermissionGroupID, 1);

                        if (sr.ListPermission.Count > 0) {
                            data.Add(sr);
                        }
                       
                    }
                }
                return data;
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

        public List<StaffPermission> GetStaffPermissionMenu(MySqlConnection ObjConn, int staffroleID, int staffPermissionGroupID, int isMenu)
        {

            try
            {
                List<StaffPermission> data = new List<StaffPermission>();
                string StrSql = @" select sr.StaffPermissionID,p.StaffPermissionID,p.StaffPermissionName,p.StaffPermissionUrl ,p.StaffPermissionIcon
                    ,pg.StaffPermissionGroupID,pg.StaffPermissionGroupName  ,p.IsMenu  ,sr.StaffRoleID
                    from staffpermission  p 
                    left join staffpermissiongroup  pg on  p.StaffPermissionGroup=pg.StaffPermissionGroupID
                    left join staffrolepermission sr on sr.StaffPermissionID=p.StaffPermissionID
                    where p.Activated=1 and p.Deleted=0 
                    and sr.StaffRoleID=" + staffroleID  ;

                if (isMenu > 0)
                    StrSql += " and p.IsMenu=" + isMenu;


                if (staffPermissionGroupID>0)
                StrSql += " and pg.staffPermissionGroupID=" + staffPermissionGroupID;

                StrSql += " Order by pg.Ordering,p.Ordering   asc  ;";
          

                DataTable dt = DBHelper.List(StrSql, ObjConn);

                StaffPermission sr = new StaffPermission();
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        sr = new StaffPermission();
                        if (dt.Rows[i]["StaffPermissionID"] != DBNull.Value)
                            sr.StaffPermissionID = Convert.ToInt32(dt.Rows[i]["StaffPermissionID"].ToString());
                        if (dt.Rows[i]["StaffPermissionGroupID"] != DBNull.Value)
                            sr.StaffPermissionGroupID = Convert.ToInt32(dt.Rows[i]["StaffPermissionGroupID"].ToString());
                        if (dt.Rows[i]["StaffPermissionName"] != DBNull.Value)
                            sr.StaffPermissionName = dt.Rows[i]["StaffPermissionName"].ToString();
                        if (dt.Rows[i]["StaffPermissionGroupName"] != DBNull.Value)
                            sr.StaffPermissionGroupName = dt.Rows[i]["StaffPermissionGroupName"].ToString();
                        if (dt.Rows[i]["StaffPermissionIcon"] != DBNull.Value)
                            sr.StaffPermissionIcon = dt.Rows[i]["StaffPermissionIcon"].ToString();
                        if (dt.Rows[i]["StaffPermissionUrl"] != DBNull.Value)
                            sr.StaffPermissionUrl = dt.Rows[i]["StaffPermissionUrl"].ToString();
                        if (dt.Rows[i]["IsMenu"] != DBNull.Value)
                            sr.IsMenu = Convert.ToInt32(dt.Rows[i]["IsMenu"].ToString());

                        data.Add(sr);
                    }
                }
                return data;
            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
     //           ObjConn.Close();
            }


        }
        public List<int> GetListStaffPermissionID(int staffroleID) {


            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);
            List<int> permissionItem = new List<int>();
            try
            {

                List<StaffPermission> item = GetStaffPermissionMenu(ObjConn, staffroleID, 0, 0);

                StaffPermission sr = new StaffPermission();


                if (item.Count > 0)
                {
                    for (int i = 0; i < item.Count; i++)
                    {
                        sr = new StaffPermission();
                        if (item[i].StaffPermissionID !=0)
                            sr.StaffPermissionID = item[i].StaffPermissionID;

                            permissionItem.Add(sr.StaffPermissionID);
                    }
                }
                return permissionItem;
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
        public string GetMenu(List<StaffPermissionGroup> permission) {

            try
            {
                string menuStr = "";

                foreach (var pg in permission)
                {

                    menuStr += " <li> <a href='javascript:void(0)' class='waves-effect'><i data-icon='/' class='" + pg.StaffPermissionGroupIcon + "'></i><span class='hide-menu'> " + pg.StaffPermissionGroupName + "<span class='fa arrow'></span></a>";
                     menuStr += "<ul class='nav nav-second-level'> ";
                   
                     
                   
                    foreach (var p in pg.ListPermission)
                    {

                           menuStr += " <li><a href='" + p.StaffPermissionUrl + "'><i class='"+p.StaffPermissionIcon+"'></i><span class='hide-menu'> " + p.StaffPermissionName + "</span></a></li> ";
                 
                    }
                     menuStr += " </ul></li>";
                }


                return menuStr;
            }
            catch (Exception ex )
            {
                
                throw;
            }
        
        
        }


        public void AddPermission(List<int> ItemSelect, int staffRoleID)
        {


            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);
            try
            {
                string strSql = @"delete From staffrolepermission where StaffRoleID="+staffRoleID+";";
                strSql += @"INSERT INTO staffrolepermission
            (StaffRoleID,  StaffPermissionID) VALUES ";
                    
                  for (int i = 0; i < ItemSelect.Count; i++)
			    {
                       if(i!=0)
                          strSql +=",";

                      strSql += "(" + staffRoleID + "," + ItemSelect [i]+ ")";
			    }
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


        public List<MarkersData> GetStaffLocation(string dateTime, int staffId)
        {
            DateTime dateAsOf = DateTime.ParseExact(dateTime, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);
            string StrSql = "";
            try
            {

                StrSql = @" SELECT c.CustomerFirstName ,SUM(d.PriceReceipts) AS PriceReceipts  ,TIME(DateAsOf) AS TimePay ,d.Latitude,d.Longitude
                 FROM daily_receipts d
                 LEFT JOIN customer c ON d.CustomerID=c.CustomerId
                  WHERE staffid=" +staffId+
                @" AND DATE(DateAsOf) ="+Utility.FormateDate(dateAsOf)+
                @"    GROUP BY c.CustomerFirstName 
                ORDER BY TIME(DateAsOf) ASC ";



                DataTable dt = DBHelper.List(StrSql, ObjConn);

                List<MarkersData> StaffLocation = new List<MarkersData>(); ;
                MarkersData obj = new MarkersData();
                if (dt.Rows.Count > 0)
                {

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        obj = new MarkersData();
                        obj.location = new List<double>();
                        obj.location.Add(dt.Rows[i]["Latitude"] != DBNull.Value ? Convert.ToDouble(dt.Rows[i]["Latitude"].ToString()) : 13.4257831);
                        obj.location.Add(dt.Rows[i]["Longitude"] != DBNull.Value ? Convert.ToDouble(dt.Rows[i]["Longitude"].ToString()) : 99.9553108);

                        obj.tooltip = new Tooltip();
                        if (dt.Rows[i]["PriceReceipts"] != DBNull.Value & dt.Rows[i]["CustomerFirstName"] != DBNull.Value)
                        obj.tooltip.text = "คุณ" + dt.Rows[i]["CustomerFirstName"].ToString() + " ชำระ " + Convert.ToDecimal(dt.Rows[i]["PriceReceipts"].ToString()) + " บาท เวลา " + dt.Rows[i]["TimePay"].ToString() + " น.";
                    
                        StaffLocation.Add(obj);
                    }

                }

                return StaffLocation;

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


        public List<StaffSettingOTP> GetStaffSettingOTP()
        {

            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);
            string StrSql = "";
            try
            {

                StrSql = @" SELECT s.StaffID ,s.StaffRoleID,s.StaffCode
                            ,CONCAT(s.StaffTitleName ,s.StaffFirstName,' ',s.StaffLastName) AS StaffName , sr.StaffRoleName , IFNULL(so.Password,'-') AS OTP
                              FROM staff s 
                            LEFT JOIN staff_otp so ON s.StaffID=so.StaffID   AND so.LoginDate='{0}' and so.Deleted=0
                            LEFT JOIN staffrole sr ON s.StaffRoleID=sr.StaffRoleID
                            where s.StaffRoleID=5
                            ORDER BY s.StaffID ASC  ";




                StrSql = string.Format(StrSql, DateTime.Now.ToString("yyyy-MM-dd"));
                DataTable dt = DBHelper.List(StrSql, ObjConn);

                List<StaffSettingOTP> staffs = new List<StaffSettingOTP>(); ;
                StaffSettingOTP obj = new StaffSettingOTP();
                if (dt.Rows.Count > 0)
                {

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        obj = new StaffSettingOTP();

                        obj.StaffID = Convert.ToInt32(dt.Rows[i]["StaffID"].ToString());
                        obj.StaffRoleID = Convert.ToInt32(dt.Rows[i]["StaffRoleID"].ToString());
                        obj.StaffCode = dt.Rows[i]["StaffCode"].ToString();
                        obj.StaffName = dt.Rows[i]["StaffName"].ToString();
                        obj.StaffRoleName = dt.Rows[i]["StaffRoleName"].ToString();
                        obj.OTP = dt.Rows[i]["OTP"].ToString();
                      
                        staffs.Add(obj);
                    }

                }

                return staffs;

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
        public List<int> GetStaffOTPSelect()
        {

            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);
            string StrSql = "";
            try
            {

                StrSql = @" SELECT so.StaffID 
                            From staff_otp so where   so.LoginDate='{0}' and so.Deleted=0 
                            ORDER BY so.StaffID ASC  ";




                StrSql = string.Format(StrSql, DateTime.Now.ToString("yyyy-MM-dd"));
                DataTable dt = DBHelper.List(StrSql, ObjConn);
                List<int> selected= new List<int>();
                if (dt.Rows.Count > 0)
                {

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        selected.Add( Convert.ToInt32(dt.Rows[i]["StaffID"].ToString()));
                        
                    }

                }

                return selected;

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



        public void AddPermission(List<int> ItemSelect)
        {
            if (ItemSelect == null) {
                throw new Exception("โปรดเลือกพนักงาน !!");
            }
            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);
            try
            {
                string sqlInsert = "";
                string strSql = @"update  db_greatgroup_uat.staff_otp  set Deleted=1 where LoginDate='{1}' and StaffID={0};
                                INSERT INTO db_greatgroup_uat.staff_otp
                                (StaffID,
                                 LoginDate,
                                 PASSWORD,
                                 Deleted)
                                VALUES ({0},
                                '{1}',
                                '{2}',
                                {3}); ";


                Random rnd;
                for (int i = 0; i < ItemSelect.Count; i++)
                {
                    rnd = new Random();

                    sqlInsert = string.Format(strSql, ItemSelect[i], DateTime.Now.ToString("yyyy-MM-dd"), rnd.Next(100001, 999999),0);


                    DBHelper.Execute(sqlInsert, ObjConn);
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

        public void DeletePassword(List<int> ItemSelect)
        {
            if (ItemSelect == null)
            {
                throw new Exception("โปรดเลือกพนักงาน !!");
            }
            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);
            try
            {
                string sqlInsert = "";
                string strSql = @"  update  db_greatgroup_uat.staff_otp  
                                    set Deleted=1 
                                    where LoginDate='{1}' and StaffID={0}; ";


                Random rnd;
                for (int i = 0; i < ItemSelect.Count; i++)
                {
                    rnd = new Random();

                    sqlInsert = string.Format(strSql, ItemSelect[i], DateTime.Now.ToString("yyyy-MM-dd"));


                    DBHelper.Execute(sqlInsert, ObjConn);
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


    }
}