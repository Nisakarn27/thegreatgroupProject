using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Globalization;
using System.Data;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Text.RegularExpressions;

public static class Extensions
{
    #region ========== Convert ==========
    public static DateTime? ToDateTime(this string sDate)
    {
        try
        {
            // dd/MM/yyyy
            if (sDate.Length == 10)
            {
                var day = int.Parse(sDate.Substring(0, 2));
                var month = int.Parse(sDate.Substring(3, 2));
                var year = int.Parse(sDate.Substring(6, 4));
                return new DateTime(year, month, day);
            }
            else
            {
                //Nitipong support format dd-MM-yyyy HH:mm
                sDate = sDate.Replace('-', '/');
                var splitDate = sDate.Split(' ');
                sDate = splitDate[0] + ' ' + (splitDate[1].Length == 5 ? splitDate[1] : '0' + splitDate[1]);
                return DateTime.ParseExact(sDate, "dd/MM/yyyy HH:mm", new CultureInfo("en-US"));
            }
        }
        catch
        {
            return null;
        }
    }
    public static DateTime? ToDateTime(this string sDate, string format)
    {
        try
        {
            return DateTime.ParseExact(sDate, format, new CultureInfo("en-US"));
        }
        catch
        {
            return null;
        }
    }
    public static DateTime? ConvertStringToDateTime(this string sDate)
    {
        try
        {
            // yyyyMMdd
            var year = int.Parse(sDate.Substring(0, 4));
            var month = int.Parse(sDate.Substring(4, 2));
            var day = int.Parse(sDate.Substring(6, 2));
            return new DateTime(year, month, day);
        }
        catch
        {
            return null;
        }
    }
    public static DateTime? ConvertStringToDateTime2(this string sDate)
    {
        try
        {
            // ddMMyyyy
            var year = int.Parse(sDate.Substring(4, 4));
            var month = int.Parse(sDate.Substring(2, 2));
            var day = int.Parse(sDate.Substring(0, 2));
            return new DateTime(year, month, day);
        }
        catch
        {
            return null;
        }
    }
    public static DateTime? ToPreviousBusinessDay(this DateTime sDate)
    {
        try
        {
            do
            {
                sDate = sDate.AddDays(-1);
            }
            while (sDate.DayOfWeek == DayOfWeek.Saturday || sDate.DayOfWeek == DayOfWeek.Sunday);

            return sDate;
        }
        catch
        {
            return null;
        }
    }
    public static int ToInt(this string str)
    {
        try
        {
            int number;
            if (int.TryParse(str, out number))
                return number;
            return -1;
        }
        catch
        {
            return -1;
        }
    }
    public static int ToInt(this object str)
    {
        try
        {
            if (str.GetType() == typeof(int))
                return (int)str;
            if (str.GetType() == typeof(double))
                return int.Parse(str.ToString());
            if (str.GetType() == typeof(decimal))
                return int.Parse(str.ToString());
            if (str.GetType() == typeof(string) || str.GetType() == typeof(String))
            {
                int number;

                if (int.TryParse((string)str, out number))
                    return number;
                return -1;
            }
            return -1;
        }
        catch
        {
            return -1;
        }
    }
    public static double ToDouble(this object str)
    {
        try
        {
            if (str.GetType() == typeof(double))
                return (double)str;
            if (str.GetType() == typeof(string) || str.GetType() == typeof(String))
            {
                double number;

                if (double.TryParse((string)str, out number))
                    return number;
                return 0;
            }
            return 0;
        }
        catch
        {
            return 0;
        }
    }
    public static decimal? ToDecimal(this object str)
    {
        try
        {
            if (str.GetType() == typeof(decimal))
                return (decimal)str;
            if (str.GetType() == typeof(string) || str.GetType() == typeof(String))
            {
                decimal number;

                if (decimal.TryParse((string)str, out number))
                    return number;
                return 0;
            }
            return 0;
        }
        catch
        {
            return 0;
        }
    }
    public static Boolean ToBoolean(this string str)
    {
        try
        {
            return Boolean.Parse(str.ToString());
        }
        catch
        {
            return true;
        }
    }
    public static string ConvertMonth_TH(this string month)
    {
        string result = string.Empty;
        switch (month)
        {
            case "01":
                result = "มกราคม";
                break;
            case "02":
                result = "กุมภาพันธ์";
                break;
            case "03":
                result = "มีนาคม";
                break;
            case "04":
                result = "เมษายน";
                break;
            case "05":
                result = "พฤษภาคม";
                break;
            case "06":
                result = "มิถุนายน";
                break;
            case "07":
                result = "กรกฏาคม";
                break;
            case "08":
                result = "สิงหาคม";
                break;
            case "09":
                result = "กันยายน";
                break;
            case "10":
                result = "ตุลาคม";
                break;
            case "11":
                result = "พฤศจิกายน";
                break;
            case "12":
                result = "ธันวาคม";
                break;
        }
        return result;
    }
    public static string ConvertShotMonth_TH(this string month)
    {
        string result = string.Empty;
        switch (month)
        {
            case "01":
                result = "ม.ค.";
                break;
            case "02":
                result = "ก.พ.";
                break;
            case "03":
                result = "มี.ค.";
                break;
            case "04":
                result = "เม.ย.";
                break;
            case "05":
                result = "พ.ค.";
                break;
            case "06":
                result = "มิ.ย.";
                break;
            case "07":
                result = "ก.ค.";
                break;
            case "08":
                result = "ส.ค.";
                break;
            case "09":
                result = "ก.ย.";
                break;
            case "10":
                result = "ต.ค.";
                break;
            case "11":
                result = "พ.ย.";
                break;
            case "12":
                result = "ธ.ค.";
                break;
        }
        return result;
    }
    public static string ConvertShotDateToTH(this string sDate)
    {
        string[] array = sDate.Split('/');
        string rlt = string.Empty;
        if (array.Length == 3)
        {
            var day = sDate.Substring(0, 2);
            var month = sDate.Substring(3, 2);
            var year = sDate.Substring(6, 4);
            string month_TH = Convert.ToString(month).ConvertShotMonth_TH();
            rlt = day + " " + month_TH + " " + Convert.ToString(Convert.ToInt32(year) + 543);
        }
        else if (array.Length == 2)
        {
            var month = sDate.Substring(0, 2);
            //var month = sDate.Substring(3, 2);
            var year = sDate.Substring(3, 4);
            string month_TH = Convert.ToString(month).ConvertShotMonth_TH();
            rlt = month_TH + " " + Convert.ToString(Convert.ToInt32(year) + 543);
        }
        else
        {
            rlt = sDate;
        }
        return rlt;
    }
    public static string ConvertDateToTH(this string sDate)
    {
        string[] array = sDate.Split('/');
        string rlt = string.Empty;
        if (array.Length == 3)
        {
            var day = sDate.Substring(0, 2);
            var month = sDate.Substring(3, 2);
            var year = sDate.Substring(6, 4);
            string month_TH = Convert.ToString(month).ConvertMonth_TH();
            rlt = day + " " + month_TH + " " + Convert.ToString(Convert.ToInt32(year) + 543);
        }
        else if (array.Length == 2)
        {
            var month = sDate.Substring(0, 2);
            //var month = sDate.Substring(3, 2);
            var year = sDate.Substring(3, 4);
            string month_TH = Convert.ToString(month).ConvertMonth_TH();
            rlt = month_TH + " " + Convert.ToString(Convert.ToInt32(year) + 543);
        }
        else
        {
            rlt = sDate;
        }
        return rlt;
    }
    public static string ChangeDateFormat(this string sDate)
    {
        var day = sDate.Substring(0, 2);
        var month = sDate.Substring(3, 2);
        var year = sDate.Substring(6, 4);
        return year + "-" + month + "-" + day;
    }
    public static string ChangeDateFormat2(this string sDate)
    {
        try
        {
            var day = sDate.Substring(0, 2);
            var month = sDate.Substring(3, 2);
            var year = sDate.Substring(6, 4);

            return month + "/" + day + "/" + year;
        }
        catch
        {
            return sDate;
        }
    }
    public static string ToStringInvariant(this DateTime dDate)
    {
        try
        {
            return dDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
        }
        catch
        {
            return "";
        }
    }
    public static string SetFileNamePrefix(this string _preFix, DateTime _dateTime, string format = "yyyyMMdd", Boolean flg_search = false)
    {
        if (flg_search == true)
        {
            _preFix = string.Format(_preFix, _dateTime.ToString(format) + "*");
        }
        else
        {
            _preFix = string.Format(_preFix, _dateTime.ToString(format));
        }
        return _preFix;
    }
    public static T ParseEnum<T>(this string value)
    {
        return (T)Enum.Parse(typeof(T), value, true);
    }
    public static string ReplaceRegex(this string _str)
    {
        return System.Text.RegularExpressions.Regex.Replace(_str, @"[',?:\r\n|\n|\r]", string.Empty);
    }
    public static string ReplaceNewLineToWhitSpace(this string str)
    {
        str = str.Trim();
        str = str.Replace("\n", " ");
        str = str.Replace("\r", " ");
        str = str.Replace("\t", " ");
        str = str.Replace(Environment.NewLine, " ");
        str = Regex.Replace(str, @"\s+", " ");
        return str;
    }
    public static string ToStringTxtFile(this int num, int len)
    {
        try
        {
            string numStr = num.ToString();
            numStr = numStr.PadLeft(len, '0');

            return numStr;
        }
        catch
        {
            return string.Empty;
        }
    }
    public static string ToStringTxtFile(this double num, int len)
    {
        try
        {
            string numStr = (num * 100).ToString();
            numStr = numStr.PadLeft(len, '0');

            return numStr;
        }
        catch
        {
            return string.Empty;
        }
    }
    public static string ToStringTxtFile(this string str, int len)
    {
        try
        {
            return str.PadRight(len, ' ');
        }
        catch
        {
            return string.Empty;
        }
    }

    #endregion

    #region ========== Validation ==========
    public static Boolean ChkDateTime(this string inputdate, string format)
    {
        DateTime DateResult;
        return DateTime.TryParseExact(inputdate, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateResult);
    }
    public static Boolean ChkDateDiff(this DateTime DateFrom, DateTime DateTo, int NumMonthLessthan)
    {
        DateTime date1 = DateFrom;
        DateTime date2 = DateTo.AddDays(1);

        int oldMonth = date2.Month;
        while (oldMonth == date2.Month)
        {
            date1 = date1.AddDays(-1);
            date2 = date2.AddDays(-1);
        }

        int years = 0, months = 0, days = 0;

        // getting number of years
        while (date2.CompareTo(date1) >= 0)
        {
            years++;
            date2 = date2.AddYears(-1);
        }
        date2 = date2.AddYears(1);
        years--;


        // getting number of months and days
        oldMonth = date2.Month;
        while (date2.CompareTo(date1) >= 0)
        {
            days++;
            date2 = date2.AddDays(-1);
            if ((date2.CompareTo(date1) >= 0) && (oldMonth != date2.Month))
            {
                months++;
                days = 0;
                oldMonth = date2.Month;
            }
        }
        date2 = date2.AddDays(1);
        days--;

        if (months > NumMonthLessthan)
            return false;

        if (months == NumMonthLessthan && days > 0)
            return false;

        return true;
    }
    public static string EnumDescription(this Enum value)
    {
        try
        {
            // variables  
            var enumType = value.GetType();
            var field = enumType.GetField(value.ToString());
            var attributes = field.GetCustomAttributes(typeof(DescriptionAttribute), false);

            // return  
            return attributes.Length == 0 ? value.ToString() : ((DescriptionAttribute)attributes[0]).Description;
        }
        catch
        {
            return value.ToString();
        }
    }
    #endregion

    #region ========== JSON Helper ==========
    public static string ToJSON(this object obj)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        return serializer.Serialize(obj);
    }

    public static string ToJSON(this object obj, int recursionDepth)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        serializer.RecursionLimit = recursionDepth;
        return serializer.Serialize(obj);
    }

    public static List<T> FromJSONList<T>(this string s)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        return serializer.Deserialize<List<T>>(s);
    }

    public static T FromJSON<T>(this string s)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        return serializer.Deserialize<T>(s);
    }
    #endregion

    #region ========== UI Helper ==========
    public static void WithDefalut(this System.Web.UI.WebControls.DropDownList ddl)
    {
        if (ddl != null)
            ddl.Items.Insert(0, new System.Web.UI.WebControls.ListItem() { Text = "----- Select All -----", Value = string.Empty });
    }

    public static void WithDefalut(this System.Web.UI.WebControls.DropDownList ddl, string text)
    {
        if (ddl != null)
            ddl.Items.Insert(0, new System.Web.UI.WebControls.ListItem() { Text = text, Value = string.Empty });
    }
    public static void WithDefalut(this System.Web.UI.WebControls.DropDownList ddl, string text, string value)
    {
        if (ddl != null)
            ddl.Items.Insert(0, new System.Web.UI.WebControls.ListItem() { Text = text, Value = value });
    }
    public static void WithItemHours(this System.Web.UI.WebControls.DropDownList ddl)
    {
        if (ddl != null)
        {
            for (int i = 0; i < 24; i++)
            {
                ddl.Items.Add(new System.Web.UI.WebControls.ListItem() { Text = i.ToString("0#"), Value = i.ToString("0#") });
            }
            ddl.Items.Insert(0, new System.Web.UI.WebControls.ListItem() { Text = "HH", Value = string.Empty });
        }
    }
    public static void WithItemMinutes(this System.Web.UI.WebControls.DropDownList ddl)
    {
        if (ddl != null)
        {
            for (int i = 0; i < 60; i++)
            {
                ddl.Items.Add(new System.Web.UI.WebControls.ListItem() { Text = i.ToString("0#"), Value = i.ToString("0#") });
            }
            ddl.Items.Insert(0, new System.Web.UI.WebControls.ListItem() { Text = "MM", Value = string.Empty });
        }
    }
    #endregion

    #region ========== Utilities Helper ==========
    public static string CleanBankAccocuntNo(this object acctNo)
    {
        string cleanAcctNo;
        cleanAcctNo = acctNo.ToString().Trim().Replace("-", "");
        return cleanAcctNo;
    }

    public static List<T> DataTableToList<T>(this DataTable table) where T : class, new()
    {
        try
        {
            List<T> list = new List<T>();

            foreach (var row in table.AsEnumerable())
            {
                T obj = new T();

                foreach (var prop in obj.GetType().GetProperties())
                {
                    try
                    {
                        PropertyInfo propertyInfo = obj.GetType().GetProperty(prop.Name);
                        //propertyInfo.SetValue(obj, Convert.ChangeType(row[prop.Name], propertyInfo.PropertyType), null);
                        propertyInfo.SetValue(obj, row[prop.Name], null);
                    }
                    catch
                    {
                        continue;
                    }
                }

                list.Add(obj);
            }
            return list;
        }
        catch
        {
            return null;
        }
    }
    public static IList<T> DataTableToIList<T>(this DataTable table) where T : class, new()
    {
        try
        {
            IList<T> list = new List<T>();

            foreach (var row in table.AsEnumerable())
            {
                T obj = new T();

                foreach (var prop in obj.GetType().GetProperties())
                {
                    try
                    {
                        var propertyInfo = obj.GetType().GetProperty(prop.Name);
                        propertyInfo.SetValue(obj, row[prop.Name], null);
                    }
                    catch
                    {
                        continue;
                    }
                }

                list.Add(obj);
            }
            return list;
        }
        catch
        {
            return null;
        }
    }
    public static DataTable IListToDataTable<T>(this IList<T> items)
    {
        try
        {
            var properties = TypeDescriptor.GetProperties(typeof(T));
            var table = new DataTable();

            foreach (PropertyDescriptor prop in properties)
            {
                if (prop.Description.ToBoolean())
                {
                    table.Columns.Add(prop.DisplayName, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                }
            }

            foreach (T item in items)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                {
                    if (prop.Description.ToBoolean())
                    {
                        row[prop.DisplayName] = prop.GetValue(item) ?? DBNull.Value;
                    }
                }
                table.Rows.Add(row);
            }
            return table;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// For serializing xml object to string 
    /// </summary>
    /// <param name="toSerialize"></param>
    /// <param name="RootName">setting xml root name.</param>
    /// <returns></returns>
    public static string SerializeObject(this object toSerialize, string RootName = "")
    {
        System.Xml.Serialization.XmlSerializer xmlSerializer;
        if (string.IsNullOrEmpty(RootName))
            xmlSerializer = new System.Xml.Serialization.XmlSerializer(toSerialize.GetType());
        else
            xmlSerializer = new System.Xml.Serialization.XmlSerializer(toSerialize.GetType(), new System.Xml.Serialization.XmlRootAttribute(RootName));

        using (System.IO.StringWriter textWriter = new System.IO.StringWriter())
        {
            xmlSerializer.Serialize(textWriter, toSerialize);
            return textWriter.ToString();
        }
    }

    public static T DeepCopy<T>(this T item)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        MemoryStream stream = new MemoryStream();
        formatter.Serialize(stream, item);
        stream.Seek(0, SeekOrigin.Begin);
        T result = (T)formatter.Deserialize(stream);
        stream.Close();
        return result;
    }

    public static List<ListItem> GetSelectedItems(this ListControl lst)
    {
        return lst.Items.OfType<ListItem>().Where(i => i.Selected).ToList();
    }

    #endregion

}
