using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheGreatGroupModules.Modules
{
    public class SetProductPrice
    {

        // สินค้า
   
        public static ProductSelect ProductNormal(string No, double ProductPrice, int Unit)
        {

            ProductSelect product = new ProductSelect();
            product.No = No;
            product.ProductPrice = ProductPrice;
            product.ProductPrice_Text = ProductPrice.ToString("#,##0.00");
            product.TotalPrice_Text = ProductPrice.ToString("#,##0.00");
            product.Unit_Text = Unit == 0 ? "" : Unit.ToString("#,##0.00");
            return product;
        }
        // หักประกาศรับซื้อทองประจำวัน 
        public static ProductSelect Product_ReceiptGoldDay(double PriceGoldReceipt, double UnitAmount)
        {
             double TotalPrice =  ((PriceGoldReceipt -(1.8 / 100))/15.20) * UnitAmount;
               ProductSelect product = new ProductSelect();
               product.ProductName = "หักประกาศรับซื้อทองประจำวัน  " + TotalPrice.ToString("#,##0.00") + " บาท ";
               product.Unit_Text = "";
            return product;
        }

      //  ส่วนต่างที่คิดภาษีมูลค่าเพิ่ม

        public static ProductSelect Product_VatDiff(ref double TotalVat,double TotalPrice, double PriceGoldReceipt, double UnitAmount)
        {
            double ReceiptGoldDay = ((PriceGoldReceipt - (1.8 / 100)) / 15.20) * UnitAmount; // ราคารับซื้อทองประจำวัน
            double TotalPrice3 = TotalPrice - ReceiptGoldDay; // ราคาสินค้ารวมค่ากำเหน็จ รวมดอกเบี้ยแล้ว
            TotalVat += TotalPrice3;
            ProductSelect product = new ProductSelect();
            product.ProductName = "ส่วนต่างที่คิดภาษีมูลค่าเพิ่ม  " + TotalPrice3.ToString("#,##0.00") + " บาท ";
            product.Unit_Text = "";
            return product;
        }

        //  รวมเงิน
        public static ProductSelect Product_TotalPrice(double TotalPrice)
        {
            ProductSelect product = new ProductSelect();
            product.ProductPrice_Text = "รวมเงิน  ";
            product.TotalPrice = TotalPrice.ToString("#,##0.00");
            product.TotalPrice_Text = TotalPrice.ToString("#,##0.00");
            product.Unit_Text = "";
            return product;
        }

        public static ProductSelect Product_TotalVat(double TotalPrice, double PriceGoldReceipt, double UnitAmount, double Vat)
        {
            double ReceiptGoldDay = ((PriceGoldReceipt - (1.8 / 100)) / 15.20) * UnitAmount; // ราคารับซื้อทองประจำวัน
            double TotalPrice3 = TotalPrice - ReceiptGoldDay; // ราคาสินค้ารวมค่ากำเหน็จ รวมดอกเบี้ยแล้ว
            double totalvat = TotalPrice3 * Vat;
            ProductSelect product = new ProductSelect();
            product.ProductPrice_Text = "ภาษีมูลค่าเพิ่ม  ";
            product.TotalPrice = totalvat.ToString("#,##0.00");
            product.TotalPrice_Text = totalvat.ToString("#,##0.00");
            product.Unit_Text = "";
            return product;
        }


        public static ProductSelect Product_TotalSale(ref double ContractPayment,double TotalPrice, double PriceGoldReceipt, double UnitAmount, double Vat)
        {
            double ReceiptGoldDay = ((PriceGoldReceipt - (1.8 / 100)) / 15.20) * UnitAmount; // ราคารับซื้อทองประจำวัน
            double TotalPrice3 = TotalPrice - ReceiptGoldDay; // ราคาสินค้ารวมค่ากำเหน็จ รวมดอกเบี้ยแล้ว
            double totalvat = TotalPrice3 * Vat;
            double TotalSale = TotalPrice + totalvat;
            ContractPayment = TotalSale;
            ProductSelect product = new ProductSelect();
            product.ProductPrice_Text = "รวมเงินทั้งสิ้น  ";
            product.TotalPrice = TotalSale.ToString("#,##0.00");
            product.TotalPrice_Text = TotalSale.ToString("#,##0.00");
            product.Unit_Text = "";
         
            return product;
        }


        public static ProductSelect ProductNormal_TotalVat(double TotalPrice,double UnitAmount, double Vat)
        {
            double totalvat = TotalPrice * UnitAmount * Vat;
            ProductSelect product = new ProductSelect();
            product.ProductPrice_Text = "ภาษีมูลค่าเพิ่ม  ";
            product.TotalPrice = totalvat.ToString("#,##0.00");
            product.TotalPrice_Text = totalvat.ToString("#,##0.00");
            product.Unit_Text = "";
            return product;
        }
        public static ProductSelect ProductNormal_TotalSale(ref double ContractPayment, double TotalPrice, double UnitAmount, double Vat)
        {
            double total = TotalPrice+(TotalPrice * UnitAmount * Vat);
            ContractPayment = total;
            ProductSelect product = new ProductSelect();
            product.ProductPrice_Text = "รวมเงินทั้งสิ้น  ";
            product.TotalPrice = total.ToString("#,##0.00");
            product.TotalPrice_Text = total.ToString("#,##0.00");
            product.Unit_Text = "";
            return product;
        }
        

    }
}