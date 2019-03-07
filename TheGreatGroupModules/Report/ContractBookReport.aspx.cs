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
    public partial class ContractReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int CustomerID = Convert.ToInt32((Request.Params["CustomerID"]!= null ? Request.Params["CustomerID"] : "0")); ;
            int ContractID = Convert.ToInt32((Request.Params["ContractID"]!= null ? Request.Params["ContractID"] : "0")); ;

            // Get  Contact
            IList<Contract> listContract = new List<Contract>();
            ContractData cd = new ContractData();
            listContract = cd.GetContract(CustomerID, ContractID);

            //Get Customer
            IList<Customers> listCustomer = new List<Customers>();
            CustomersData dataCus = new CustomersData();
            listCustomer = dataCus.Get(CustomerID);


            Contract contract=new Contract();
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
            prod= listProductsSelect1.FirstOrDefault();
            string productName = prod.ProductName + "  จำนวน " + prod.Unit_Text.Replace(".00","") + " รายการ";


            var param = new ReportParameter[18];
            param[0] = new ReportParameter("paramDate", contract.ContractCreateDate.Day+"");
            param[1] = new ReportParameter("paramMonth", contract.ContractCreateDate.ToString("MMMM", CultureInfo.GetCultureInfo("th-TH")));
            param[2] = new ReportParameter("paramYear", contract.ContractCreateDate.ToString("yyyy", CultureInfo.GetCultureInfo("th-TH")));
            param[3] = new ReportParameter("paramCustomerName", Customer.CustomerName );
            param[4] = new ReportParameter("paramAddress", Customer.CustomerAddress2);
            param[5] = new ReportParameter("paramCustomerIDCard",Convert.ToInt64(Customer.CustomerIdCard).ToString("#-####-#####-##-#"));
            param[6] = new ReportParameter("paramStartDate", contract.ContractStartDate.ToString("d  MMMM  yyyy", CultureInfo.GetCultureInfo("th-TH")));
            param[7] = new ReportParameter("paramPeriod", contract.ContractPeriod.ToString());
            param[8] = new ReportParameter("paramProducts", productName);
            param[9] = new ReportParameter("paramAmount", contract.ContractAmount.ToString());
            param[10] = new ReportParameter("paramContractSurety1", contract.CustomerSuretyData1 != null ? contract.CustomerSuretyData1.CustomerSuretyName : "");
            param[11] = new ReportParameter("paramContractSurety2", contract.CustomerSuretyData2!= null ?contract.CustomerSuretyData2.CustomerSuretyName:"");
            param[12] = new ReportParameter("paramContractPartner", contract.CustomerPartnerData != null ? contract.CustomerPartnerData.CustomerPartnerName : "");
            param[13] = new ReportParameter("paramContractCreateDate", contract.ContractCreateDate.ToString("d  MMMM  yyyy", CultureInfo.GetCultureInfo("th-TH")));
            if (contract.CustomerSuretyData2 != null)
            {
                param[14] = new ReportParameter("paramContractSurety", (contract.CustomerSuretyData1 != null ? contract.CustomerSuretyData1.CustomerSuretyName : "") + ((string.IsNullOrEmpty(contract.CustomerSuretyData2.CustomerSuretyName.TrimEnd())) ? "" : " และ " + contract.CustomerSuretyData2.CustomerSuretyName));
            }
            else {
                param[14] = new ReportParameter("paramContractSurety", contract.CustomerSuretyData1 != null ? contract.CustomerSuretyData1.CustomerSuretyName : "");
            }
           param[15] = new ReportParameter("paramContractNumber", contract.ContractNumber);
            param[16] = new ReportParameter("paramContractSuretyIDCard", contract.CustomerSuretyData1 != null ? Convert.ToInt64(contract.CustomerSuretyData1.CustomerSuretyIdCard).ToString("#-####-#####-##-#") : "");
            param[17] = new ReportParameter("paramContractSuretyAddress1", contract.CustomerSuretyData1 != null ? contract.CustomerSuretyData1.CustomerSuretyAddress2 : "");


            string sourceViewReport = @"\Report\ContractBookReport.rdlc";
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
            string PDF_FILE_NAME = "ContractBook";

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
         //  Response.Redirect("../Report/PDFContractDocument");
            //System.Diagnostics.Process.Start(Server.MapPath(PDF_File));
       //     Path.GetTempPath();
        }
    }
}