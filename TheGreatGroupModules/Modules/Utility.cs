using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Security;
using TheGreatGroupModules.Models;

namespace TheGreatGroupModules.Modules
{
    public class Utility
    {
        static string errMsg = "";


        public static string CheckAccountStatus(int status) {
            string result = string.Empty;
            switch (status)
            {
                case 1:
                    result = "บัญชีไม่ดี งดเครดิต";
                    break;
                case 2:
                    result = "บัญชีพอใช้";
                    break;
                case 3:
                    result = "บัญชีดีพอใช้";
                    break;
                case 4:
                    result = "บัญชีดี";
                    break;
                case 5:
                    result = "บัญชีดีมาก";
                    break;
                default:
                    result = "ยังไม่กำหนด";
                    break;
            }


            return result;
        }
        public static int GetMaxID(String table_name ,String column_name )
        {
            
            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);

            try
            {

                string StrSql = @" Select Max(" + column_name + ") As ID From " + table_name ;

            
            DataTable dt = DBHelper.List(StrSql, ObjConn);
            if (dt.Rows.Count > 0)
                return dt.Rows[0][0]!=DBNull.Value? Convert.ToInt32(dt.Rows[0][0].ToString()) + 1:1;
            else
                return -99;

            
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



        public static bool IsHolidays(DateTime date, DateTime[] holidays)
        {
            return holidays.Contains(date.Date);

        }


        public static DateTime[] Holidays(int activated)
        {


            MySqlConnection ObjConn = DBHelper.ConnectDb(ref errMsg);
            string StrSql = "";
            try
            {



                StrSql = @" SELECT * FROM holidays WHERE  deleted=0 ";


                if (activated > 0)
                {
                    StrSql += " and activated=1  ";
                }


                DataTable dt = DBHelper.List(StrSql, ObjConn);

                DateTime[] Holidays = new DateTime[dt.Rows.Count]; ;
                if (dt.Rows.Count > 0)
                {

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Holidays[i] = Convert.ToDateTime(dt.Rows[i]["Date"].ToString());

                    }

                }

                return Holidays;

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

        //Convert จำนวนเงิน  Example: Utility.numConvertChar("2321.89");
        public static string numConvertChar(string txt)
        {
            string bahtTxt, n, bahtTH = "";
            double amount;
            try { amount = Convert.ToDouble(txt); }
            catch { amount = 0; }
            bahtTxt = amount.ToString("####.00");
            string[] num = { "ศูนย์", "หนึ่ง", "สอง", "สาม", "สี่", "ห้า", "หก", "เจ็ด", "แปด", "เก้า", "สิบ" };
            string[] rank = { "", "สิบ", "ร้อย", "พัน", "หมื่น", "แสน", "ล้าน" };
            string[] temp = bahtTxt.Split('.');
            string intVal = temp[0];
            string decVal = temp[1];
            if (Convert.ToDouble(bahtTxt) == 0)
                bahtTH = "ศูนย์บาทถ้วน";
            else
            {
                for (int i = 0; i < intVal.Length; i++)
                {
                    n = intVal.Substring(i, 1);
                    if (n != "0")
                    {
                        if ((i == (intVal.Length - 1)) && (n == "1"))
                            bahtTH += "เอ็ด";
                        else if ((i == (intVal.Length - 2)) && (n == "2"))
                            bahtTH += "ยี่";
                        else if ((i == (intVal.Length - 2)) && (n == "1"))
                            bahtTH += "";
                        else
                            bahtTH += num[Convert.ToInt32(n)];
                        bahtTH += rank[(intVal.Length - i) - 1];
                    }
                }
                bahtTH += "บาท";
                if (decVal == "00")
                    bahtTH += "ถ้วน";
                else
                {
                    for (int i = 0; i < decVal.Length; i++)
                    {
                        n = decVal.Substring(i, 1);
                        if (n != "0")
                        {
                            if ((i == decVal.Length - 1) && (n == "1"))
                                bahtTH += "เอ็ด";
                            else if ((i == (decVal.Length - 2)) && (n == "2"))
                                bahtTH += "ยี่";
                            else if ((i == (decVal.Length - 2)) && (n == "1"))
                                bahtTH += "";
                            else
                                bahtTH += num[Convert.ToInt32(n)];
                            bahtTH += rank[(decVal.Length - i) - 1];
                        }
                    }
                    bahtTH += "สตางค์";
                }
            }
            return bahtTH;
        }

        public static String FormateCurrency(Decimal value)
        {
            String data;
            data = String.Format("{0:0,0.00}", value);
            return data;
        }

        public static String FormateDate(DateTime date)
        {
            String strDate;
            CultureInfo invC = System.Globalization.CultureInfo.InvariantCulture;
            strDate = "{ d '" + date.ToString("yyyy-MM-dd", invC) + "' }";
            return strDate;
        }

        public static String FormateDateTime(DateTime date)
        {
            String strDate;
            CultureInfo invC = System.Globalization.CultureInfo.InvariantCulture;
            strDate = "{ ts '" + date.ToString("yyyy-MM-dd HH:mm:ss", invC) + "' }";
            return strDate;
        }

        public static String ReplaceString(String text)
        {
            String data="";
            if (String.IsNullOrEmpty(text))
            {
                text = "";
            }
            data = text.Replace("'", "''");
            data = data.Replace("\\", "\\\\");
            data = data.Replace("'-'", "");
            data = data.Replace("''", "");
            data = data.Replace("'&'", "");
            data = data.Replace("'*'", "");
            data = data.Replace("' or''-'", "");
            data = data.Replace("' or 'x'='x", "");
            data = data.Replace("' or 'x'='x", "");

            return "'"+data+"'";
        }

        public static String HashPassword(String password)
        {
            String strHash = null;
            String passwordFormat = "SHA1";
            strHash = FormsAuthentication.HashPasswordForStoringInConfigFile(password, passwordFormat);
            return strHash;
        }

        public static DateTime ConvertStringDateToDateTime(string strDate)
        {
            string[] strValue = strDate.Split('-');
            DateTime dateTime;
            if (strValue[2].IndexOf(" ") != -1)
            {
                string[] strData = strValue[2].Split(' ');
                string[] strTime = strData[1].Split(':');
                dateTime = new DateTime(Convert.ToInt32(strValue[0]), Convert.ToInt32(strValue[1]), Convert.ToInt32(strValue[2]), Convert.ToInt32(strTime[0]), Convert.ToInt32(strTime[1]), 0);

            }
            else
            {
                dateTime = new DateTime(Convert.ToInt32(strValue[0]), Convert.ToInt32(strValue[1]), Convert.ToInt32(strValue[2]));

            }

            return dateTime;

        }

        public static DateTime ConvertStringDateToDateTime(string strDate, string strTime)
        {
            string[] strValue = strDate.Split('-');
            DateTime dateTime;
            if (strTime != "" || strTime != null)
            {
                string[] strTimeData = strTime.Split(':');
                dateTime = new DateTime(Convert.ToInt32(strValue[0]), Convert.ToInt32(strValue[1]), Convert.ToInt32(strValue[2]), Convert.ToInt32(strTimeData[0]), Convert.ToInt32(strTimeData[1]), 0);

            }
            else
            {
                dateTime = new DateTime(Convert.ToInt32(strValue[0]), Convert.ToInt32(strValue[1]), Convert.ToInt32(strValue[2]));

            }

            return dateTime;

        }
       
        public static bool ExportPDF(string filename, string pathCrystalReport, IEnumerable<object> listObject, Dictionary<string, object> parameters)
        {
            try
            {
            
                var bll = new TheGreatGroupModules.Modules.Component.ExportFunction();
                var parameter = bll.SetReportParameter(
                    filename,
                    pathCrystalReport,
                    listObject,
                    parameters
                );
                bll.ExportToPDF(parameter);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}