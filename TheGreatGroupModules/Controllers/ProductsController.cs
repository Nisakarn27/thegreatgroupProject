
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheGreatGroupModules.Models;
using TheGreatGroupModules.Modules;

namespace TheGreatGroupModules.Controllers
{
    public class ProductsController : Controller
    {
        //
        // GET: /Products/

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

        public ActionResult AddProduct()
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

        public ActionResult EditProduct(int ProductID)
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

        

        [HttpPost]
        public JsonResult GetListProductCart(int CustomerID, int productID,int unit)
        {

            List<ProductSelect> ProductData = new List<ProductSelect>();

            try
            { 
                ProductData product = new ProductData();
                ProductSelect selectItem = new ProductSelect();
                selectItem.ProductID = productID;
                selectItem.Unit = unit;
                product.GetProductPrice(ref selectItem);
                ProductData.Add(selectItem);

                   return Json(new
                {
                    data = ProductData,
                    success = true
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                   return Json(new
                {
                    data = ProductData,
                    success = false
                }, JsonRequestBehavior.AllowGet);
            }



         
        }



        // GET: /Products/PostAddProduct?CustomerID=1&ContractID=1
        [HttpPost]
        public JsonResult PostAddProduct(ProductSelect product,
            int CustomerID, int ContractID)
        {
            try
            {
                ContractData cd = new ContractData();

                // ลบสินค้าเดิมทั้งหมด แก้จำนวนเงินสินค้าทั้งหมดในสัญญา
                cd.Deleted_Product_customer(CustomerID, ContractID);


                List<ProductSelect> products = new List<ProductSelect>();
                products.Add(product);

                // เพิ่มสินค้า
                ProductData pd = new ProductData();
                pd.AddProductSelect(products, CustomerID, ContractID);

                // getProduct By Contract
                ProductData dataPro = new ProductData();
                IList<ProductSelect> listProductsSelect = new List<ProductSelect>();
                listProductsSelect = dataPro.GetProductCustomer(CustomerID, ContractID);

                double ContractPayment = 0;
                List<ProductSelect> listProductsSelect1 = new List<ProductSelect>();
                listProductsSelect1 = dataPro.ProductContractSummary(ref ContractPayment, listProductsSelect);

          
               
                // update จำนวนเงินสินค้าทั้งหมด 
                cd.UpdateContractPayment(ContractID, CustomerID, ContractPayment);


                // คำนวณ ค่างวด / วันสิ้นสุดสัญญา
                cd.UpdateContractAmount_ContractExpDate(CustomerID, ContractID);



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

        // Get: /Products/GetListProduct/1
          [HttpGet]
        public JsonResult GetListProduct(int id)
        {


            try
            {


                if (Session["iuser"] == null)
                    throw new Exception(" Session หมดอายุ , กรุณาเข้าสู่ระบบใหม่อีกครั้ง !! ");

               
                Products item = new Products();
                ProductData data = new ProductData();
                List<Products> listdata = data.GetListProduct(id);
                IList<ProductGroup> listProductGroup =new List<ProductGroup>();
                    listProductGroup= data.GetListProductGroup();

                return Json(new
                {
                    data = listdata,
                    dataProductGroup=listProductGroup,
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
          public JsonResult AddProduct(Products item)
          {


              try
              {


                  if (Session["iuser"] == null)
                      throw new Exception(" Session หมดอายุ , กรุณาเข้าสู่ระบบใหม่อีกครั้ง !! ");


                  ProductData data = new ProductData();
                  data.AddProduct(item);

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
                      success = false,
                      data = ex.Message
                  }, JsonRequestBehavior.AllowGet);

              }
          }

          // ../Products/EditProduct
          [HttpPost]
          public JsonResult EditProduct(Products item)
          {


              try
              {


                  if (Session["iuser"] == null)
                      throw new Exception(" Session หมดอายุ , กรุณาเข้าสู่ระบบใหม่อีกครั้ง !! ");


                  ProductData data = new ProductData();
                  data.UpdateProduct(item);

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
                      success = false,
                      data = ex.Message
                  }, JsonRequestBehavior.AllowGet);

              }
          }


          // ../Products/DeletedProduct/id
          public JsonResult DeletedProduct(int id)
          {
              try
              {

                  if (Session["iuser"] == null)
                      throw new Exception(" Session หมดอายุ , กรุณาเข้าสู่ระบบใหม่อีกครั้ง !! ");


                  ProductData data = new ProductData();
                  data.DeletedProduct(id);

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
                      success = false,
                      data = ex.Message
                  }, JsonRequestBehavior.AllowGet);

              }
          }


          // ../Products/ActivatedProduct/id
          public JsonResult ActivatedProduct(int id)
          {
              try
              {

                  if (Session["iuser"] == null)
                      throw new Exception(" Session หมดอายุ , กรุณาเข้าสู่ระบบใหม่อีกครั้ง !! ");

                  ProductData data = new ProductData();
                  data.ActivatedProduct(id);


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
                      success = false,
                      data = ex.Message
                  }, JsonRequestBehavior.AllowGet);

              }
          }


    }
}
