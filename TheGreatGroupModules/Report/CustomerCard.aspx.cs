using MessagingToolkit.QRCode.Codec;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TheGreatGroupModules.Models;
using TheGreatGroupModules.Modules;

namespace TheGreatGroupModules.Report
{
    public partial class CustomerCard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int ContractID = Convert.ToInt32(Request.QueryString["ContractID"]);
            int CustomerID = Convert.ToInt32(Request.QueryString["CustomerID"]);
            string code = CustomerID + ":" + ContractID;
            QRCodeEncoder enc = new QRCodeEncoder();
            enc.QRCodeScale = 5;
            Bitmap qrcode = enc.Encode(code);
            ContractData cd = new ContractData();
        
            List<ReportCustomerOnCard> listData = cd.GetPayment_OnCard(ContractID);

            using (MemoryStream ms = new MemoryStream())
            {
                qrcode.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                qrcode.Save(Server.MapPath("~/Content/Barcode1.jpg"), System.Drawing.Imaging.ImageFormat.Png);
            }


            string url=new Uri(Server.MapPath("~/Content/Barcode1.jpg")).AbsoluteUri;


            // Get  Contact
            IList<Contract> listContract = new List<Contract>();
            listContract = cd.GetContract(CustomerID, ContractID);

            //Get Customer
            IList<Customers> listCustomer = new List<Customers>();
            CustomersData dataCus = new CustomersData();
            listCustomer = dataCus.Get(CustomerID);


            Contract contract = new Contract();
            contract = listContract.FirstOrDefault();
            Customers Customer = new Customers();
            Customer = listCustomer.FirstOrDefault();


            ProductData dataPro = new ProductData();
            IList<ProductSelect> listProductsSelect = new List<ProductSelect>();
            List<ProductSelect> listProductsSelect1 = new List<ProductSelect>();
            if (ContractID > 0)
            {
                listProductsSelect = dataPro.GetProductCustomer(CustomerID, ContractID);
            }
            double ContractPayment = 0;
            listProductsSelect1 = dataPro.ProductContractSummary(ref ContractPayment, listProductsSelect);

            ProductSelect prod = new ProductSelect();
            prod = listProductsSelect1.FirstOrDefault();
            string productName = prod.ProductName + "  จำนวน " + prod.Unit_Text.Replace(".00", "") + " รายการ";

            string amountUnit = listProductsSelect1[0].UnitAmount.ToString("#,##0.00");
            string unit = listProductsSelect1[0].Unit_Text.Replace(".00", "").ToString();
            string totalprice = listProductsSelect1[0].TotalPrice_Text;
            string price1 = listProductsSelect1[2].ProductName;  // หักประกาศรับซื้อทอง
            string price2 = listProductsSelect1[3].ProductName; // ส่วนต่างที่คิดภาษีมูลค่าเพิ่ม
            string price3 = listProductsSelect1[4].TotalPrice_Text;// รวมเงิน
            string price4 = listProductsSelect1[5].TotalPrice_Text;// ภาษีมูลค่าเพิ่ม
            string price5 = listProductsSelect1[6].TotalPrice_Text;// รวมเงินทั้งสิ้น

            SettingData sd = new SettingData();
            StaffZone sz = sd.GetStaffZone(Customer.SaleID) ;

            var param = new ReportParameter[25];
            param[0] = new ReportParameter("Path", url);
            param[1] = new ReportParameter("ProductName", productName);
            param[2] = new ReportParameter("ContractExpireDate", contract.ContractExpDate.ToString("d  MMMM  yyyy", CultureInfo.GetCultureInfo("th-TH")));
            param[3] = new ReportParameter("CustomerName", Customer.CustomerName + " (" + Customer.CustomerNickName + ") ");
            param[4] = new ReportParameter("Address", Customer.CustomerAddress2);
            param[5] = new ReportParameter("CustomerIDCard", Convert.ToInt64(Customer.CustomerIdCard).ToString("#-####-#####-##-#"));
            param[6] = new ReportParameter("ContractStartDate", contract.ContractStartDate.ToString("d  MMMM  yyyy", CultureInfo.GetCultureInfo("th-TH")));
            param[7] = new ReportParameter("Period", contract.ContractPeriod.ToString());
            param[8] = new ReportParameter("ContractPayment", contract.ContractPayment.ToString("#,##0.00"));
            param[9] = new ReportParameter("CustomerNickName", Customer.CustomerNickName);
            param[10] = new ReportParameter("ContractCreateDate", contract.ContractCreateDate.ToString("d  MMMM  yyyy", CultureInfo.GetCultureInfo("th-TH")));
            param[11] = new ReportParameter("CustomerMobile", Customer.CustomerMobile);
            param[12] = new ReportParameter("ContractAmount", contract.ContractAmount.ToString("#,##0.00"));
            param[13] = new ReportParameter("ContractAmountLast", contract.ContractAmountLast.ToString("#,##0.00"));
            param[14] = new ReportParameter("SaleName", sz.StaffName);
            param[15] = new ReportParameter("ZoneName", sz.ZoneName);

            param[16] = new ReportParameter("ContractSuretyName", contract.CustomerSuretyData1 != null ? contract.CustomerSuretyData1.CustomerSuretyName : "");
            param[17] = new ReportParameter("ContractSuretyAddress", contract.CustomerSuretyData1 != null ? contract.CustomerSuretyData1.CustomerSuretyAddress2: "");
            param[18] = new ReportParameter("ContractSuretyIDCard", contract.CustomerSuretyData1 != null ? Convert.ToInt64(contract.CustomerSuretyData1.CustomerSuretyIdCard).ToString("#-####-#####-##-#") : "");
            param[19] = new ReportParameter("ContractSuretyMobile", contract.CustomerSuretyData1 != null ? contract.CustomerSuretyData1.CustomerSuretyMobile : "");
            param[20] = new ReportParameter("ContractNumber", contract.ContractNumber);
            param[21] = new ReportParameter("DownMonney", "0.00");
           
            decimal amountLast=Math.Round(contract.ContractPayment%359,2);
            decimal ContractAmount1 = Convert.ToDecimal(Math.Round((contract.ContractPayment-amountLast)/359,2));
            param[22] = new ReportParameter("ContractAmount1", ContractAmount1.ToString("#,##0.00"));
            param[23] = new ReportParameter("ContractAmountLast1", amountLast.ToString("#,##0.00"));
            param[24] = new ReportParameter("Period1", "360");

            string sourceViewReport = @"\Report\CustomerCard.rdlc";

            ReportViewer ReportViewer1 = new Microsoft.Reporting.WebForms.ReportViewer();
            ReportViewer1.ProcessingMode = ProcessingMode.Local;

            // ReportViewer ReportViewer1 = new ReportViewer();
            ReportViewer1.LocalReport.ReportPath = Path.GetDirectoryName(HttpContext.Current.Server.MapPath("~/")) + sourceViewReport;

         
            ReportDataSource rpt = new ReportDataSource("DataSet1", listData);
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(rpt);
        
            ReportViewer1.LocalReport.EnableExternalImages = true;
            ReportViewer1.LocalReport.SetParameters(param);
            ReportViewer1.LocalReport.Refresh();
            string fileType = ".pdf";
            Warning[] warnings = null;
            string[] streamIds = null;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            string PDF_FOLDER_FILE = "../PDF/";
            string PDF_FILE_NAME = "CustomerCard";

            byte[] bytes = ReportViewer1.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);

            string PDF_File = Convert.ToString((Convert.ToString(PDF_FOLDER_FILE + Convert.ToString("/"))
                + PDF_FILE_NAME) + "_") + fileType;

            //########################### Check Folder ######################################

            string folder = Server.MapPath(PDF_FOLDER_FILE);
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            //########################### Check Folder ######################################

            int len = 0;
            using (Stream f = File.Open(Server.MapPath(PDF_File), FileMode.Create))
            {
                if (bytes != null)
                {
                    len = bytes.Length;
                    f.Write(bytes, 0, bytes.Length);
                }
                f.Close();
            }

            Response.Redirect((PDF_File + Convert.ToString("?")) + System.DateTime.Now.ToString());
        }
    }
}