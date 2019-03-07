using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheGreatGroupModules.Modules;

namespace TheGreatGroupModules.Controllers
{
    public class ReportController : Controller
    {
        //
        // GET: /Report/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult PDFContractDocument()
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
        public ActionResult PDFReceipt()
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
        
        public ActionResult DiscountReport()
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
        public ActionResult SaleReport(int StaffID)
        {
            if (Session["iuser"] != null)
            {
                return View();
            }
            else {

                return RedirectToAction("error404", "Home");

            }


            }
            public ActionResult CostProfix()
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
        public ActionResult OpenCloseAccountByCustomerReport()
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
        public ActionResult OpenCloseAccountReport()
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
        public ActionResult CloseAccountReport()
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

        public ActionResult NPLReport()
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


        // POST: /Report/GetOpenAccountReport?zoneId=1&datefrom=2018-04-01&dateto=2018-04-30
        [HttpPost]
        public JsonResult GetOpenAccountReport(SearchCriteria search)
        {

            //DateTime datetime
            try
            {

            
                List<OpenAccountReport> listData = new List<OpenAccountReport>();
                ReportData data = new ReportData();
                listData = data.OpenAccountReports(search);



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

        // POST: /Report/GetNPLReport
        [HttpPost]
        public JsonResult GetNPLReport(NPLReport search)
        {

            //DateTime datetime
            try
            {


                IList<NPLReport> listData = new List<NPLReport>();
                ReportData data = new ReportData();
                listData = data.GetNPLReport(search);



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
        // Post: /Report/GetDiscountReport
        [HttpPost]
        public JsonResult GetDiscountReport(SearchCriteria search)
        {


            try
            {
                ReportData rt = new ReportData();
                IList<DailyReceiptsReport> data = new List<DailyReceiptsReport>();
                data = rt.GetDiscountReport(search);

                return Json(new
                {
                    data = data,
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


        // Report/GetPaymentReportByCustomer?CustomerID=1&ContractID=1
        [HttpPost]
        public JsonResult GetPaymentReportByCustomer(SearchCriteria item) 
        {


            try
            {
                ContractData rt = new ContractData();
                IList<ReportCustomer> data = new List<ReportCustomer>();
                data = rt.GetPaymentReportByCustomer(item);

                return Json(new
                {
                    data = data,
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
