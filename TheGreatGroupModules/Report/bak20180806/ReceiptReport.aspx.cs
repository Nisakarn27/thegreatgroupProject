using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
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
    public partial class ReceiptReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int CustomerID = Convert.ToInt32((Request.Params["CustomerID"] != null ? Request.Params["CustomerID"] : "0")); ;
            int ContractID = Convert.ToInt32((Request.Params["ContractID"] != null ? Request.Params["ContractID"] : "0")); ;

            // Get  Contact
            IList<Contract> listContract = new List<Contract>();
            ContractData cd = new ContractData();
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
            string productName = prod.ProductName;


            string amountUnit = listProductsSelect1[0].UnitAmount.ToString("#,##0.00");
            string unit = listProductsSelect1[0].Unit_Text.Replace(".00", "").ToString();
            string totalprice = listProductsSelect1[0].TotalPrice_Text;
            string price1 = listProductsSelect1[2].ProductName;  // หักประกาศรับซื้อทอง
            string price2 = listProductsSelect1[3].ProductName; // ส่วนต่างที่คิดภาษีมูลค่าเพิ่ม
            string price3 = listProductsSelect1[4].TotalPrice_Text;// รวมเงิน
            string price4 = listProductsSelect1[5].TotalPrice_Text;// ภาษีมูลค่าเพิ่ม
            string price5 = listProductsSelect1[6].TotalPrice_Text;// รวมเงินทั้งสิ้น


            var param = new ReportParameter[30];
            param[0] = new ReportParameter("paramDate", contract.ContractCreateDate.Day + "");
            param[1] = new ReportParameter("paramMonth", contract.ContractCreateDate.ToString("MMMM", CultureInfo.GetCultureInfo("th-TH")));
            param[2] = new ReportParameter("paramYear", contract.ContractCreateDate.ToString("yyyy", CultureInfo.GetCultureInfo("th-TH")));
            param[3] = new ReportParameter("paramCustomerName", Customer.CustomerName);
            param[4] = new ReportParameter("paramAddress", Customer.CustomerAddress1);
            param[5] = new ReportParameter("paramCustomerIDCard", Convert.ToInt64(Customer.CustomerIdCard).ToString("#-####-#####-##-#"));
            param[6] = new ReportParameter("paramStartDate", contract.ContractStartDate.ToString("d  MMMM  yyyy", CultureInfo.GetCultureInfo("th-TH")));
            param[7] = new ReportParameter("paramPeriod", contract.ContractPeriod.ToString());
            param[8] = new ReportParameter("paramProducts", productName);
            param[9] = new ReportParameter("paramAmount", contract.ContractAmount.ToString());
            param[10] = new ReportParameter("paramContractSurety1", contract.CustomerSuretyData1 != null ? contract.CustomerSuretyData1.CustomerSuretyName : "");
            param[11] = new ReportParameter("paramContractSurety2", contract.CustomerSuretyData2 != null ? contract.CustomerSuretyData2.CustomerSuretyName : "");
            param[12] = new ReportParameter("paramContractPartner", contract.CustomerPartnerData != null ? contract.CustomerPartnerData.CustomerPartnerName : "");
            param[13] = new ReportParameter("paramContractCreateDate", contract.ContractCreateDate.ToString("d  MMMM  yyyy", CultureInfo.GetCultureInfo("th-TH")));
            param[14] = new ReportParameter("paramContractSurety", (contract.CustomerSuretyData1 != null ? contract.CustomerSuretyData1.CustomerSuretyName : "") + (contract.CustomerSuretyData2 != null && !string.IsNullOrEmpty(contract.CustomerSuretyData2.CustomerSuretyName) ? " และ " + contract.CustomerSuretyData2.CustomerSuretyName : ""));
            param[15] = new ReportParameter("paramContractNumber", contract.ContractNumber);
            param[16] = new ReportParameter("paramContractSuretyIDCard", contract.CustomerSuretyData1 != null ? Convert.ToInt64(contract.CustomerSuretyData1.CustomerSuretyIdCard).ToString("#-####-#####-##-#") : "");
            param[17] = new ReportParameter("paramContractSuretyAddress1", contract.CustomerSuretyData1 != null ? contract.CustomerSuretyData1.CustomerSuretyAddress1 : "");
            param[18] = new ReportParameter("paramContractExpireDate", contract.ContractExpDate.ToString("dd/MM/yyyy", CultureInfo.GetCultureInfo("th-TH")));
            param[19] = new ReportParameter("paramProductUnit", unit);
            param[20] = new ReportParameter("paramProductUnitAmount", totalprice);
            param[21] = new ReportParameter("paramProductTotalAmount", totalprice);
            param[22] = new ReportParameter("paramContractReward", contract.ContractReward.ToString("#,##0.00"));
            param[23] = new ReportParameter("paramPrice1", price1);
            param[24] = new ReportParameter("paramPrice2", price2);
            param[25] = new ReportParameter("paramPrice3", price3);
            param[26] = new ReportParameter("paramPrice4", price4);
            param[27] = new ReportParameter("paramPrice5", price5);
            param[28] = new ReportParameter("paramTotalPriceThai","(  "+ Utility.numConvertChar(contract.ContractPayment.ToString())+"  )" );

            param[29] = new ReportParameter("paramRef", contract.ContractRefNumber);

            string sourceViewReport = @"\Report\Receipt.rdlc";
            ReportViewer ReportViewer1 = new ReportViewer();
            ReportViewer1.LocalReport.ReportPath = Path.GetDirectoryName(HttpContext.Current.Server.MapPath("~/")) + sourceViewReport;
            //ReportDataSource rpt = new ReportDataSource("ClosedAccReport", listData);
            //ReportViewer1.LocalReport.DataSources.Clear();
            //ReportViewer1.LocalReport.DataSources.Add(rpt);

             ReportViewer1.LocalReport.SetParameters(param);
            string fileType = ".pdf";
            Warning[] warnings = null;
            string[] streamIds = null;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            string PDF_FOLDER_FILE = "../PDF/";
            string PDF_FILE_NAME = "ReceiptReport";

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