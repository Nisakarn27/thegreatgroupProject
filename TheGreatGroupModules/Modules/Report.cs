using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace TheGreatGroupModules.Modules
{


    /*  บันทึกค่างวดรายวัน */
    public class DailyReceiptsReport
    {
        public int ID { get; set; }
        public int CustomerID { get; set; }
        public int ContractID { get; set; }
        public string ContractNumber { get; set; }
        public string CustomerName { get; set; }
        public string CustomerNickName { get; set; }
        public DateTime ContractCreateDate { get; set; }
        public DateTime ContractExpDate { get; set; }

        public DateTime ContractStartDate { get; set; }
        public bool ContractSpecialholiday { get; set; }
        public int ContractPayEveryDay { get; set; }
        public decimal ContractAmountLast { get; set; }
        public decimal ContractAmount { get; set; }
        public decimal ContractDiscount { get; set; }
        public decimal PriceReceipts { get; set; }
        public decimal Balance { get; set; }
        public decimal TotalSales { get; set; }
        public string ContractCreateDate_Text { get { return ContractCreateDate.ToString("dd/MM/yyyy"); } }
        public string ContractExpDate_Text { get { return ContractExpDate.ToString("dd/MM/yyyy"); } }
        public string ContractAmount_Text { get { return ContractAmount.ToString("#,##0.00"); } }
        public string PriceReceipts_Text { get { return PriceReceipts.ToString("#,##0.00"); } }
        public string Balance_Text { get { return Balance.ToString("#,##0.00"); } }
        public string TotalSales_Text { get { return TotalSales.ToString("#,##0.00"); } }
        public string ContractDiscount_Text { get { return ContractDiscount.ToString("#,##0.00"); } }
        public int StaffID { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileNumber { get; set; }
        public decimal TotalBalance { get; set; }
        public string Status { get; set; }
        public string Remark { get; set; }
        public string StaffName { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime LastPayment { get; set; }
        public int ContractAccountStatus { get; set; }
        public string ContractAccountStatusName { get { string ContractAccountStatusName = Utility.CheckAccountStatus(ContractAccountStatus); return ContractAccountStatusName; } }
        public IList<LastTransaction> latest_transaction { get {
                ReportData data = new ReportData();
                return data.GetTransaction(StaffID.ToString(), CustomerID.ToString(), ContractID.ToString()); 

            } }

        public int ContractTimes { get; internal set; }
        public DateTime ContractCloseDate { get; internal set; }
        public string ContractCloseDate_Text { get { return ContractCloseDate.ToString("dd/MM/yyyy"); } }
        public static List<DailyReceiptsReport> ToObjectList(DataTable dt)
        {
            return dt.AsEnumerable().Select(dr => new DailyReceiptsReport()
            {
                CustomerID = dr.Field<int>("CustomerID"),
                ContractID = dr.Field<int>("ContractID"),
                ContractNumber = dr.Field<string>("ContractNumber"),
                CustomerName = dr.Field<string>("CustomerName"),
                CustomerNickName = dr.Field<string>("CustomerNickName"),
                ContractCreateDate = dr.Field<DateTime>("ContractCreateDate"),
                ContractExpDate = dr.Field<DateTime>("ContractExpDate"),
                ContractAmount = dr.Field<decimal>("ContractAmount"),
                PriceReceipts = dr.Field<decimal>("PriceReceipts"),
                Balance = dr.Field<decimal>("Balance"),
                TotalBalance = dr.Field<decimal>("ContractPayment"),
                ContractAmountLast = dr.Field<decimal>("ContractAmountLast"),
                PhoneNumber = dr.Field<string>("CustomerTelephone"),
                MobileNumber = dr.Field<string>("CustomerMobile"),
                StaffID = dr.Field<int>("StaffID"),
            }).ToList();
        }
    }

    public class DailyRemark
    {
        public int ID { get; set; }
        public int CustomerID { get; set; }
        public int ContractID { get; set; }
        public string Remark { get; set; }
        public DateTime DateAsOf { get; set; }
        public string Date { get { return DateAsOf.ToString("dd/MM/yyyy"); } }
    }
    public class Transaction
    {

        public int CustomerID { get; set; }
        public int ContractID { get; set; }
        public DateTime DateAsOf { get; set; }
        public string DateAsOf_Text { get { return DateAsOf.ToString("dd/MM/yyyy"); } }
        public string ContractNumber { get; set; }
        public string CustomerName { get; set; }
        public decimal Balance { get; set; } // ยอดเงินคงเหลือ
        public decimal ContractAmount { get; set; } // งวดละ
        public decimal ContractPayment { get; set; } //  จำนวนเงินทั้งหมด
        public string Balance_Text { get; set; } // ยอดเงินคงเหลือ
        public string ContractAmount_Text { get; set; } // งวดละ
        public string ContractPayment_Text { get; set; } // จำนวนเงินทั้งหมด
    }

    public class LastTransaction
    {
        public DateTime DateAsOf { get; set; }
        public string Date { get { return DateAsOf.ToString("dd/MM/yyyy"); } }
        public decimal Amount { get; set; } // งวดละ
    }

    public class OpenAccountReport
    {

        public int CustomerID { get; set; }
        public int ContractID { get; set; }
        public string ContractNumber { get; set; }
        public string CustomerName { get; set; }
        public string CustomerMobile { get; set; }
        public string CustomerAddress { get; set; }
        public DateTime ContractCreateDate { get; set; }
        public string ContractCreateDate_Text { get { return ContractCreateDate.ToString("dd/MM/yyyy"); } }
        public DateTime ContractExpDate { get; set; }
        public string ContractExpDate_Text { get { return ContractExpDate.ToString("dd/MM/yyyy"); } }
        public decimal UnitAmount { get; set; } // น้ำหนัก
        public decimal TotalPayment { get; set; } // ยอดสินเชื่อ
        public decimal CostAmount { get; set; } // ราคาทุน
        public string TotalPayment_Text { get { return TotalPayment.ToString("#,##0.00"); } } // ยอดสินเชื่อ
        public string CostAmount_Text { get { return CostAmount.ToString("#,##0.00"); } } // ราคาทุน
        public DateTime? ContractCloseDate { get; set; }
        public string ContractCloseDate_Text { get { return ContractCloseDate == null ?"":Convert.ToDateTime(ContractCloseDate).ToString("dd/MM/yyyy"); } }
        public decimal Benefit { get; set; } // ยอดสินเชื่อ
        public string Benefit_Text { get { return Benefit.ToString("#,##0.00"); } } // ราคาทุน
        public static List<OpenAccountReport> ToObjectList(DataTable dt)
        {
            return dt.AsEnumerable().Select(dr => new OpenAccountReport()
            {
                CustomerID = dr.Field<int>("ContractCustomerID"),
                ContractID = dr.Field<int>("ContractID"),
                ContractNumber = dr.Field<string>("ContractNumber"),
                CustomerName = dr.Field<string>("CustomerName") ,
               //CustomerMobile = dr.Field<string>("CustomerMobile") ,
               // CustomerAddress = " ที่อยู่ " + dr.Field<string>("CustomerAddress"),
                ContractCreateDate = dr.Field<DateTime>("ContractCreateDate"),
                ContractExpDate = dr.Field<DateTime>("ContractExpDate"),
                ContractCloseDate = dr.Field<DateTime?>("ContractCloseDate"),
                TotalPayment = dr.Field<decimal>("ContractPayment"),
                CostAmount = dr.Field<decimal>("PriceCost"),
                UnitAmount = dr.Field<decimal>("UnitAmount"),
                Benefit = dr.Field<decimal>("diff"),
            }).ToList();
        }

    }


    public class SearchCriteria
    {

        public int TypeDate { get; set; }
        public string CustomerName { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string FromDateStr { get { return FromDate.ToString("dd/MM/yyyy"); } }
        public string ToDateStr { get { return ToDate.ToString("dd/MM/yyyy"); } }
        public int Month { get; set; }
        public int Year { get; set; }
        public int ContractID { get; set; }
        public int CustomerID { get; set; }
        public int ZoneID { get; set; }
    }



    public class ReportCustomer {

            public int Day { get; set; }
            public decimal Month1 { get; set; }
            public decimal Month2 { get; set; }
            public decimal Month3 { get; set; }
            public decimal Month4 { get; set; }
            public decimal Month5 { get; set; }
            public decimal Month6 { get; set; }
            public decimal Month7 { get; set; }
            public decimal Month8 { get; set; }
            public decimal Month9 { get; set; }
            public decimal Month10 { get; set; }
            public decimal Month11 { get; set; }
            public decimal Month12 { get; set; }

            public string Month1_Str { get {
                if (Month1 > 0) {

                    return Month1.ToString("#,##0.00");
                }
                else { return ""; }
                }
             
            }
            public string Month2_Str
            {
                get
                {
                    if (Month2 > 0)
                    {

                        return Month2.ToString("#,##0.00");
                    }
                    else { return Month2 == -99 ? "*" : ""; }
                }
            }
            public string Month3_Str
            {
                get
                {
                    if (Month3 > 0)
                    {

                        return Month3.ToString("#,##0.00");
                    }
                    else { return ""; }
                }
            }
            public string Month4_Str
            {
                get
                {
                    if (Month4 > 0)
                    {

                        return Month4.ToString("#,##0.00");
                    }
                    else { return Month4 == -99 ? "*" : ""; }
                }
            }
            public string Month5_Str
            {
                get
                {
                    if (Month5 > 0)
                    {

                        return Month5.ToString("#,##0.00");
                    }
                    else { return ""; }
                }
            }
            public string Month6_Str
            {
                get
                {
                    if (Month6 > 0)
                    {

                        return Month6.ToString("#,##0.00");
                    }
                    else { return Month6 == -99 ? "*" : ""; }
                }
            }
            public string Month7_Str
            {
                get
                {
                    if (Month7 > 0)
                    {

                        return Month7.ToString("#,##0.00");
                    }
                    else { return ""; }
                }
            }
            public string Month8_Str
            {
                get
                {
                    if (Month8 > 0)
                    {

                        return Month8.ToString("#,##0.00");
                    }
                    else { return ""; }
                }
            }
            public string Month9_Str
            {
                get
                {
                    if (Month9 > 0)
                    {

                        return Month9.ToString("#,##0.00");
                    }
                    else { return Month9 == -99 ? "*" : ""; }
                }
            }
            public string Month10_Str
            {
                get
                {
                    if (Month10 > 0)
                    {

                        return Month10.ToString("#,##0.00");
                    }
                    else { return ""; }
                }
            }
            public string Month11_Str
            {
                get
                {
                    if (Month11 > 0)
                    {

                        return Month11.ToString("#,##0.00");
                    }
                    else { return Month11 == -99?"*":""; }
                }
            }
            public string Month12_Str
            {
                get
                {
                    if (Month12 > 0)
                    {

                        return Month12.ToString("#,##0.00");
                    }
                    else { return ""; }
                }
            } 

    
    
    
    }



    public class ReportCustomerOnCard
    {

        public int Day { get; set; }
        public string Month1 { get; set; }
        public string Month2 { get; set; }
        public string Month3 { get; set; }
        public string Month4 { get; set; }
        public string Month5 { get; set; }
        public string Month6 { get; set; }
        public string Month7 { get; set; }
        public string Month8 { get; set; }
        public string Month9 { get; set; }
        public string Month10 { get; set; }
        public string Month11 { get; set; }
        public string Month12 { get; set; }
    }

    public class NPLReport {


        public int StaffID { get; set; }
        public int ZoneID { get; set; }
        public int NPLDay { get; set; }
        public DateTime?  NPLDate  { get; set; }
    
        public DateTime ContractCreateDate { get; set; }
        public DateTime ContractStartDate { get; set; }
        public DateTime ContractExpDate { get; set; }
        public int ContractCustomerID { get; set; }
        public string ContractNumber { get; set; }
        public string CustomerName { get; set; }
        public decimal ContractPayment { get; set; }

        public decimal NPL { get; set; }

        public DateTime LastpayDate { get; set; }
        
    }                         
}                             
                              