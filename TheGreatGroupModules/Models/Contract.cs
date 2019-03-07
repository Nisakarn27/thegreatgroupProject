using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheGreatGroupModules.Models
{
    public class Contract
    {
        public int ContractID { get; set; }
        public int ContractCustomerID { get; set; }
        public string ContractCustomerName { get; set; }
        public DateTime ContractCreateDate { get; set; }
        public DateTime ContractStartDate { get; set; }
        public DateTime ContractExpDate { get; set; }
        public string ContractCreateDate_Text { get { return ContractCreateDate.ToShortDateString(); } }
        public string ContractStartDate_Text { get { return ContractStartDate.ToShortDateString(); } }
        public string ContractExpDate_Text { get { return ContractExpDate.ToShortDateString(); } }
        public string ContractNumber { get; set; }
        public decimal ContractInterest { get; set; }
        public decimal ContractAmount { get; set; }
        public decimal ContractAmountLast { get; set; }
        public int ContractPeriod { get; set; }
        public decimal ContractPayment { get; set; }
        public decimal ContractReward { get; set; }
        public string ContractRefNumber { get; set; }
        public int ContractInsertBy { get; set; }
        public DateTime ContractInsertDate { get; set; }
        public string ContractType { get; set; }
        public int ContractStatus { get; set; }
        public string ContractRemark { get; set; }
        public int ContractTimes { get; set; }
        public int ContractPayEveryDay { get; set; }
        public bool ContractSpecialholiday { get; set; }
        public int ContractAccountStatus { get; set; }
        public string ContractAccountStatusName { get; set; }
        public int ContractUpdateBy { get; set; }
        public DateTime ContractUpdateDate { get; set; }
        public int Activated { get; set; }
        public int Deleted { get; set; }
        public int IsContractAmountLast { get; set; }
        public int CustomerSurety1 { get; set; }
        public int CustomerSurety2 { get; set; }
        public int CustomerPartner { get; set; }
        public CustomerSurety CustomerSuretyData1 { get; set; }
        public CustomerSurety CustomerSuretyData2 { get; set; }
        public CustomerPartner CustomerPartnerData { get; set; }

        public DateTime ContractCloseDate { get; set; }
        public string ContractCloseDate_Text { get { return ContractCloseDate.ToShortDateString(); } }
    }


    public class CustomerSurety {

        public int CustomerSuretyID { get; set; }
        public string  CustomerSuretyTitle { get; set; }
        public string CustomerSuretyFirstName { get; set; }
        public string CustomerSuretyLastName { get; set; }

        public string CustomerSuretyName {
            get
            {
                return CustomerSuretyTitle + CustomerSuretyFirstName + " " + CustomerSuretyLastName;
            }
        }

        
        public string CustomerSuretyAddress1
        {
            get
            {
                return CustomerSuretyAddress + " ตำบล/แขวง" + CustomerSuretySubDistrictName
                 + " อำเภอ/เขต" + CustomerSuretyDistrictName
                 + " จังหวัด" + CustomerSuretyProvinceName
                 + " " + CustomerSuretyZipCode;
            }
        }
        public string CustomerSuretyAddress2
        {
            get
            {
                return CustomerSuretyAddress + " ต." + CustomerSuretySubDistrictName
                 + " อ."+ CustomerSuretyDistrictName
                 + " จ." + CustomerSuretyProvinceName
                 + " " + CustomerSuretyZipCode;
            }
        }
        public string CustomerSuretyAddress { get; set; }
        public int CustomerSuretySubDistrictId { get; set; }
        public string CustomerSuretySubDistrictName { get; set; }
        public int CustomerSuretyDistrictId { get; set; }

        public string CustomerSuretyDistrictName { get; set; }
        public int CustomerSuretyProvinceId { get; set; }
        public string CustomerSuretyProvinceName { get; set; }
        public string CustomerSuretyZipCode { get; set; }
        public string CustomerSuretyMobile { get; set; }
        public string CustomerSuretyTelephone { get; set; }
        public string CustomerSuretyIdCard { get; set; }

    }

    public class CustomerPartner
    {

        public int CustomerPartnerID { get; set; }
        public string CustomerPartnerTitle { get; set; }
        public string CustomerPartnerFirstName { get; set; }
        public string CustomerPartnerLastName { get; set; }
        public string CustomerPartnerName
        {
            get
            {
                return CustomerPartnerTitle + CustomerPartnerFirstName + " " + CustomerPartnerLastName;
            }
        }
        
        public string CustomerPartnerAddress { get; set; }
        public int CustomerPartnerSubDistrictId { get; set; }
        public int CustomerPartnerDistrictId { get; set; }
        public int CustomerPartnerProvinceId { get; set; }
        public string CustomerPartnerZipCode { get; set; }
        public string CustomerPartnerMobile { get; set; }
        public string CustomerPartnerTelephone { get; set; }
        public string CustomerPartnerIdCard { get; set; }
    }


}