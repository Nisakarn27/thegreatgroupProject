using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TheGreatGroupModules.Models;
using TheGreatGroupModules.Modules;

namespace TheGreatGroupModules.Report
{
    public partial class ReportPage1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
             int staffID = Convert.ToInt32((Request.Params["staffID"]==null ? Request.Params["staffID"] :"0"));
         
              string dateAsOf = Request.Params["date"];

            IList<DailyReceiptsReport> listData = new List<DailyReceiptsReport>();
            ReportData data = new ReportData();
          
          
             staffID = Convert.ToInt32(Request.Params["staffID"].ToString());
           

            SettingData sd = new SettingData();
            StaffZone sz = sd.GetStaffZone(staffID);


            List<DailyReceiptsReport> listData2 = new List<DailyReceiptsReport>();
            listData2 = data.GetDailyReceiptsReport(staffID, dateAsOf);


            var param = new ReportParameter[4];
            param[0] = new ReportParameter("DateAsOf", dateAsOf);
            param[1] = new ReportParameter("CountCustomer", listData2.Count.ToString());
            param[2] = new ReportParameter("SaleName", sz.StaffName);
            param[3] = new ReportParameter("ZoneName", sz.ZoneName);

            string sourceViewReport = @"\Report\DailyReport.rdlc";
            ReportViewer ReportViewer1 = new ReportViewer();
            ReportViewer1.LocalReport.ReportPath = Path.GetDirectoryName(HttpContext.Current.Server.MapPath("~/")) + sourceViewReport;

            List<DailyReceiptsReport> listData1 = new List<DailyReceiptsReport>();
            listData1 = listData2.Where(x => Convert.ToInt32(x.Status) <= 0).ToList();

            ReportViewer1.LocalReport.DataSources.Clear();

            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource() { Name = "ClosedAccReport", Value = listData1 });
            List<DailyReceiptsReport> listData3 = new List<DailyReceiptsReport>();
            listData3 = listData2.Where(x => Convert.ToInt32(x.Status) > 0).ToList();
            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource() { Name = "DsCustomer", Value = listData3 });


            ReportViewer1.LocalReport.SetParameters(param);

         
        

            string fileType = ".pdf";
            Warning[] warnings = null;
            string[] streamIds = null;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            string PDF_FOLDER_FILE = "../PDF/";
            string PDF_FILE_NAME = "DailyReport";
            
            byte[] bytes = ReportViewer1.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);

            string PDF_File = Convert.ToString((Convert.ToString(PDF_FOLDER_FILE + Convert.ToString("/"))
                + PDF_FILE_NAME) +  "_" ) + fileType;

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