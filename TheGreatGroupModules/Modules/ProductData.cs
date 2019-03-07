using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using TheGreatGroupModules.Models;

namespace TheGreatGroupModules.Modules
{
    public class ProductData
    {

        private string errMsg = "";
        public IList<Products> GetListProduct()
        {

            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);

            try
            {

                string StrSql = @" SELECT pg.ProductGroupName,p.* FROM products p 
                                LEFT JOIN product_group pg ON p.ProductGroupID=pg.ProductGroupID
                                WHERE p.Activated=1 AND p.Deleted=0
                                ";
                StrSql += @" order by  p.ProductGroupID Asc ,p.UnitAmount DESC";
                DataTable dt = DBHelper.List(StrSql, ObjConn);
                IList<Products> listData = new List<Products>();
                if (dt != null && dt.Rows.Count > 0)
                {
                    listData = Products.ToObjectList(dt);
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

        public void GetProductPrice(ref ProductSelect item)
        {

            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);

            try
            {

                string StrSql = @" SELECT pg.ProductGroupName,p.* FROM products p 
                                LEFT JOIN product_group pg ON p.ProductGroupID=pg.ProductGroupID
                                WHERE p.Activated=1 AND p.Deleted=0
                                ";


                if (item.ProductID > 0)
                {

                    StrSql += @" and p.ProductID=" + item.ProductID;
                }

                StrSql += @" Order by pg.ProductGroupName ASC";
                DataTable dt = DBHelper.List(StrSql, ObjConn);
                int productgroupId = 0;
                SettingData data = new SettingData();
                List<GoldData> datanew = new List<GoldData>();
                datanew = data.GetPriceGold();
                if (dt.Rows.Count > 0)
                {

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        productgroupId = Convert.ToInt32(dt.Rows[i]["ProductGroupID"].ToString());
                        item.ProductGroupID = productgroupId;
                        if (productgroupId == 1)
                        {
                            item.ProductPrice = Math.Round((Convert.ToDouble(datanew[1].ask) / 15.20) * Convert.ToDouble(dt.Rows[i]["UnitAmount"].ToString()),2);
                            item.ProductPrice_Text = item.ProductPrice.ToString("#,###.00");
                            item.TotalPrice_Text = item.ProductPrice.ToString("#,###.00");
                            item.ProductName = dt.Rows[i]["ProductGroupName"].ToString() + " " + dt.Rows[i]["ProductName"].ToString() +
                                 " (" + dt.Rows[i]["UnitAmount"].ToString() + " " + dt.Rows[i]["UnitName"].ToString() + ")";
                            item.PriceGold = Math.Round(Convert.ToDouble(datanew[1].ask), 2);
                            item.PriceGoldReceipt = Math.Round(Convert.ToDouble(datanew[1].bid), 2);
                        }
                        else if (productgroupId == 2)
                        {
                            item.ProductPrice = Math.Round((Convert.ToDouble(datanew[1].ask) / 15.20) * Convert.ToDouble(dt.Rows[i]["UnitAmount"].ToString()), 2);
                            item.ProductPrice_Text = item.ProductPrice.ToString("#,###.00");
                            item.TotalPrice_Text = item.ProductPrice.ToString("#,###.00");
                            item.ProductName = dt.Rows[i]["ProductGroupName"].ToString() + " " + dt.Rows[i]["ProductName"].ToString() +
                                                           " (" + dt.Rows[i]["UnitAmount"].ToString() + " " + dt.Rows[i]["UnitName"].ToString() + ")";
                            item.PriceGold = Math.Round(Convert.ToDouble(datanew[1].ask),2);
                            item.PriceGoldReceipt = Math.Round(Convert.ToDouble(datanew[1].bid), 2); ;
                        }
                        else
                        {
                            item.ProductName = dt.Rows[i]["ProductName"].ToString();
                            item.ProductPrice = Math.Round(Convert.ToDouble(dt.Rows[i]["ProductPrice"].ToString()), 2);
                            item.PriceGold = 0;
                            item.PriceGoldReceipt = 0;
                        }



                    }

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




        public void AddProductSelect(List<ProductSelect> products,
            int CustomerID, int ContractID)
        {
            ProductSelect product = new ProductSelect();
            if (products.Count > 0)
            {

                for (int i = 0; i < products.Count; i++)
                {
                    product = new ProductSelect();
                    product = products[i];
                    GetProductPrice(ref product);

                    // AddData
                    AddCustomerProduct(product, CustomerID, ContractID);
                }

            }



        }



        public void AddCustomerProduct(
            ProductSelect products,
            int CustomerID, int ContractID)
        {

            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);
            try
            {

                string StrSql = @"INSERT INTO product_customer
                    (CustomerID,
                     ContractID,
                     ProductID,
                     ProductGroupID,
                     PriceGold,
                     PriceGoldReceipt,
                     Unit,
                     ProductPrice,
                     deleted,
                     IsCal)
                    VALUES ({0}, {1}, {2}, {3}, {4},{5},{6}, {7}, {8}, {9});";

                StrSql = String.Format(StrSql
                         , CustomerID
                         , ContractID
                         , products.ProductID
                         , products.ProductGroupID
                         , products.PriceGold
                         , products.PriceGoldReceipt
                         , products.Unit
                         , products.ProductPrice
                         , 0
                         , 0
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

        public void AddDailyReceipt(decimal TotalSales, decimal ContractAmountLast,
       
         int CustomerID, int ContractID)
        {

            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);
            try
            {
                string StrSql = " Update daily_receipts Set Activated=0,Deleted=1 where CustomerID=" + CustomerID + " And ContractID=" + ContractID + " ;";

                DBHelper.Execute(StrSql, ObjConn);

                StrSql = " Update contract Set IsContractAmountLast=1 where ContractCustomerID=" + CustomerID + " And ContractID=" + ContractID + " ;";
                StrSql += @" INSERT INTO daily_totalreceipts(ID,CustomerID,ContractID,DateAsOf,
            TotalSales,PriceReceipts,Principle,Interest,StaffID,Activated,Deleted)
            VALUES("

            + Utility.GetMaxID("daily_receipts", "ID") + ","
            + CustomerID + ","
            + ContractID + ","
            + Utility.FormateDateTime(DateTime.Now) + ","
            + TotalSales + ","
            + ContractAmountLast + ","
            + 0 + ","
            + 0 + ","
            + 0 + ","
            + 0 + ","
            + 0 + ");";
            
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


        public List<ProductSelect> GetProductCustomer(int CustomerID, int ContractID)
        {


            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);
            try
            {

                string StrSql = @"SELECT pg.ProductGroupName,p.ProductName,p.UnitAmount,p.UnitName,
            pc.*,c.ContractReward ,c.ContractInterest,c.ContractType
                FROM product_customer pc 
                LEFT JOIN products  p  ON pc.ProductID=p.ProductID
                LEFT JOIN product_group  pg  ON p.ProductGroupID=pg.ProductGroupID
                 LEFT JOIN contract  c  ON pc.ContractID=c.ContractID
                WHERE 0=0 ";
                if (CustomerID > 0)
                    StrSql += " AND pc.CustomerID=" + CustomerID;

                //   if (ContractID > 0)
                StrSql += " AND pc.ContractID=" + ContractID;

                DataTable dt = DBHelper.List(StrSql, ObjConn);


                List<ProductSelect> product = new List<ProductSelect>();

                ProductSelect data = new ProductSelect();
                if (dt.Rows.Count > 0 && dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        data.ProductGroupID = Convert.ToInt32(dt.Rows[i]["ProductGroupID"]);
                        if (data.ProductGroupID == 1 || data.ProductGroupID == 2)
                        {
                            if (dt.Rows[i]["ContractInterest"] != DBNull.Value)
                                data.ContractInterest = Convert.ToDouble(dt.Rows[i]["ContractInterest"].ToString());

                            data.PriceGoldReceipt = Convert.ToDouble(dt.Rows[i]["PriceGoldReceipt"].ToString());
                            data.PriceGold = Convert.ToDouble(dt.Rows[i]["PriceGold"].ToString());
                            data.ProductID = Convert.ToInt32(dt.Rows[i]["ProductID"]);
                            data.ProductName = dt.Rows[i]["ProductGroupName"].ToString() + " "
                                + dt.Rows[i]["ProductName"].ToString() + " ( " + dt.Rows[i]["UnitAmount"].ToString()
                                + dt.Rows[i]["UnitName"] + " )";
                            data.Unit = Convert.ToInt32(dt.Rows[i]["Unit"]);
                            data.ProductPrice = Math.Round((Convert.ToDouble(dt.Rows[i]["PriceGold"].ToString())
                                / 15.20) * Convert.ToDouble(dt.Rows[i]["UnitAmount"].ToString()),2);

                            if (dt.Rows[i]["ContractInterest"] != DBNull.Value)
                                data.ContractReward = Convert.ToDouble(dt.Rows[i]["ContractReward"].ToString());

                            data.UnitAmount = Convert.ToDouble(dt.Rows[i]["UnitAmount"].ToString());

                            if (dt.Rows[i]["ContractType"] != DBNull.Value)
                            data.ContractType =dt.Rows[i]["ContractType"].ToString();
                            
                        }
                        else
                        {

                            data.ProductID = Convert.ToInt32(dt.Rows[i]["ProductID"]);
                            data.ProductName = dt.Rows[i]["ProductGroupName"].ToString() + " "
                                + dt.Rows[i]["ProductName"].ToString() + "(" + dt.Rows[i]["UnitAmount"].ToString()
                                + dt.Rows[i]["UnitName"] + ")";

                            data.Unit = Convert.ToInt32(dt.Rows[i]["Unit"]);
                            data.ProductPrice = Convert.ToDouble(dt.Rows[i]["ProductPrice"].ToString());
                            data.ContractReward = 0;
                            data.PriceGoldReceipt = 0;
                            data.PriceGold = 0;
                            data.UnitAmount = 0;
                            if (dt.Rows[i]["ContractType"] != DBNull.Value)
                                data.ContractType = dt.Rows[i]["ContractType"].ToString();
                        }

                        product.Add(data);
                    }

                }

                return product;
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


        public List<ProductSelect> ProductContractSummary(ref double ContractPayment, IList<ProductSelect> products)
        {
            List<ProductSelect> listproduct = new List<ProductSelect>();
            try
            {
                ProductSelect product = new ProductSelect();
                //double
                int no = 0;
                double ProductPrice = 0; // ราคาสินค้า
                double Vat = 0.07; // ภาษี 
                double Reward = 0; // ค่ากำเหน็จ
                double PriceGoldReceipt = 0; // ราคารับซื้อทอง
                //  double TotalPriceip = 0; // ราคารวม
                double interest = 0; // ดอกเบี้ย
                double TotlalVat = 0;
                if (products.Count > 0)
                {


                    for (int i = 0; i < products.Count; i++)
                    {
                        product = new ProductSelect();
                        product = products[i];
                        Reward = products[i].ContractReward;
                        PriceGoldReceipt = products[i].PriceGoldReceipt;
                        interest = products[i].ContractInterest / 100;
                        no = (i + 1);

                   
                        if (products[i].ProductGroupID == 2)
                        {

                            //ราคาทอง
                            ProductPrice += (products[i].ProductPrice + Reward + Math.Round((products[i].ProductPrice * interest), 2)) * products[i].Unit;



                            //สินค้าทองรูปพรรณ
                            product = SetProductPrice.ProductNormal(no.ToString(), (ProductPrice - Reward), products[i].Unit);
                            product.ProductName = products[i].ProductName;
                            listproduct.Add(product);

                            //ค่ากำเหน็จ
                            product = new ProductSelect();
                            product = SetProductPrice.ProductNormal("", Reward, 0);
                            product.ProductName = "ค่ากำเหน็จ";
                            listproduct.Add(product);

                            // หักประกาศรับซื้อทองประจำวัน 
                            product = new ProductSelect();
                            product = SetProductPrice.Product_ReceiptGoldDay(PriceGoldReceipt, products[i].UnitAmount);
                            listproduct.Add(product);

                            // ส่วนต่างที่คิดภาษีมูลค่าเพิ่ม

                            product = new ProductSelect();
                            product = SetProductPrice.Product_VatDiff(ref TotlalVat, ProductPrice, PriceGoldReceipt, products[i].UnitAmount);
                            listproduct.Add(product);

                            // รวมเงิน
                            product = new ProductSelect();
                            product = SetProductPrice.Product_TotalPrice(ProductPrice);
                            listproduct.Add(product);


                            //ไม่คิด Vat
                            if (products[i].ContractType == "GP")
                            {

                                // ภาษีมูลค่าเพิ่ม
                                product = new ProductSelect();
                                product = SetProductPrice.Product_TotalVat(ProductPrice, PriceGoldReceipt, products[i].UnitAmount, 0);
                                listproduct.Add(product);

                                // รวมเงินทั้งสิ้น
                                product = new ProductSelect();
                                product = SetProductPrice.Product_TotalSale(ref ContractPayment, ProductPrice, PriceGoldReceipt, products[i].UnitAmount, 0);
                                listproduct.Add(product);
                            }

                            else if (products[i].ContractType == "G")
                            {



                                // ภาษีมูลค่าเพิ่ม
                                product = new ProductSelect();
                                product = SetProductPrice.Product_TotalVat(ProductPrice, PriceGoldReceipt, products[i].UnitAmount, Vat);
                                listproduct.Add(product);

                                // รวมเงินทั้งสิ้น
                                product = new ProductSelect();
                                product = SetProductPrice.Product_TotalSale(ref ContractPayment, ProductPrice, PriceGoldReceipt, products[i].UnitAmount, Vat);
                                listproduct.Add(product);
                            }
                        }
                        else if (products[i].ProductGroupID == 1)
                        {
                            product.No = no.ToString();
                            product = SetProductPrice.ProductNormal(no.ToString(), ProductPrice, products[i].Unit);
                            product.ProductName = products[i].ProductName;
                            listproduct.Add(product);
                            // รวมเงิน
                            product = new ProductSelect();
                            product = SetProductPrice.Product_TotalPrice(ProductPrice);
                            listproduct.Add(product);


                            //// ภาษีมูลค่าเพิ่ม
                            //product = new ProductSelect();
                            //product = SetProductPrice.Product_TotalVat(ProductPrice, PriceGoldReceipt, products[i].UnitAmount, Vat);
                            //listproduct.Add(product);

                            //// รวมเงินทั้งสิ้น
                            //product = new ProductSelect();
                            //product = SetProductPrice.Product_TotalSale(ProductPrice, PriceGoldReceipt, products[i].UnitAmount, Vat);
                            //listproduct.Add(product);

                        }
                        else {
                            //คำนวนราคาจากสัญญา
                            ProductPrice += (products[i].ProductPrice + Reward + Math.Round((products[i].ProductPrice * interest), 2)) * products[i].Unit;


                            //สินค้าอื่นๆ
                            product = SetProductPrice.ProductNormal(no.ToString(), ProductPrice ,products[i].Unit);
                            product.ProductName = products[i].ProductName;
                            listproduct.Add(product);


                            // ภาษีมูลค่าเพิ่ม
                            product = new ProductSelect();
                            product = SetProductPrice.ProductNormal_TotalVat(ProductPrice,  products[i].UnitAmount, Vat);
                            listproduct.Add(product);


                            // รวมเงินทั้งสิ้น
                            product = new ProductSelect();
                            product = SetProductPrice.ProductNormal_TotalSale(ref ContractPayment, ProductPrice,  products[i].UnitAmount, Vat);
                            listproduct.Add(product);
                        
                        }

                    }

                }



                return listproduct;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }

        }

        public void AddProduct(Products item)
        {


            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);
            item.ProductID = Utility.GetMaxID("products", "ProductID");

            if (item.ProductGroupID == 1 | item.ProductGroupID == 2)
            item.ProductCode = "GOLD" + 1000 + item.ProductID;
            else
                item.ProductCode = "PP" + 1000 + item.ProductID;

            try
            {
                string StrSql = @" insert into products (
              ProductID,
             ProductGroupID,
             ProductCode,
             ProductName,
             UnitAmount,
             UnitName,
             ProductPrice,
             Activated,
             Deleted)VALUES("
            + item.ProductID + ","
            + item.ProductGroupID + ","
            + Utility.ReplaceString(item.ProductCode) + ","
            + Utility.ReplaceString(item.ProductName) + ","
            + item.UnitAmount + ","
            + Utility.ReplaceString(item.UnitName) + ","
            + item.ProductPrice + ","
            + 1 + ","
            + 0 + ")";

                DBHelper.Execute(StrSql, ObjConn);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                ObjConn.Close();
            }
        }

        public void UpdateProduct(Products item)
        {

            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);
            try
            {
                string StrSql = @"  UPDATE  products
                SET ProductGroupID = " + item.ProductGroupID + ","
              + " ProductName =" + Utility.ReplaceString(item.ProductName) + ","
              + " UnitAmount =" + item.UnitAmount + ","
              + " UnitName =" + Utility.ReplaceString(item.UnitName) + ","
              + " ProductPrice =" + item.ProductPrice + ","
              + " Activated =" + 1 + ","
              + " Deleted =" + 0
              + " WHERE ProductID =" + item.ProductID;

                DBHelper.Execute(StrSql, ObjConn);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                ObjConn.Close();
            }
        }
        public void DeletedProduct(int ProductID)
        {

            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);
            try
            {
                string StrSql = @"  UPDATE  products
                SET  Deleted =" + 1 +
               " WHERE ProductID =" + ProductID;

                DBHelper.Execute(StrSql, ObjConn);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                ObjConn.Close();
            }
        }

        public void ActivatedProduct(int ProductID)
        {

            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);
            try
            {

                DataTable dt = DBHelper.List("select Activated From products Where ProductID=" + ProductID, ObjConn);
                string StrSql = @"  UPDATE  products  SET  Activated ={0} " +
                            " WHERE ProductID ={1} ";


                if (Convert.ToInt32(dt.Rows[0]["Activated"].ToString()) == 1)
                {
                    StrSql = string.Format(StrSql, 0, ProductID);

                    DBHelper.Execute(StrSql, ObjConn);
                }
                else if (Convert.ToInt32(dt.Rows[0]["Activated"].ToString()) == 0)
                {

                    StrSql = string.Format(StrSql, 1, ProductID);

                    DBHelper.Execute(StrSql, ObjConn);

                }


            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                ObjConn.Close();
            }
        }
        public List<Products> GetListProduct(int proId)
        {

            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);
            try
            {
                string strSql = @"SELECT p.*,pg.ProductGroupName FROM products p 
                 LEFT JOIN  product_group pg ON p.ProductGroupID=pg.ProductGroupID
                 WHERE p.Deleted=0 ";

                if (proId>0)
               strSql+=" AND p.ProductID=" + proId;



                DataTable dt = DBHelper.List(strSql, ObjConn);
                List<Products> list = new List<Products>();
                Products sz = new Products();
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        sz = new Products();
                        sz.ProductID = Convert.ToInt32(dt.Rows[i]["ProductID"]);
                        sz.ProductGroupID = Convert.ToInt32(dt.Rows[i]["ProductGroupID"]);
                        sz.ProductName = dt.Rows[i]["ProductName"].ToString();
                        sz.ProductGroupName = dt.Rows[i]["ProductGroupName"].ToString();
                        sz.UnitAmount = Convert.ToDecimal(dt.Rows[i]["UnitAmount"]);
                        sz.UnitName = dt.Rows[i]["UnitName"].ToString();
                        sz.ProductPrice = Convert.ToDecimal(dt.Rows[i]["ProductPrice"]);
                        sz.Activated = Convert.ToInt32(dt.Rows[i]["Activated"]);
                        list.Add(sz);
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

        public IList<ProductGroup> GetListProductGroup()
        {

            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);
            try
            {
                string strSql = @" SELECT * FROM product_group WHERE Activated=1 ";

               


                DataTable dt = DBHelper.List(strSql, ObjConn);
                IList<ProductGroup> list = new List<ProductGroup>();
                ProductGroup sz = new ProductGroup();

                if (dt != null && dt.Rows.Count > 0)
                {
                    list = ProductGroup.ToObjectList(dt);
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
    }
}
