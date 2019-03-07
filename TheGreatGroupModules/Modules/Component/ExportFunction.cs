using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace TheGreatGroupModules.Modules.Component
{
    public class ExportFunction
    {
        #region ----- Export PDF ------
        // set parameter for export to CrystalReports PDF 
        public ExportParameter.PDF SetReportParameter(string filename, string pathCrystalReport, IEnumerable<object> listObject, Dictionary<string, object> parameter)
        {
            try
            {
                if (string.IsNullOrEmpty(filename))
                {
                    throw new Exception("Filename to export is not null or empty.");
                }

                if (string.IsNullOrEmpty(pathCrystalReport))
                {
                    throw new Exception("Path CrystalReport is not null or empty.");
                }

                if (listObject == null)
                {
                    throw new Exception("Data to export is not null.");
                }

                var reportParameter = new ExportParameter.PDF();

                IList<ExportParameter.Parameter> parameters = new List<ExportParameter.Parameter>();
                foreach (var p in parameter)
                {
                    parameters.Add(new ExportParameter.Parameter()
                    {
                        Name = p.Key,
                        Value = p.Value
                    });
                }

                reportParameter.FileName = filename;
                reportParameter.PathCrystalReport = pathCrystalReport;
                reportParameter.DataSource = listObject;
                reportParameter.Parameter = parameters;

                return reportParameter;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        // Export to CrystalReports PDF 
        public void ExportToPDF(ExportParameter.PDF ReportParameter)
        {
            var report = new ReportDocument();

            try
            {
                if (ReportParameter == null)
                {
                    throw new Exception("parameter to export is not null.");
                }

                HttpContext.Current.Response.Buffer = false;
                HttpContext.Current.Response.ClearContent();
                HttpContext.Current.Response.ClearHeaders();

                report.Load(HttpContext.Current.Server.MapPath(ReportParameter.PathCrystalReport));
                report.SetDataSource(ReportParameter.DataSource);

                foreach (var parameter in ReportParameter.Parameter)
                {
                    report.SetParameterValue(parameter.Name, parameter.Value);
                }

                report.ExportToHttpResponse(ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, ReportParameter.FileName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                report.Close();
                report.Dispose();
                GC.Collect();
            }
        }
        #endregion

        #region ----- Export Txtfile ------
        // set parameter for export to txtfile
        public ExportParameter.TxtFile SetReportParameter(string filename, IList<string> listData)
        {
            try
            {
                if (string.IsNullOrEmpty(filename))
                {
                    throw new Exception("Filename to export is not null or empty.");
                }

                if (listData == null || listData.Count == 0)
                {
                    throw new Exception("Data to export is not null or empty.");
                }

                var parameter = new ExportParameter.TxtFile()
                {
                    FileName = filename,
                    ListData = listData
                };

                return parameter;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        // Export to txtfile
        public void ExportToTxtfile(ExportParameter.TxtFile parameter)
        {
            try
            {
                if (parameter == null)
                {
                    throw new Exception("parameter to export is not null.");
                }

                string filename = parameter.FileName;

                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.ClearContent();
                HttpContext.Current.Response.ClearHeaders();
                HttpContext.Current.Response.Buffer = true;
                HttpContext.Current.Response.ContentType = "text/plain";
                HttpContext.Current.Response.Charset = "windows-874"; // ระบบปลายทางต้องการ format UNIX ANSI จึงต้องใช้ windows-874
                HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding(874);
                HttpContext.Current.Response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}", filename));

                using (StreamWriter sw = new StreamWriter(HttpContext.Current.Response.OutputStream, Encoding.Default))
                {
                    foreach (string txt in parameter.ListData)
                    {
                        sw.WriteLine(txt);
                    }
                }

                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region ----- Export CSV ------
        // set parameter for export to csv
        public ExportParameter.CSV SetReportParameter(string filename, PropertyDescriptorCollection headerExport, IEnumerable<object> BodyExport)
        {
            try
            {
                if (string.IsNullOrEmpty(filename))
                {
                    throw new Exception("Filename to export is not null or empty.");
                }

                if (headerExport == null)
                {
                    throw new Exception("Data invalid.");
                }

                var parameter = new ExportParameter.CSV()
                {
                    FileName = filename,
                    HeaderExport = headerExport,
                    BodyExport = BodyExport
                };

                return parameter;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        // Export to csv
        public void ExportToCSV(ExportParameter.CSV parameter)
        {
            try
            {
                if (parameter == null)
                {
                    throw new Exception("parameter to export is not null.");
                }

                string filename = parameter.FileName;

                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.ClearContent();
                HttpContext.Current.Response.ClearHeaders();
                HttpContext.Current.Response.Buffer = true;
                HttpContext.Current.Response.ContentType = "text/plain";
                HttpContext.Current.Response.Charset = "windows-874"; // ระบบปลายทางต้องการ format UNIX ANSI จึงต้องใช้ windows-874
                HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding(874);
                HttpContext.Current.Response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}", filename));

                IList<string> contents = GetContentCSV(parameter);
                using (StreamWriter sw = new StreamWriter(HttpContext.Current.Response.OutputStream, Encoding.Default))
                {
                    foreach (string content in contents)
                    {
                        sw.WriteLine(content);
                    }
                }

                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private IList<string> GetContentCSV(ExportParameter.CSV parameter)
        {
            IList<string> list = new List<string>();

            // header  
            string _header = string.Empty;
            foreach (PropertyDescriptor header in parameter.HeaderExport)
            {
                _header += header.DisplayName + ",";
            }
            list.Add(_header);

            // body    
            foreach (var item in parameter.BodyExport)
            {
                string _body = string.Empty;
                foreach (PropertyDescriptor header in parameter.HeaderExport)
                {
                    var _value = header.GetValue(item) ?? DBNull.Value;
                    _body += MakeValueCSV(_value) + ",";
                }
                list.Add(_body);
            }

            return list;
        }
        protected static string MakeValueCSV(object value)
        {
            if (value == null)
                return "";

            if (value is INullable && ((INullable)value).IsNull)
                return "";

            if (value is DateTime)
            {
                if (((DateTime)value).TimeOfDay.TotalSeconds == 0)
                    return ((DateTime)value).ToString("yyyy-MM-dd");

                return ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss");
            }

            if (value is decimal || value is double || value is float)
            {
                return value.ToDouble().ToString("n2");
            }

            string output = value.ToString();
            if (output.Contains(',') || output.Contains("\""))
            {
                output = '"' + output.Replace("\"", "\"\"") + '"';
            }
            if (Regex.IsMatch(output, @"(?:\r\n|\n|\r)"))
            {
                output = string.Join(" ", Regex.Split(output, @"(?:\r\n|\n|\r)"));
            }
            return output;
        }
        #endregion

        #region ----- Export Excel ------
        // set parameter for export to Excel
        public ExportParameter.Excel SetReportParameterExcel(string filename, PropertyDescriptorCollection headerExport, IEnumerable<object> BodyExport)
        {
            try
            {
                if (string.IsNullOrEmpty(filename))
                {
                    throw new Exception("Filename to export is not null or empty.");
                }

                if (headerExport == null)
                {
                    throw new Exception("Data invalid.");
                }

                var parameter = new ExportParameter.Excel()
                {
                    FileName = filename,
                    HeaderExport = headerExport,
                    BodyExport = BodyExport
                };

                return parameter;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        // Export to Excel
        public void ExportToExcel(ExportParameter.Excel parameter)
        {
            try
            {
                if (parameter == null)
                {
                    throw new Exception("parameter to export is not null.");
                }

                string filename = parameter.FileName;
                string html = GetContentExcel(parameter);

                System.Web.HttpContext.Current.Response.Clear();
                System.Web.HttpContext.Current.Response.Buffer = true;
                System.Web.HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
                System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", String.Format("attachment; filename=\"{0}\"", filename));
                System.Web.HttpContext.Current.Response.Write(html);
                System.Web.HttpContext.Current.Response.Flush();
                System.Web.HttpContext.Current.Response.End();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private string GetContentExcel(ExportParameter.Excel parameter)
        {
            string html =
            @"<head>
                <meta http-equiv=Content-Type content='text/html; charset=utf-8'>
                <style> table .txtStyle { mso-number-format:'\@' } </style>
            </head>
            <body>
                <font style='font-size:10.0pt; font-family:tahoma;'>
                    <br><br><br>
                    <table border='1' bgColor='#ffffff' borderColor='#000000' cellSpacing='0' cellPadding='0' style='font-size:10.0pt; font-family:tahoma; background:white;'>
                        {@header}
                        {@detail}
                    </table>
                </font>
            </body>";

            string headTable = string.Empty;
            string detailTable = string.Empty;

            // header 
            headTable += "<tr>";
            foreach (PropertyDescriptor header in parameter.HeaderExport)
            {
                headTable += string.Format("<td><b>{0}</b></td>", header.DisplayName);
            }
            headTable += "</tr>";

            // body    
            foreach (var item in parameter.BodyExport)
            {
                detailTable += "<tr>";
                foreach (PropertyDescriptor header in parameter.HeaderExport)
                {
                    var _value = header.GetValue(item) ?? DBNull.Value;
                    detailTable += string.Format("<td class='txtStyle'>{0}</td>", MakeValueExcel(_value));
                }
                detailTable += "</tr>";
            }

            html = html
                .Replace("{@header}", headTable)
                .Replace("{@detail}", detailTable);

            return html;
        }
        protected static string MakeValueExcel(object value)
        {
            if (value == null)
                return "";

            if (value is INullable && ((INullable)value).IsNull)
                return "";

            if (value is DateTime)
            {
                if (((DateTime)value).TimeOfDay.TotalSeconds == 0)
                    return ((DateTime)value).ToString("yyyy-MM-dd");

                return ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss");
            }

            if (value is decimal || value is double || value is float)
            {
                return value.ToDouble().ToString("n2");
            }

            string output = value.ToString();
            if (output.Contains(',') || output.Contains("\""))
            {
                output = '"' + output.Replace("\"", "\"\"") + '"';
            }
            if (Regex.IsMatch(output, @"(?:\r\n|\n|\r)"))
            {
                output = string.Join(" ", Regex.Split(output, @"(?:\r\n|\n|\r)"));
            }
            return output;
        }
        #endregion
    }
}