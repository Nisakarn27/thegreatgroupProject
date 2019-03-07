using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheGreatGroupModules.Models;
using TheGreatGroupModules.Modules;

namespace TheGreatGroupModules.Controllers
{
    public class StaffsController : Controller
    {
        //
        // GET: /Staffs/

        public ActionResult Index()
        {
            if (Session["iuser"] != null)
            {
                return View();
            }
            else
            {
                TempData["error"] = "Session หมดอายุ , กรูณาเข้าสู่ระบบใหม่อีกครั้ง";
                return RedirectToAction("Login", "Home");
            }
          
        }

        public ActionResult ListStaffPassword()
        {
            if (Session["iuser"] != null)
            {
                return View();
            }
            else
            {
                TempData["error"] = "Session หมดอายุ , กรูณาเข้าสู่ระบบใหม่อีกครั้ง";
                return RedirectToAction("Login", "Home");
            }

        }
        

        public ActionResult ListStaff()
        {
            if (Session["iuser"] != null)
            {
                return View();
            }
            else
            {
                TempData["error"] = "Session หมดอายุ , กรูณาเข้าสู่ระบบใหม่อีกครั้ง";
                return RedirectToAction("Login", "Home");
            }
          
        }
        public ActionResult AddStaff(int staffID)
        {
            if (Session["iuser"] != null)
            {
                return View();
            }
            else
            {
                TempData["error"] = "Session หมดอายุ , กรูณาเข้าสู่ระบบใหม่อีกครั้ง";
                return RedirectToAction("Login", "Home");
            }
          
        }

        public ActionResult EditStaff(int staffID)
        {
            if (Session["iuser"] != null)
            {
                return View();
            }
            else
            {
                TempData["error"] = "Session หมดอายุ , กรูณาเข้าสู่ระบบใหม่อีกครั้ง";
                return RedirectToAction("Login", "Home");
            }

        }

        public ActionResult ListStaffRole()
        {
            if (Session["iuser"] != null)
            {
                return View();
            }
            else
            {
                TempData["error"] = "Session หมดอายุ , กรูณาเข้าสู่ระบบใหม่อีกครั้ง";
                return RedirectToAction("Login", "Home");
            }
          
        }
        public ActionResult ListStaffBranch()
        {
            if (Session["iuser"] != null)
            {
                return View();
            }
            else
            {
                TempData["error"] = "Session หมดอายุ , กรูณาเข้าสู่ระบบใหม่อีกครั้ง";
                return RedirectToAction("Login", "Home");
            }
          
        }

        public ActionResult SettingPermission(int staffID)
        {
            if (Session["iuser"] != null)
            {
                return View();
            }
            else
            {
                TempData["error"] = "Session หมดอายุ , กรูณาเข้าสู่ระบบใหม่อีกครั้ง";
                return RedirectToAction("Login", "Home");
            }
          
        }
        public ActionResult StaffLocation()
        {
            if (Session["iuser"] != null)
            {
                return View();
            }
            else
            {
                TempData["error"] = "Session หมดอายุ , กรูณาเข้าสู่ระบบใหม่อีกครั้ง";
                return RedirectToAction("Login", "Home");
            }
          
        }
        

        // GET: /Staffs/GetZone
        public JsonResult GetZone()
        {
            // รับค่าราคา
            StaffData st = new StaffData();
            DataTable dt = new DataTable();
            List<ListItems> item = new List<ListItems>();
               dt= st.GetZone();
               if (dt.Rows.Count > 0) { 
                        item = dt.AsEnumerable().Select(dr => new ListItems()
                        {
                            ID = dr.Field<int>("zoneid"),
                            Code =dr.Field<string>("zonecode"),
                            Value = dr.Field<string>("zonename"),

                        }).ToList();

               }
            return Json(new
            {
                data = item,
                success = true
            }, JsonRequestBehavior.AllowGet);
        }
       
        public JsonResult GetStaffs(int staffroleId,int zoneId)
        {
            // รับค่าราคา
            StaffData st = new StaffData();
            DataTable dt = new DataTable();
            List<ListItems> item = new List<ListItems>();
            dt = st.GetStaff(staffroleId, zoneId);
            if (dt.Rows.Count > 0)
            {
                item = dt.AsEnumerable().Select(dr => new ListItems()
                {
                    ID = dr.Field<int>("StaffID"),
                    Value = dr.Field<string>("StaffTitleName") + dr.Field<string>("StaffFirstName") +" "
                    + dr.Field<string>("StaffLastName"),

                }).ToList();

            }
            return Json(new
            {
                data = item,
                success = true
            }, JsonRequestBehavior.AllowGet);
        }

        // GET: /Staffs/GetStaffData?staffID=0&staffroleId=0
        public JsonResult GetStaffData(int staffID,int staffroleId)
        {
            // รับค่าราคา
            StaffData st = new StaffData();
            DataTable dt = new DataTable();
            List<Staffs> staffList=new List<Staffs>();

            try
            {
                dt = st.GetStaffRolePermissionSale(staffID, staffroleId);
                if (dt.Rows.Count > 0)
                {
                    staffList = dt.AsEnumerable().Select(dr => new Staffs()
                    {
                        StaffID = dr.Field<int>("StaffID"),
                        StaffCode = dr.Field<string>("StaffCode"),
                        StaffTitleName = dr.Field<string>("StaffTitleName"),
                        StaffFirstName = dr.Field<string>("StaffFirstName"),
                        StaffLastName = dr.Field<string>("StaffLastName"),
                        StaffRoleID = dr.Field<int>("StaffRoleID"),
                        StaffRoleName = dr.Field<string>("StaffRoleName"),
                        StaffName = dr.Field<string>("StaffTitleName") + dr.Field<string>("StaffFirstName") + " "
                        + dr.Field<string>("StaffLastName"),
                        Activated = dr.Field<int>("Activated"),
                        Deleted = dr.Field<int>("Deleted"),
                    }).ToList();

                }


                return Json(new
                {
                    data = staffList,
                    success = true
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new
                {
                    data = ex.Message,
                    success = false
                }, JsonRequestBehavior.AllowGet);

            }
        }


        #region ::  Manage Staff ::

        public JsonResult GetListStaffs(int staffID)
        {

            try
            {
                StaffData st = new StaffData();
                List<Staffs> item = new List<Staffs>();
                item = st.GetStaff(staffID);


                List<StaffRole> item2 = new List<StaffRole>();
                item2 = st.GetListStaffRole(0);


                return Json(new
                {
                    data = item,
                    dataStaffRole=item2,
                    success = true
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new
                {
                    data = ex.Message,
                    success = false
                }, JsonRequestBehavior.AllowGet);
            }
          

        }

        [HttpPost]
        public JsonResult AddStaffs(Staffs staff)
        {


            StaffData data = new StaffData();

            try
            {
                if (Session["iuser"] == null)
                    throw new Exception(" Session หมดอายุ , กรุณาเข้าสู่ระบบใหม่อีกครั้ง !! ");

                staff.InsertBy = (Int32)Session["iuser"];

                data.AddStaff(staff);

                return Json(new
                {
                    data = "บันทึกข้อมูลสำเร็จ",
                    success = true
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new
                {
                    data = ex.Message,
                    success = false
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult EditStaffs(Staffs staff)
        {


            StaffData data = new StaffData();

            try
            {
                if (Session["iuser"] == null)
                    throw new Exception(" Session หมดอายุ , กรุณาเข้าสู่ระบบใหม่อีกครั้ง !! ");

                staff.UpdateBy = (Int32)Session["iuser"];

                data.EditStaff(staff);

                return Json(new
                {
                    data = "บันทึกข้อมูลสำเร็จ",
                    success = true
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new
                {
                    data = ex.Message,
                    success = false
                }, JsonRequestBehavior.AllowGet);
            }
        }


        
            
        [HttpPost]
        public JsonResult EditStaffPassword(Staffs staff)
        {


            StaffData data = new StaffData();

            try
            {
                if (Session["iuser"] == null)
                    throw new Exception(" Session หมดอายุ , กรุณาเข้าสู่ระบบใหม่อีกครั้ง !! ");

                staff.UpdateBy = (Int32)Session["iuser"];

                data.EditStaffPassword(staff);

                return Json(new
                {
                    data = "บันทึกข้อมูลสำเร็จ",
                    success = true
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new
                {
                    data = ex.Message,
                    success = false
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult DeletedStaffs(int StaffID)
        {


            StaffData data = new StaffData();

            try
            {
                if (Session["iuser"] == null)
                    throw new Exception(" Session หมดอายุ , กรุณาเข้าสู่ระบบใหม่อีกครั้ง !! ");

                int UpdateBy = (Int32)Session["iuser"];

                data.DeletedStaff(StaffID, UpdateBy);

                return Json(new
                {
                    data = "บันทึกข้อมูลสำเร็จ",
                    success = true
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new
                {
                    data = ex.Message,
                    success = false
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult ActivatedStaffs(int StaffID)
        {


            StaffData data = new StaffData();

            try
            {
                if (Session["iuser"] == null)
                    throw new Exception(" Session หมดอายุ , กรุณาเข้าสู่ระบบใหม่อีกครั้ง !! ");

                int UpdateBy = (Int32)Session["iuser"];

                data.ActivatedStaffs(StaffID, UpdateBy);

                return Json(new
                {
                    data = "บันทึกข้อมูลสำเร็จ",
                    success = true
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new
                {
                    data = ex.Message,
                    success = false
                }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion  ::  Manage Staff ::

        #region  :: Manage StaffRole  ::
        // POST:  /Staffs/AddStaffRole
         //  {StaffRoleName:""}
        [HttpPost]
        public JsonResult AddStaffRole(StaffRole role) {


            StaffData data = new StaffData();

            try
            {

                data.AddStaffRole(role);

                  return Json(new
                {
                    data = "บันทึกข้อมูลสำเร็จ",
                    success = true
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex) 
            {
                
                return Json(new
                {
                    data = ex.Message,
                    success = false
                }, JsonRequestBehavior.AllowGet);
            }
        }

        // POST:  /Staffs/EditStaffRole
        //  {StaffRoleID:"",
        // StaffRoleName:""}
        [HttpPost]
        public JsonResult EditStaffRole(StaffRole role)
        {


            StaffData data = new StaffData();

            try
            {

                data.EditStaffRole(role);

                return Json(new
                {
                    data = "บันทึกข้อมูลสำเร็จ",
                    success = true
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new
                {
                    data = ex.Message,
                    success = false
                }, JsonRequestBehavior.AllowGet);
            }
        }


        // Get:  /Staffs/DeleteStaffRole?staffroleId
       
        public JsonResult DeleteStaffRole(int staffroleId)
        {


            StaffData data = new StaffData();

            try
            {

                data.DeletedStaffRole(staffroleId);

                return Json(new
                {
                    data = "บันทึกข้อมูลสำเร็จ",
                    success = true
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new
                {
                    data = ex.Message,
                    success = false
                }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: /Staffs/GetListStaffRole?staffroleID=0
        public JsonResult GetListStaffRole(int staffroleID)
        {

            try
            {

                StaffData st = new StaffData();
                List<StaffRole> item = new List<StaffRole>();
                item = st.GetListStaffRole(staffroleID);

                return Json(new
                {
                    data = item,
                    success = true
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    data = ex.Message,
                    success = false
                }, JsonRequestBehavior.AllowGet);
            }

        }
        #endregion  :: Manage StaffRole  ::

        //api : ../staffs/GetLocationStaff?staffId=1?dateTime=2018-04-08
        public JsonResult GetLocationStaff(string dateTime, int staffId)
        {

            try
            {

                StaffData st = new StaffData();
                List<MarkersData> item = new List<MarkersData>();
                item = st.GetStaffLocation(dateTime, staffId);

                return Json(new
                {
                    data = item,
                    success = true
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    data = ex.Message,
                    success = false
                }, JsonRequestBehavior.AllowGet);
            }

        }

        
      

        //api : ../staffs/GetMenu?staffroleID=1
           public JsonResult GetMenu(int staffroleID)
        {

            try
            {
                StaffData st = new StaffData();
                List<StaffPermissionGroup>  item = new  List<StaffPermissionGroup> ();
                item = st.GetStaffMenu(staffroleID);

                return Json(new
                {
                    data = item,
                    success = true
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    data = ex.Message,
                    success = false
                }, JsonRequestBehavior.AllowGet);
            }

        }
      
    

        //api : ../staffs/GetStaffPermission?staffroleID=1
        public JsonResult GetStaffPermission(int staffroleID)
        {

            try
            {
               
                StaffData st = new StaffData();
                 List<StaffPermission> item = new  List<StaffPermission>();
                 List<StaffRole> item1 = new List<StaffRole>();

                
                item = st.GetStaffPermission();
                item1 =  st.GetListStaffRole(staffroleID);

                List<int> item2 = st.GetListStaffPermissionID(staffroleID);

                return Json(new
                {
                    data = item,
                    dataStaffRole = item1,
                    dataSelect=item2, 
                    success = true
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    data = ex.Message,
                    success = false
                }, JsonRequestBehavior.AllowGet);
            }

        }

        //  ../staffs/GetStaffSettingOTP
        [HttpGet]
        public JsonResult GetStaffSettingOTP()
        {

            try
            {
                List<StaffSettingOTP> st = new List<StaffSettingOTP>();
                StaffData data = new StaffData();
                st= data.GetStaffSettingOTP();
                List<int> select = new List<int>();
                select = data.GetStaffOTPSelect();
                return Json(new
                {
                    data = st,
                    dataSelect= select,
                    success = true
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    data = ex.Message,
                    success = false
                }, JsonRequestBehavior.AllowGet);
            }

        }



        //  ../staffs/AddPermission
        [HttpPost]
        public JsonResult AddPermission(List<int> ItemSelect)
        {

            try
            {

                StaffData st = new StaffData();

                st.AddPermission(ItemSelect);

                return Json(new
                {
                    data = "",
                    success = true
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    data = ex.Message,
                    success = false
                }, JsonRequestBehavior.AllowGet);
            }

        }
        //  ../staffs/DeletePassword
        [HttpPost]
        public JsonResult DeletePassword(List<int> ItemSelect)
        {

            try
            {

                StaffData st = new StaffData();

                st.DeletePassword(ItemSelect);

                return Json(new
                {
                    data = "",
                    success = true
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    data = ex.Message,
                    success = false
                }, JsonRequestBehavior.AllowGet);
            }

        }
        //  ../staffs/GetAddStaffPermission
        [HttpPost]
        public JsonResult GetAddStaffPermission(List<int> ItemSelect,int staffRoleID)
        {

            try
            {

                StaffData st = new StaffData();

                st.AddPermission(ItemSelect, staffRoleID);

                return Json(new
                {
                    data="",
                    success = true
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    data = ex.Message,
                    success = false
                }, JsonRequestBehavior.AllowGet);
            }

        }


    //    ./staffs/DiffLastTransaction

        public JsonResult  DiffLastTransaction()
        {
            DateTime dateTransaction= new DateTime(2018,05,02).Date;
            int ContractPayEveryDay=1;
            bool ContractSpecialholiday=true; 
            DateTime[] HolidaysArr = Utility.Holidays(1);
            DateTime dateNow = DateTime.Now.Date;
            double result = 0;
            result = (dateNow - dateTransaction).TotalDays;
            int diffdate = Convert.ToInt32(Math.Floor(result))-1;
            int totaldate = 0;

            DateTime startDate = dateTransaction;
            DateTime EndDate = dateNow;
            totaldate = diffdate;
            // เว้นวันหยุด
            if (ContractSpecialholiday)
            {

                if (ContractPayEveryDay == 1) // ทุกวัน ยกเว้นวันหยุดนขตฤกษ์
                {
                    while (diffdate > 0)
                    {

                        EndDate = EndDate.AddDays(-1);
                        
                        if (Utility.IsHolidays(EndDate, HolidaysArr))
                            totaldate = totaldate - 1;

                        diffdate--;
                    }

                }
                else if (ContractPayEveryDay == 2)
                {// จัน-ศุก ยกเว้นวันหยุดนขตฤกษ์
                    while (diffdate > 0)
                    {
                       EndDate = EndDate.AddDays(-1);
                        if (EndDate.DayOfWeek == DayOfWeek.Saturday || EndDate.DayOfWeek == DayOfWeek.Sunday || Utility.IsHolidays(EndDate, HolidaysArr))
                            totaldate = totaldate - 1;

                       
                        diffdate--;
                    }
                }
            }
            else  // จ-ศ =1
            {
                if (ContractPayEveryDay == 1) // ทุกวัน ไม่เว้นวันหยุดนขตฤกษ์
                {
                    totaldate = diffdate;
                }
                else if (ContractPayEveryDay == 2) // จัน-ศุก ไม่เว้นวันหยุดนขตฤกษ์
                {


                    while (diffdate > 0) 
                    {
                        EndDate = EndDate.AddDays(-1);
                       
                        if (EndDate.DayOfWeek == DayOfWeek.Saturday || EndDate.DayOfWeek == DayOfWeek.Sunday)
                            totaldate = totaldate - 1;
                        
                    
                        diffdate--;
                    }

                }
            }

            if (dateTransaction != DateTime.MinValue)
            {
                if (dateNow > dateTransaction)
                {
                    if (totaldate >= 1) 
                    {
                        
                    
                    }
                    else // วันปัจจุบันยังไม่จ่าย
                    {
                        totaldate= - 1;
                    }
                }
                else // จ่ายแล้ว
                {
                    totaldate = 0;
                }


            }
            else
            {
                totaldate =-1;
             

            }



            return Json(new
            {
                data = totaldate,
                success = true
            }, JsonRequestBehavior.AllowGet);

        }
    }
}
