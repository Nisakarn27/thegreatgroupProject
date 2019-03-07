using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheGreatGroupModules.Models;
using TheGreatGroupModules.Modules;

namespace TheGreatGroupModules.Controllers
{
    public class ContractController : Controller
    {
        //
        // GET: /Contract/

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


        public ActionResult AccountStatus()
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


        
        // GET: /Customers/GetListContract/:id
        public JsonResult GetListContract(int CustomerID)
        {
            try
            {

                IList<Contract> listContract = new List<Contract>();
                ContractData cd = new ContractData();
                listContract = cd.GetListContract(CustomerID, 0);
                return Json(new
                {
                    data = listContract,
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

        // GET: /Customers/GetContract?CustomerID=id&ContractID=id
        public JsonResult GetContract(int CustomerID, int ContractID)
        {
            try
            {
                IList<Contract> listContract = new List<Contract>();
                ContractData cd = new ContractData();
                listContract = cd.GetContract(CustomerID, ContractID);

                IList<Customers> listData = new List<Customers>();
                CustomersData dataCus = new CustomersData();
                listData = dataCus.Get(CustomerID);

                ProductData dataPro = new ProductData();
                IList<ProductSelect> listProductsSelect = new List<ProductSelect>();

                IList<Products> listProduct = new List<Products>();
                listProduct = dataPro.GetListProduct();


                List<ProductSelect> listProductsSelect1 = new List<ProductSelect>();
              
                if (ContractID > 0) {
                    listProductsSelect = dataPro.GetProductCustomer(CustomerID, ContractID);
                }
                double ContractPayment = 0;
                listProductsSelect1 = dataPro.ProductContractSummary(ref ContractPayment, listProductsSelect);

            


                return Json(new
                {
                    data = listContract,
                    dataCustomers = listData,
                    dataProduct = listProduct,
                    dataProductSelect = listProductsSelect1,
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

        // เพิ่มข้อมูลสัญญา
        // POST: /Customers/PostAdd_NewContract
        [HttpPost]
        public JsonResult PostAdd_NewContract(Contract item)
        {

            ContractData CD = new ContractData();
            int Surety1 = 0;
            int Surety2 = 0;
            int ContractID = 0;
            int parner = 0;
            try
            {
                if (Session["iuser"]==null)
                    throw new Exception(" Session หมดอายุ , กรุณาเข้าสู่ระบบใหม่อีกครั้ง !! ");

                item.ContractInsertBy = (Int32)Session["iuser"];

                ContractID = CD.Add_NewContract(item);


                if (item.CustomerSuretyData1 != null)
                {
                    Surety1 = CD.Add_Surety(item.CustomerSuretyData1);
                }

                if (item.CustomerSuretyData2 != null)
                {
                    Surety2 = CD.Add_Surety(item.CustomerSuretyData2);
                }

                if (item.CustomerPartnerData != null)
                {
                    parner = CD.Add_Partner(item.CustomerPartnerData);
                }

                item.ContractID = ContractID;
                item.CustomerSurety1 = Surety1;
                item.CustomerSurety2 = Surety2;
                item.CustomerPartner = parner;

                //Update Product this Contract
                CD.Update_Product_customer(item);
                // getProduct By Contract
                ProductData dataPro = new ProductData();
                IList<ProductSelect> listProductsSelect = new List<ProductSelect>();
                listProductsSelect = dataPro.GetProductCustomer(item.ContractCustomerID, ContractID);
                item.ContractPayment = Convert.ToDecimal(listProductsSelect.Sum(c => c.ProductPrice));
                // Update Contract Surety
                CD.UpdateSurety_In_Contract(item);

                

            


                return Json(new
                {
                    ContractID = ContractID,
                    data = "บันทึกการทำสัญญาสำเร็จ",
                    success = true
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    ContractID = 0,
                    data = ex.Message,
                    success = false
                }, JsonRequestBehavior.AllowGet);

            }

        }


        // แก้ไขข้อมูลสัญญา
        // POST: /Customers/PostEdit_Contract
        [HttpPost]
        public JsonResult PostEdit_Contract(Contract item)
        {
            int Surety1 = 0;
            int Surety2 = 0;
            int parner = 0;
            try
            {
                if (Session["iuser"] == null)
                    throw new Exception(" Session หมดอายุ , กรุณาเข้าสู่ระบบใหม่อีกครั้ง !! ");

                item.ContractInsertBy = (Int32)Session["iuser"];


                ContractData CD = new ContractData();
                CD.Edit_NewContract(item);

                if (item.CustomerPartnerData != null) {
                    //check insert Partner
                    if (item.CustomerPartner == 0)
                    {
                        parner = CD.Add_Partner(item.CustomerPartnerData);
                        item.CustomerPartner = parner;
                    }
                    else
                    {
                        parner = CD.Update_Partner(item.CustomerPartnerData);
                    }

                }
             

                //check insert Surety1
                if (item.CustomerSuretyData1 != null)
                {
                    if (item.CustomerSurety1 == 0 & item.CustomerSuretyData1 != null)
                    {
                        Surety1 = CD.Add_Surety(item.CustomerSuretyData1);
                        item.CustomerSurety1 = Surety1;
                    }
                    else
                    {
                        Surety1 = CD.Update_Surety(item.CustomerSuretyData1);
                    }
                }


                //check insert Surety1
                if (item.CustomerSuretyData2 != null)
                {
                    if (item.CustomerSurety2 == 0 & item.CustomerSuretyData2 != null)
                    {
                        Surety2 = CD.Add_Surety(item.CustomerSuretyData2);
                        item.CustomerSurety2 = Surety2;
                    }
                    else
                    {
                        Surety2 = CD.Update_Surety(item.CustomerSuretyData2);
                    }
                }

                //Update Product this Contract
                CD.Update_Product_customer(item);


                // getProduct By Contract
                ProductData dataPro = new ProductData();
                IList<ProductSelect> listProductsSelect = new List<ProductSelect>();
                listProductsSelect = dataPro.GetProductCustomer(item.ContractCustomerID, item.ContractID);
                
         




                double ContractPayment = 0;
                List<ProductSelect> listProductsSelect1 = new List<ProductSelect>();
                listProductsSelect1 = dataPro.ProductContractSummary(ref ContractPayment, listProductsSelect);

                item.ContractPayment = Convert.ToDecimal(Math.Round(ContractPayment,2));

                // update จำนวนเงินสินค้าทั้งหมด 
                CD.UpdateContractPayment(item.ContractID, item.ContractCustomerID, ContractPayment);

                // Update Contract Surety
              
                CD.UpdateSurety_In_Contract(item);

                // คำนวณ ค่างวด / วันสิ้นสุดสัญญา
                
                CD.UpdateContractAmount_ContractExpDate(item.ContractCustomerID, item.ContractID);
                return Json(new
                {
                    data = "บันทึกการแก้ไขสัญญาสำเร็จ",
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
        public JsonResult ActivatedContract(int ContractID)
        {
            ContractData data = new ContractData();

            try
            {
                if (Session["iuser"] == null)
                    throw new Exception(" Session หมดอายุ , กรุณาเข้าสู่ระบบใหม่อีกครั้ง !! ");

                int UpdateBy = (Int32)Session["iuser"];

                data.ActivatedContract(ContractID, UpdateBy);

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
        // GET: /Customers/GetDataPurchaseData/:id
        public JsonResult GetDataPurchaseData(int id)
        {


            try
            {
                IList<Customers> listData = new List<Customers>();
                CustomersData data = new CustomersData();
                listData = data.Get(id);


                ProductData dataPro = new ProductData();
                IList<Products> listProduct = new List<Products>();
                listProduct = dataPro.GetListProduct();

                IList<ProductSelect> listProductsSelect = new List<ProductSelect>();
                listProductsSelect = dataPro.GetProductCustomer(id, 0);

                return Json(new
                {
                    data = listData,
                    dataProduct = listProduct,
                    dataProductSelect = listProductsSelect,
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



        // GET: /Contract/GetApproveOpen_CloseContract?custpmerIDCard=:id
        public JsonResult GetApproveOpen_CloseContract(string custpmerIDCard)
           {


            try
            {
                List<DailyReceiptsReport> listDataOpen = new List<DailyReceiptsReport>();

                List<DailyReceiptsReport> listDataClose = new List<DailyReceiptsReport>();
                ContractData data = new ContractData();
                listDataOpen = data.GetApproveOpen_CloseContract(custpmerIDCard, "1");
                listDataClose = data.GetApproveOpen_CloseContract(custpmerIDCard, "0");

                return Json(new
                {
                    dataOpen = listDataOpen,
                    dataClose = listDataClose,
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


        // POST: /Contract/PostAddDiscount
        public JsonResult PostAddDiscount(DailyReceiptsReport item) {


            try
            {

                if (Session["iuser"] == null)
                    throw new Exception(" Session หมดอายุ , กรุณาเข้าสู่ระบบใหม่อีกครั้ง !! ");

                item.StaffID = (Int32)Session["iuser"];

                ContractData data = new ContractData();
                data.AddDiscount(item);


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



        [HttpPost]
        public JsonResult PostAddContractAmountLast(Contract item)
        {
            try
            {
                if (Session["iuser"] == null)
                    throw new Exception(" Session หมดอายุ , กรุณาเข้าสู่ระบบใหม่อีกครั้ง !! ");

                item.ContractInsertBy = (Int32)Session["iuser"];

                IList<Contract> listContract = new List<Contract>();
                ContractData cd = new ContractData();
                listContract = cd.GetContract(item.ContractCustomerID, item.ContractID);
                item = new Contract();
                item = listContract[0];
                ProductData pd = new ProductData();
               pd.AddDailyReceipt(item.ContractPayment,item.ContractAmountLast,item.ContractCustomerID,item.ContractID);


                return Json(new
                {
                    data = "บันทึกค่างวดแรกสำเร็จ",
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
        
    }
}
