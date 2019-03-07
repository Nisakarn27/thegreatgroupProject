using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace TheGreatGroupModules.Modules
{
    public class Products
    {
        public int No { get; set; }
        public int ProductID { get; set; }
        public int ProductGroupID { get; set; }
        public string ProductCode { get; set; }
        public string ProductDetail { get; set; }
        public string ProductName { get; set; }
        public string ProductGroupName { get; set; }
        public decimal UnitAmount { get; set; }
        public string UnitAmount_Text
        {
            get
            {
                return UnitAmount.ToString();
        } }
        public string UnitName { get; set; }
        public decimal ProductPrice { get; set; }
        public string ProductPrice_Text
        {
            get
            {
                return ProductPrice.ToString("#,##0.00");
            }
        }
        public int Activated { get; set; }
        public static IList<Products> ToObjectList(DataTable dt)
        {
            return dt.AsEnumerable().Select(dr => new Products()
            {
                ProductID = dr.Field<int>("ProductID"),
                UnitAmount = dr.Field<decimal>("UnitAmount"),
                ProductDetail = dr.Field<string>("ProductGroupName")+" "+dr.Field<string>("ProductName")+
                " (" + dr.Field<decimal>("UnitAmount").ToString() + " " + dr.Field<string>("UnitName") + ")",
                ProductPrice = dr.Field<decimal>("ProductPrice"),
         
            }).ToList();
        }
    }

    public class ProductSelect
    {
        public string No { get; set; }
        public int ProductID { get; set; }
       // public string ProductDetail { get; set; }
        public string ProductName { get; set; }
        public string ContractType { get; set; }
        public int Unit{ get; set; }
        public string Unit_Text { get; set; }
        public double UnitAmount { get; set; }
        public double ContractInterest { get; set; }
        public double ProductPrice { get; set; } // ราคาต่อหน่วย
        public string ProductPrice_Text
        { get; set; } 
        public string TotalPrice { set {
             (Unit * ProductPrice).ToString("#,#00.00");
        }
        }

        public string TotalPrice_Text
        { get; set; } 
        public int ProductGroupID { get; set; }
        public double PriceGold { get; set; }
        public double PriceGoldReceipt { get; set; }


        public double ContractReward { get; set; }
        public string ContractReward_Text
        {
            get
            {
                return ProductPrice.ToString("#,##0.00");

            }
        }
        
    }

    public class ProductGroup {
        public int ProductGroupID { get; set; }
        public string ProductGroupName { get; set; }
        public int Activated { get; set; }
        public static IList<ProductGroup> ToObjectList(DataTable dt)
        {
            return dt.AsEnumerable().Select(dr => new ProductGroup()
            {
                ProductGroupID = dr.Field<int>("ProductGroupID"),
                ProductGroupName = dr.Field<string>("ProductGroupName"),
                Activated = dr.Field<int>("ProductGroupID")

            }).ToList();
        }
    }
}