using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheGreatGroupModules.Models;
using CrystalDecisions.Shared;
using TheGreatGroupModules.Modules;
using System.Web.Routing;
using System.Drawing;
using System.Net;
using Photoshop;
using MessagingToolkit.QRCode.Codec;
using MessagingToolkit.QRCode.Codec.Data;
namespace TheGreatGroupModules.Controllers
{
    public class CustomersController : Controller
    {
        //
        // GET: /Customers/

        public ActionResult Index()
        {
            if (Session["iuser"] != null)
            {
                return View();
            }
            else
            {
                TempData["error"] = "Session หมดอายุ , กรูณาเข้าสู่ระบบใหม่อีกครั้ง";
                return   RedirectToAction("Login", "Home");
            }
          
          
        }
        public ActionResult SearchListContract()
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
        public ActionResult PurchaseOrder(int CustomerID)
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
        public ActionResult ListContract(int CustomerID)
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

        public ActionResult Contract(int CustomerID, int ContractID)
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


        public ActionResult ContractSurety(int CustomerID, int ContractID)
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
        public ActionResult ApproveCloseAccount()
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
        public ActionResult EditCustomer(int CustomerID)
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
        public ActionResult AddCustomer()
        {
            if (Session["iuser"] != null)
            {
                return View();
            }
            else
            {
                TempData["error"] = "Session หมดอายุ , กรูณาเข้าสู่ระบบใหม่อีกครั้ง";
                return RedirectToAction("Login", "Home", new { area = "" });
            }
          
        }
        //public ActionResult Contract()
        //{
        //    return View();
        //}
        public ActionResult Discount()
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
        public ActionResult CustomerProduct(int CustomerID)
        {
            ViewBag.CustomerID = CustomerID;

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
        public ActionResult ExportExcel()
        {

            IWorkbook workbook = new HSSFWorkbook();
            ISheet sheet1 = workbook.CreateSheet("Sheet1");

            var cellStyleHeader = workbook.CreateCellStyle();
            cellStyleHeader.Alignment = HorizontalAlignment.Center;
            cellStyleHeader.VerticalAlignment = VerticalAlignment.Center;

            //cellStyleHeader.FillBackgroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
            //cellStyleHeader.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
            //cellStyleHeader.FillPattern = FillPattern.SolidForeground;
            //cellStyleHeader.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            //cellStyleHeader.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            //cellStyleHeader.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            //cellStyleHeader.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyleHeader.WrapText = true;

            var cellStyleRowRight = workbook.CreateCellStyle();
            cellStyleRowRight.Alignment = HorizontalAlignment.Right;
            cellStyleRowRight.VerticalAlignment = VerticalAlignment.Center;
            cellStyleRowRight.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyleRowRight.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyleRowRight.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyleRowRight.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            //cellStyleRowRight.WrapText = true;

            var cellStyleRowLeft = workbook.CreateCellStyle();
            cellStyleRowLeft.Alignment = HorizontalAlignment.Left;
            cellStyleRowLeft.VerticalAlignment = VerticalAlignment.Center;
            cellStyleRowLeft.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyleRowLeft.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyleRowLeft.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyleRowLeft.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            //cellStyleRowLeft.WrapText = true;

            var cellStyleRowCenter = workbook.CreateCellStyle();
            cellStyleRowCenter.Alignment = HorizontalAlignment.Center;
            cellStyleRowCenter.VerticalAlignment = VerticalAlignment.Center;
            cellStyleRowCenter.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyleRowCenter.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyleRowCenter.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyleRowCenter.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            //cellStyleRowCenter.WrapText = true;


            var cellStyleNumber4 = workbook.CreateCellStyle();
            cellStyleNumber4.DataFormat = workbook.CreateDataFormat().GetFormat("#,##0.0000");
            cellStyleNumber4.Alignment = HorizontalAlignment.Right;
            cellStyleNumber4.VerticalAlignment = VerticalAlignment.Center;
            cellStyleNumber4.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyleNumber4.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyleNumber4.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyleNumber4.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;

            var cellStyleNumber6 = workbook.CreateCellStyle();
            cellStyleNumber6.DataFormat = workbook.CreateDataFormat().GetFormat("#,##0.000000");
            cellStyleNumber6.Alignment = HorizontalAlignment.Right;
            cellStyleNumber6.VerticalAlignment = VerticalAlignment.Center;
            cellStyleNumber6.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyleNumber6.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyleNumber6.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyleNumber6.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;

            int _start = 0;
            IRow row1 = sheet1.CreateRow(_start);
            row1.Height = 400;
            IRow row2 = sheet1.CreateRow(_start + 1);
            row2.Height = 400;


            row1.CreateCell(0).SetCellValue("No.");
            row1.Cells[0].CellStyle = cellStyleHeader;
            row2.CreateCell(0).CellStyle = cellStyleHeader;
            sheet1.AddMergedRegion(new CellRangeAddress(_start, _start + 1, 0, 0));

            row1.CreateCell(1).SetCellValue("Date As Of");
            row1.Cells[1].CellStyle = cellStyleHeader;
            row2.CreateCell(1).CellStyle = cellStyleHeader;

            sheet1.AddMergedRegion(new CellRangeAddress(_start, _start + 1, 1, 1));

            row1.CreateCell(2).SetCellValue("Security Code");
            row1.Cells[2].CellStyle = cellStyleHeader;
            row2.CreateCell(2).CellStyle = cellStyleHeader;
            sheet1.AddMergedRegion(new CellRangeAddress(_start, _start + 1, 2, 2));

            row1.CreateCell(3).SetCellValue("Portfolio");
            row1.Cells[3].CellStyle = cellStyleHeader;
            row2.CreateCell(3).CellStyle = cellStyleHeader;
            sheet1.AddMergedRegion(new CellRangeAddress(_start, _start + 1, 3, 3));

            row1.CreateCell(4).SetCellValue("Fund Name");
            row1.Cells[4].CellStyle = cellStyleHeader;
            row2.CreateCell(4).CellStyle = cellStyleHeader;
            sheet1.AddMergedRegion(new CellRangeAddress(_start, _start + 1, 4, 4));

            row1.CreateCell(5).SetCellValue("Fund Code");
            row1.Cells[5].CellStyle = cellStyleHeader;
            row2.CreateCell(5).CellStyle = cellStyleHeader;
            sheet1.AddMergedRegion(new CellRangeAddress(_start, _start + 1, 5, 5));

            row1.CreateCell(6).SetCellValue("Securities Type");
            row1.Cells[6].CellStyle = cellStyleHeader;
            row2.CreateCell(6).CellStyle = cellStyleHeader;
            sheet1.AddMergedRegion(new CellRangeAddress(_start, _start + 1, 6, 6));

            row1.CreateCell(7).SetCellValue("Unit Amount");
            row1.Cells[7].CellStyle = cellStyleHeader;
            row2.CreateCell(7).CellStyle = cellStyleHeader;
            sheet1.AddMergedRegion(new CellRangeAddress(_start, _start + 1, 7, 7));

            row1.CreateCell(8).SetCellValue("Face Amount");
            row1.Cells[8].CellStyle = cellStyleHeader;
            row2.CreateCell(8).CellStyle = cellStyleHeader;
            sheet1.AddMergedRegion(new CellRangeAddress(_start, _start + 1, 8, 8));

            row1.CreateCell(9).SetCellValue("MTM");
            row1.Cells[9].CellStyle = cellStyleHeader;
            row2.CreateCell(9).CellStyle = cellStyleHeader;
            sheet1.AddMergedRegion(new CellRangeAddress(_start, _start + 1, 9, 9));


            //for (int i = 0; i < datareport.Count; i++)
            //{
            //    IRow row = sheet1.CreateRow((i + 1) + (_start + 1));
            //    row.Height = 310;

            //    row.CreateCell(0).SetCellValue((i + 1).ToString("N0"));
            //    row.Cells[0].CellStyle = cellStyleRowCenter;

            //    row.CreateCell(1).SetCellValue(datareport[i].DateAsOf);
            //    row.Cells[1].CellStyle = cellStyleRowLeft;

            //    row.CreateCell(2).SetCellValue(datareport[i].SECURITYCODE);
            //    row.Cells[2].CellStyle = cellStyleRowLeft;

            //    row.CreateCell(3).SetCellValue(datareport[i].PORTFOLIOCODE);
            //    row.Cells[3].CellStyle = cellStyleRowLeft;

            //    row.CreateCell(4).SetCellValue(datareport[i].PORTFOLIONAME);
            //    row.Cells[4].CellStyle = cellStyleRowLeft;

            //    row.CreateCell(5).SetCellValue(datareport[i].FUNDCODE);
            //    row.Cells[5].CellStyle = cellStyleRowLeft;

            //    row.CreateCell(6).SetCellValue(datareport[i].SECURITYTYPE);
            //    row.Cells[6].CellStyle = cellStyleRowLeft;


            //    row.CreateCell(7).SetCellValue(Convert.ToDouble(datareport[i].UNIT.ToString()));
            //    row.Cells[7].CellStyle = cellStyleNumber4;


            //    row.CreateCell(8).SetCellValue(Convert.ToDouble(datareport[i].FACEAMOUNT.ToString()));
            //    row.Cells[8].CellStyle = cellStyleNumber4;


            //    row.CreateCell(9).SetCellValue((Convert.ToDouble(datareport[i].PRICE.ToString())));
            //    row.Cells[9].CellStyle = cellStyleNumber6;

            //    if (i == datareport.Count - 1)
            //    {
            //        row = sheet1.CreateRow((i + 2) + (_start + 1));
            //        row.Height = 310;
            //        row.CreateCell(0).SetCellValue("");
            //        row.Cells[0].CellStyle = cellStyleRowRight;
            //        row.CreateCell(1).SetCellValue("");
            //        row.Cells[1].CellStyle = cellStyleRowRight;
            //        row.CreateCell(2).SetCellValue("");
            //        row.Cells[2].CellStyle = cellStyleRowRight;
            //        row.CreateCell(3).SetCellValue("");
            //        row.Cells[3].CellStyle = cellStyleRowRight;
            //        row.CreateCell(4).SetCellValue("");
            //        row.Cells[4].CellStyle = cellStyleRowRight;
            //        row.CreateCell(5).SetCellValue("");
            //        row.Cells[5].CellStyle = cellStyleRowRight;

            //        row.CreateCell(6).SetCellValue("Total");
            //        row.Cells[6].CellStyle = cellStyleRowCenter;

            //        row.CreateCell(7).SetCellValue(Convert.ToDouble(datareport.Sum(s => s.UNIT).ToString()));
            //        row.Cells[7].CellStyle = cellStyleNumber4;


            //        row.CreateCell(8).SetCellValue(Convert.ToDouble(datareport.Sum(s => s.FACEAMOUNT).ToString()));
            //        row.Cells[8].CellStyle = cellStyleNumber4;

            //        row.CreateCell(9).SetCellValue("");
            //        row.Cells[9].CellStyle = cellStyleRowRight;
            //    }
            //}
            //sheet1.SetColumnWidth(0, 1400);
            //sheet1.SetColumnWidth(1, 4000);
            //sheet1.SetColumnWidth(2, 4000);
            //sheet1.SetColumnWidth(3, 4000);
            //sheet1.SetColumnWidth(4, 4200);
            //sheet1.SetColumnWidth(5, 4200);
            //sheet1.SetColumnWidth(6, 4200);
            //sheet1.SetColumnWidth(7, 4500);
            //sheet1.SetColumnWidth(8, 5200);
            //sheet1.SetColumnWidth(9, 3300);


            using (var export = new MemoryStream())
            {
                System.Web.HttpContext.Current.Response.Clear();
                workbook.Write(export);
                System.Web.HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
                System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", "HoldingCrossBySecuritiesReport_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xls"));
                System.Web.HttpContext.Current.Response.BinaryWrite(export.GetBuffer());
                System.Web.HttpContext.Current.Response.Flush();
                System.Web.HttpContext.Current.Response.End();
            }

            return View();
        }


        public ActionResult ExportPDF(Customers item)
        {
            IList<Customers> listData = new List<Customers>();
            CustomersData data = new CustomersData();
            listData = data.Get(item);

            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("pCondition", listData[0].CustomerFirstName + " " + listData[0].CustomerLastName);

            param.Add("pAddress", "  100 ต.ควนมะพร้าว อ.เมืองพัทลุง จ.พัทลุง 10000 เบอร์โทร 081-476-2091");
            Utility.ExportPDF(
                "RedemptionReport_" + DateTime.Now.ToString("yyyyMMdd_HHmmss"),
                "~/Report/CustomerReport.rpt",
                 listData,
                param
            );
            return View();
        }
        // GET: /Customers/GetCustomers/:id
        public JsonResult GetCustomers(int id)
        {


            try
            {
                IList<Customers> listData = new List<Customers>();
                CustomersData data = new CustomersData();
                listData = data.Get(id);
                var jsonResult = Json(new
                {
                    data = listData,
                    success = true
                }, JsonRequestBehavior.AllowGet);

                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
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

        // GET: /Customers/GetListCustomers/:id
        public JsonResult GetListCustomers(int id)
        {


            try
            {
                IList<Customers> listData = new List<Customers>();
                CustomersData data = new CustomersData();
                listData = data.GetListCustomers(id);

                var jsonResult = Json(new
                {
                    data = listData,
                    success = true
                }, JsonRequestBehavior.AllowGet);

                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;

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

        // GET: /Customers/GetCustomerByZone?zoneId=:zoneId
        public JsonResult GetCustomerByZone(int zoneId) {


            try
            {
                IList<Customers> listData = new List<Customers>();
                CustomersData data = new CustomersData();
               listData = data.GetCustomerByZone(zoneId);

                var jsonResult = Json(new
                {
                    data = listData,
                    success = true
                }, JsonRequestBehavior.AllowGet);

                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;

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

        [HttpPost]
        public JsonResult GetCustomers(Customers item)
        {


            try
            {
                IList<Customers> listData = new List<Customers>();
                CustomersData data = new CustomersData();
                listData = data.Get(item);


                var jsonResult = Json(new
                {
                    data = listData,
                    success = true
                }, JsonRequestBehavior.AllowGet);

                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
               
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
        [HttpPost]
        public ActionResult AddCustomers(Customers item)
        {
            try
            {


                if (Session["iuser"] == null)
                    throw new Exception(" Session หมดอายุ , กรุณาเข้าสู่ระบบใหม่อีกครั้ง !! ");

                item.CustomerInsertBy = (Int32)Session["iuser"];

                new CustomersData().AddCustomer(ref item);

                return RedirectToAction("Index", "Customers");

            }
            catch (Exception ex)
            {

              //  return RedirectToAction("AddCustomer");
                return View();
            }
        }

        [HttpPost]
        // POST: /Customers/EditCustomers
        public JsonResult EditCustomers(Customers item)
        {

            try
            {

                if (Session["iuser"] == null)
                    throw new Exception(" Session หมดอายุ , กรุณาเข้าสู่ระบบใหม่อีกครั้ง !! ");

                item.CustomerUpdateBy = (Int32)Session["iuser"];

                new CustomersData().EditCustomer(ref item);

                return Json(new
                {
                    data = "บันทึกการแก้ไขข้อมูลสำเร็จ",
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
        // POST: /Customers/DeleteCustomers?CustomerID=
        public JsonResult DeleteCustomers(int CustomerID)
        {

            try
            {
                new CustomersData().DeleteCustomer(CustomerID);

                return Json(new
                {
                    data = "ลบข้อมูลลูกค้าสำเร็จ",
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
       
        // GET: /Customers/GetCustomerID/:id

        public JsonResult GetCustomerID(int id)
        {
        

            Customers cus_info = new Customers();
            CustomersData data3 = new CustomersData();
            cus_info = data3.GetCustomerInfo_ByID(id);

            StaffData st = new StaffData();
            DataTable dt = new DataTable();
            List<Staffs> staffList=new List<Staffs>();
            dt = st.GetStaffRole(0, 5);
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

                }).ToList();

            }
            return Json(new
            {
                dataCustomer = cus_info,
                dataZone = staffList,
                success = true
            }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        // POST: /Customers/PostChangeMobilePhone
        public JsonResult PostChangeMobilePhone(Customers item)
        {

            try
            {
                CustomersData CD = new CustomersData();
                CD.GetChangeMobilePhone(item);

                return Json(new
                {
                    data = "บันทึกการแก้ไขข้อมูลสำเร็จ",
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

        //  POST: /Customers/ExportCard

        public FileResult ExportCard(int ContractID, int CustomerID)
        {
           
            IList<Customers> listData = new List<Customers>();
            CustomersData data = new CustomersData();
            listData = data.Get(CustomerID);

            try
            {

                string yourcode = CustomerID + ":" + ContractID;
                string firstText = listData[0].CustomerName;
                string secondText = "(" + listData[0].CustomerNickName + ")";

                PointF firstLocation = new PointF(550f, 250f);
                PointF secondLocation = new PointF(550f, 300f);

                string imageFilePath = Server.MapPath("~/Content/bg_1.png");

                Bitmap bitmap = new Bitmap(1800, 1400);
                bitmap = (Bitmap)Image.FromFile(imageFilePath);//load the image file

                //string imageFilePath1 = Server.MapPath("~/Content/qr1.png");
                //Bitmap bitmap1 = new Bitmap(400, 400);
                //bitmap1 = (Bitmap)Image.FromFile(imageFilePath1.ToString());//load the image file

              
                QRCodeEncoder enc = new QRCodeEncoder();
                enc.QRCodeScale = 5;
                Bitmap qrcode = enc.Encode(yourcode);
           

                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    using (Font arialFont = new Font("Cordia New", 30))
                    {
                        graphics.DrawString(firstText, arialFont, Brushes.White, firstLocation);
                        graphics.DrawString(secondText, new Font("Cordia New", 30), Brushes.White, secondLocation);


                        graphics.DrawImage(qrcode.Clone(new Rectangle(0, 0, qrcode.Width - 1, qrcode.Height - 1), qrcode.PixelFormat), new Point(200, 200));



                    }
                }
                MemoryStream ms = new MemoryStream();
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

                bitmap.Save(Server.MapPath("~/Content/img1.jpg"), System.Drawing.Imaging.ImageFormat.Png);

                return File(Server.MapPath("~/Content/img1.jpg"), System.Net.Mime.MediaTypeNames.Application.Octet,
                                       "MEMBER"+(CustomerID*1000) +ContractID+".png");
            }
            catch (Exception)
            {

                throw;
            }
        }



    }
}
