using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheGreatGroupModules.Models;
using TheGreatGroupModules.Modules;

namespace TheGreatGroupModules.Controllers
{
    public class ManagePaymentController : Controller
    {
        //
        // GET: /ManagePayment/

        public ActionResult Installment()
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

        public ActionResult CustomerHistory()
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

        public JsonResult PaymentCallData(string CustomerID)
        {
            // รับค่าราคา


            return Json(new
            {
                data = "",
                success = true
            }, JsonRequestBehavior.AllowGet);
        }


        // GET: /ManagePayment/DailyReceiptsReport?staffId=1&dateAsOf=2018-04-08
        public JsonResult GetDailyReceiptsReport(int staffId, string dateAsOf)
        {

            //DateTime datetime
            try
            {
                List<DailyReceiptsReport> listData = new List<DailyReceiptsReport>();
                ReportData data = new ReportData();
                listData = data.GetDailyReceiptsReport(staffId, dateAsOf);



                return Json(new
                {
                    data = listData,
                    SumData = (listData.Sum(c => c.PriceReceipts)).ToString("#,##0.00"),
                    SumDataContractAmount = (listData.Sum(c => c.ContractAmount)).ToString("#,##0.00"),
                    SumDataBalance = (listData.Sum(c => c.Balance)).ToString("#,##0.00"),
                    countData= listData.Count,
                    success = true
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    errMsg = ex.Message
                }, JsonRequestBehavior.AllowGet);

            }

        }

        public JsonResult SaveActivateDailyReceipts(int staffId, string dateAsOf)
        {
            try
            {
               
                ReportData data = new ReportData();
                 data.SaveActivateDailyReceipts(staffId, dateAsOf);



                return Json(new
                {
                    success = true
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    errMsg = ex.Message
                }, JsonRequestBehavior.AllowGet);

            }
        
        }
        // GET: /ManagePayment/GetCustomerDetailOnCard?staffId=1?CustomerID=1?ContractID=1
        public JsonResult GetCustomerDetailOnCard(string staffId, string CustomerID, string ContractID)
        {

            try
            {
                IList<DailyReceiptsReport> listData = new List<DailyReceiptsReport>();
                ReportData data = new ReportData();
                listData = data.GetCustomerDetailOnCard(staffId, CustomerID, ContractID);

                IList<LastTransaction> listData1 = new List<LastTransaction>();
                listData1 = data.GetTransaction(staffId, CustomerID, ContractID);
                listData1 = listData1.OrderByDescending(c => c.DateAsOf).ToArray();
                return Json(new
                {
                    data = listData,
                   latest_transaction = listData1,
                    success = true
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    errMsg = ex.Message
                }, JsonRequestBehavior.AllowGet);

            }
        }

        [HttpPost] // Post: /ManagePayment/PostPaymentDailyReceipts
        public JsonResult PostPaymentDailyReceipts(DailyReceiptsReport item)
        {
            try
            {
                List<DailyReceiptsReport> listData = new List<DailyReceiptsReport>();
                CustomersData data = new CustomersData();
                data.PaymentDailyReceipts(item);


                return Json(new
                {
                    data = "บันทึกรายการสำเร็จ",
                    success = true
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    data = ex.Message
                }, JsonRequestBehavior.AllowGet);

            }
             
             }


        // Post: /ManagePayment/PostAdd_DailyRemark
         [HttpPost]
        public JsonResult PostAdd_DailyRemark(DailyRemark item)
        {
            try
            {
                ContractData data = new ContractData();
                data.Add_DailyRemark(item);


                return Json(new
                {
                    data = "บันทึกรายการสำเร็จ",
                    success = true
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    data = ex.Message
                }, JsonRequestBehavior.AllowGet);

            }
           }

         // GET: /ManagePayment/GetListDailyRemark?CustomerID=1&ContractID=1

         public JsonResult GetListDailyRemark(int CustomerID, int ContractID)
         {


             try
             {
                 ContractData data = new ContractData();
                 List<DailyRemark> items = new List<DailyRemark>(); 
                 
                 
              items=   data.GetListDailyRemark(CustomerID, ContractID);


                 return Json(new
                 {
                     data = items,
                     success = true
                 }, JsonRequestBehavior.AllowGet);
             }
             catch (Exception ex)
             {
                 return Json(new
                 {
                     success = false,
                     data = ex.Message
                 }, JsonRequestBehavior.AllowGet);

             }
         
         }


         // Post: /ManagePayment/PostStaffLoginOnMobile
         [HttpPost]
         public JsonResult PostStaffLoginOnMobile(StaffLogin item)
         {
             try
             {
                 string hostname = "http://203.154.41.217/";
                
                 if (!String.IsNullOrEmpty(item.StaffCode) || !String.IsNullOrEmpty(item.StaffPassword))
                 {

                     CustomersData data = new CustomersData();
                    item=  data.GetStaffLoginOnMobile(item);

                 }else{
                    if (String.IsNullOrEmpty(item.StaffCode) || String.IsNullOrEmpty(item.StaffPassword))
                    {

                        throw new Exception("กรุณากรอกรหัสพนักงานและรหัสผ่าน");
                    }
                    else if (String.IsNullOrEmpty(item.StaffCode))
                    {
                        throw new Exception("กรุณากรอกรหัสพนักงาน");
                    }
                    else if (String.IsNullOrEmpty(item.StaffPassword))
                    {

                        throw new Exception("กรุณากรอกรหัสผ่าน");
                    }
                    else
                    {
                        throw new Exception("ไม่มีข้อมูลพนักงาน");
                    }

                }
                 return Json(new
                 {   success = true,
                     iuser = item.StaffID,
                     iusername= item.StaffName,
                     istaffrole= item.StaffRoleID,
                     imageUrl = hostname +item.ImageUrl
                 }, JsonRequestBehavior.AllowGet);
             }
             catch (Exception ex)
             {
                 return Json(new
                 {
                     success = false,
                     errorMessage = ex.Message
                 }, JsonRequestBehavior.AllowGet);

             }
         }

        // GET: /ManagePayment/GetListCustomerOnMobile?staffId=1
        public JsonResult GetListCustomerOnMobile(int staffId)
        {

            try
            {
                CustomersData cus =new CustomersData();
                IList<ListCustomerOnMobile> listData = new List<ListCustomerOnMobile>();
                listData=  cus.GetListCustomerOnMobile( staffId);
         

                return Json(new
                {
                    data = listData,
                    success = true
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    errMsg = ex.Message
                }, JsonRequestBehavior.AllowGet);

            }
        }

    }
}
