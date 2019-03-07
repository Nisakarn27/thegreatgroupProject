using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheGreatGroupModules.Modules;

namespace TheGreatGroupModules.Models
{
    public class ListCustomerOnMobile
    {
        public int ContractID { get; set; }
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public DateTime ContractStartDate { get; set; }
        public DateTime ContractExpDate { get; set; }
        public string ContractExpDate_Text { get { return ContractExpDate.ToString(@"dd\/MM\/yyyy"); } }
        public DateTime LastDate { get; set; }
        public int ContractPayEveryDay { get; set; }
        public bool ContractSpecialholiday { get; set; }
        public int ContractPeriod { get; set; }
        public decimal ContractAmount { get; set; }
        public decimal ContractPayment { get; set; }
        public decimal TotalPay { get; set; }
        public string CustomerNumber { get; set; }
        public string ContractAmount_Text { get { return ContractAmount.ToString("#,##0.00"); } }
        public string ContractPayment_Text { get { return ContractPayment.ToString("#,##0.00"); } }
        public string TotalPay_Text { get { return TotalPay.ToString("#,##0.00"); } }
        public int StatusPay { get; set; }
        public int IsFirstTime { get; set; }
        public static List<ListCustomerOnMobile> ToObjectList(DataTable dt)
        {
            DateTime[] HolidaysArr = Utility.Holidays(1);
            List<ListCustomerOnMobile> list = new List<ListCustomerOnMobile>();
            ListCustomerOnMobile obj = new ListCustomerOnMobile();
            DateTime lastpay = new DateTime();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                obj = new ListCustomerOnMobile();
                if (dt.Rows[i]["ContractID"] != DBNull.Value)
                    obj.ContractID = Convert.ToInt32(dt.Rows[i]["ContractID"].ToString());
                if (dt.Rows[i]["CustomerId"] != DBNull.Value)
                    obj.CustomerID = Convert.ToInt32(dt.Rows[i]["CustomerId"].ToString());
                if (dt.Rows[i]["CustomerName"] != DBNull.Value)
                    obj.CustomerName = dt.Rows[i]["CustomerName"].ToString();
                if (dt.Rows[i]["ContractNumber"] != DBNull.Value)
                    obj.CustomerNumber = dt.Rows[i]["ContractNumber"].ToString();
                if (dt.Rows[i]["ContractPeriod"] != DBNull.Value)
                    obj.ContractPeriod = Convert.ToInt32(dt.Rows[i]["ContractPeriod"].ToString());
                if (dt.Rows[i]["ContractStartDate"] != DBNull.Value)
                    obj.ContractStartDate = Convert.ToDateTime(dt.Rows[i]["ContractStartDate"].ToString());
                if (dt.Rows[i]["ContractExpDate"] != DBNull.Value)
                    obj.ContractExpDate = Convert.ToDateTime(dt.Rows[i]["ContractExpDate"].ToString());
                if (dt.Rows[i]["ContractAmount"] != DBNull.Value)
                    obj.ContractAmount = Convert.ToDecimal(dt.Rows[i]["ContractAmount"].ToString());
                if (dt.Rows[i]["ContractPayment"] != DBNull.Value)
                    obj.ContractPayment = Convert.ToDecimal(dt.Rows[i]["ContractPayment"].ToString());

                // ทุกวัน =2 / จ-ศ =1
                if (dt.Rows[i]["ContractPayEveryDay"] != DBNull.Value)
                    obj.ContractPayEveryDay = Convert.ToInt32(dt.Rows[i]["ContractPayEveryDay"].ToString());

                // 1 เว้นวันหยุด  / 0 ไม่เว้นไม่หยุด
                if (dt.Rows[i]["ContractSpecialholiday"] != DBNull.Value)
                    obj.ContractSpecialholiday = Convert.ToBoolean(dt.Rows[i]["ContractSpecialholiday"].ToString());


                if (dt.Rows[i]["TotalPay"] != DBNull.Value)
                    obj.TotalPay = Convert.ToDecimal(dt.Rows[i]["TotalPay"].ToString());
                else
                    obj.TotalPay = 0;

              
                    lastpay = dt.Rows[i]["lastDate"] == DBNull.Value ? obj.ContractStartDate : Convert.ToDateTime(dt.Rows[i]["lastDate"].ToString());
                    obj.IsFirstTime = dt.Rows[i]["lastDate"] == DBNull.Value ? 1 : 0 ;


                if (obj.IsFirstTime == 1)
                {
                    if (obj.TotalPay == 0)
                    {

                        obj.StatusPay = -1;
                    }
                    else
                    {
                        obj.StatusPay = DiffLastTransaction(lastpay, obj.ContractPayEveryDay, obj.ContractSpecialholiday, HolidaysArr, obj.IsFirstTime);
                    }

                }
                else {
                    obj.StatusPay = DiffLastTransaction(lastpay, obj.ContractPayEveryDay, obj.ContractSpecialholiday, HolidaysArr, obj.IsFirstTime);
                }

                list.Add(obj);
            }
            return list;
        }



        public static int DiffLastTransaction(DateTime dateTransaction, int ContractPayEveryDay, bool ContractSpecialholiday, DateTime[] HolidaysArr ,int isFirstTime=0)
        {

            try
            {
                // -2 จ่ายล่วงหน้า 
                // -1  ยังไม่ชำระแล้ว
                //  0 ชำระแล้ว
                if (isFirstTime == 1) {
                    return -1;
                }
                DateTime dateNow = DateTime.Now.Date;
                double result = 0;
                   result = (dateNow - dateTransaction.Date).TotalDays;
                int diffdate = Convert.ToInt32(Math.Floor(result)) + 1;
            

                if (dateNow == dateTransaction)
                {
                    return 0;
                }
                else if (dateNow > dateTransaction)
                {
                    return -1;
                }
                else {

                    return -2;
                }
                    

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

}
    }


