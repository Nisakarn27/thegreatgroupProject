using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace TheGreatGroupModules.Models
{
    public class Customers
    {
       
        public int CustomerID { get; set; }
        public string CustomerTitleName { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerPassword { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }
        public string CustomerNickName { get; set; }
        public string CustomerName {
            get{
                return CustomerTitleName + CustomerFirstName + " " + CustomerLastName;
            }
        }
        public string CustomerAddress1 { get; set; }
        public string CustomerAddress2 { get; set; }
        public string CustomerSubDistrict { get; set; }
        public string CustomerDistrict { get; set; }
        public string CustomerProvince { get; set; }
        public string CustomerZipCode { get; set; }
        public int CustomerSubDistrictId { get; set; }
        public int CustomerDistrictId { get; set; }
        public int CustomerProvinceId { get; set; }
        public string CustomerMobile { get; set; }
        public string CustomerTelephone { get; set; }
        public string CustomerEmail { get; set; }
        
        public string CustomerIdCard { get; set; }
        public string CustomerCareer { get; set; }
           public string   CustomerStatus{ get; set; }
          public int     CustomerPartner{ get; set; }
            public string   CustomerJob{ get; set; }
            public string   CustomerJobYear{ get; set; }
          public string     CustomerSalary{ get; set; }
           public string    CustomerJobAddress{ get; set; }
           public int    CustomerJobSubDistrictId{ get; set; }
           public int CustomerJobDistrictId { get; set; }
           public int CustomerJobProvinceId { get; set; }
          public string     CustomerJobZipCode{ get; set; }
          public string     CustomerSpouseTitle{ get; set; }
           public string    CustomerSpouseFirstName{ get; set; }
           public string    CustomerSpouseLastName{ get; set; }
           public string    CustomerSpouseNickName{ get; set; }
           public string    CustomerSpouseAddress{ get; set; }
           public int CustomerSpouseSubDistrictId { get; set; }
           public int CustomerSpouseDistrictId { get; set; }
           public int CustomerSpouseProvinceId { get; set; }
            public string   CustomerSpouseZipCode{ get; set; }
            public string   CustomerSpouseMobile{ get; set; }
            public string   CustomerSpouseTelephone{ get; set; }
           public string  CustomerEmergencyTitle{ get; set; }
             public string CustomerEmergencyFirstName{ get; set; }
             public string CustomerEmergencyLastName{ get; set; }
             public string CustomerEmergencyRelation{ get; set; }
             public string CustomerEmergencyMobile{ get; set; }
             public string CustomerEmergencyTelephone { get; set; }
           public int    SaleID{ get; set; }
           public int ContractID { get; set; }
        public int Activated { get; set; }
        public int Deleted { get; set; }
        public int CustomerInsertBy { get; set; }
        public int CustomerUpdateBy { get; set; }
        public static IList<Customers> ToObjectList(DataTable dt)
        {
            return dt.AsEnumerable().Select(dr => new Customers()
            {
                CustomerID = dr.Field<int>("CustomerId"),
                CustomerTitleName = dr.Field<string>("CustomerTitleName"),
                CustomerCode = dr.Field<string>("CustomerCode"),
                CustomerNickName = dr.Field<string>("CustomerNickName"),
                CustomerFirstName = dr.Field<string>("CustomerFirstname"),
                CustomerLastName = dr.Field<string>("CustomerLastname"),
                CustomerAddress1 = dr.Field<string>("CustomerAddress1"),
                CustomerSubDistrict = dr.Field<string>("CustomerSubDistrict"),
                CustomerDistrict = dr.Field<string>("CustomerDistrict"),
                CustomerProvince = dr.Field<string>("CustomerProvince"),
                CustomerSubDistrictId = dr.Field<int>("CustomerSubDistrictId"),
                CustomerDistrictId = dr.Field<int>("CustomerDistrictId"),
                CustomerProvinceId = dr.Field<int>("CustomerProvinceId"),
                CustomerZipCode = dr.Field<string>("CustomerZipCode"),
                CustomerEmail = dr.Field<string>("CustomerEmail"),
                CustomerMobile = dr.Field<string>("CustomerMobile"),
                CustomerTelephone = dr.Field<string>("CustomerTelephone"),
                CustomerStatus = dr.Field<string>("CustomerStatus"),
                CustomerIdCard = dr.Field<string>("CustomerIdCard"),
                CustomerCareer = dr.Field<string>("CustomerCareer"),
                CustomerJob= dr.Field<string>("CustomerJob"),
                CustomerJobYear= dr.Field<string>("CustomerJobYear"),
                CustomerSalary = dr.Field<string>("CustomerSalary"),
                CustomerJobAddress = dr.Field<string>("CustomerJobAddress"),
                SaleID = dr.Field<int>("SaleID"),
                CustomerPartner = dr.Field<int>("CustomerPartner"),
                CustomerJobSubDistrictId = dr.Field<int>("CustomerJobSubDistrictId"),
                CustomerJobDistrictId = dr.Field<int>("CustomerJobDistrictId"),
                CustomerJobProvinceId = dr.Field<int>("CustomerJobProvinceId"),
                CustomerJobZipCode = dr.Field<string>("CustomerJobZipCode"),
                CustomerSpouseTitle = dr.Field<string>("CustomerSpouseTitle"),
                CustomerSpouseFirstName = dr.Field<string>("CustomerSpouseFirstName"),
                CustomerSpouseLastName = dr.Field<string>("CustomerSpouseLastName"),
                CustomerSpouseNickName = dr.Field<string>("CustomerSpouseNickName"),
                CustomerSpouseAddress = dr.Field<string>("CustomerSpouseAddress"),
                CustomerSpouseSubDistrictId = dr.Field<int>("CustomerSpouseSubDistrictId"),
                CustomerSpouseDistrictId = dr.Field<int>("CustomerSpouseDistrictId"),
                CustomerSpouseProvinceId = dr.Field<int>("CustomerSpouseProvinceId"),
                CustomerSpouseZipCode = dr.Field<string>("CustomerSpouseZipCode"),
                CustomerSpouseMobile = dr.Field<string>("CustomerSpouseMobile"),
                CustomerSpouseTelephone = dr.Field<string>("CustomerSpouseTelephone"),
                CustomerEmergencyTitle = dr.Field<string>("CustomerEmergencyTitle"),
                CustomerEmergencyFirstName = dr.Field<string>("CustomerEmergencyFirstName"),
                CustomerEmergencyLastName = dr.Field<string>("CustomerEmergencyLastName"),
                CustomerEmergencyRelation = dr.Field<string>("CustomerEmergencyRelation"),
                CustomerEmergencyMobile = dr.Field<string>("CustomerEmergencyMobile"),
                CustomerEmergencyTelephone = dr.Field<string>("CustomerEmergencyTelephone"),
            }).ToList();
        }
         
        public static IList<Customers> ToObjectList2(DataTable dt)
        {
            return dt.AsEnumerable().Select(dr => new Customers()
            {
                CustomerID = dr.Field<int>("CustomerId"),
                CustomerTitleName = dr.Field<string>("CustomerTitleName"),
                CustomerCode = dr.Field<string>("CustomerCode"),
                CustomerNickName = dr.Field<string>("CustomerNickName"),
                CustomerFirstName = dr.Field<string>("CustomerFirstname"),
                CustomerLastName = dr.Field<string>("CustomerLastname"),
                CustomerAddress1 = dr.Field<string>("CustomerAddress1") 
                 +" ตำบล/แขวง"  + dr.Field<string>("CustomerSubDistrict")
                 + " อำเภอ/เขต" + dr.Field<string>("CustomerDistrict")
                 + " จังหวัด" + dr.Field<string>("CustomerProvince")
                 +" " + dr.Field<string>("CustomerZipCode"),
                CustomerAddress2 = dr.Field<string>("CustomerAddress1")
                 + " ต." + dr.Field<string>("CustomerSubDistrict")
                 + " อ." + dr.Field<string>("CustomerDistrict")
                 + " จ." + dr.Field<string>("CustomerProvince"),
            
                CustomerEmail = dr.Field<string>("CustomerEmail"),
                CustomerMobile = dr.Field<string>("CustomerMobile"),
                CustomerTelephone = dr.Field<string>("CustomerTelephone"),
                CustomerIdCard = dr.Field<string>("CustomerIdCard"),
                CustomerCareer = dr.Field<string>("CustomerCareer"),
                SaleID = dr.Field<int>("SaleID"),
            }).ToList();
        }

        public static IList<Customers> ToObjectList3(DataTable dt)
        {
            return dt.AsEnumerable().Select(dr => new Customers()
            {
                ContractID = dr.Field<int>("ContractID"),
                CustomerID = dr.Field<int>("CustomerId"),
                CustomerTitleName = dr.Field<string>("CustomerTitleName"),
                CustomerCode = dr.Field<string>("CustomerCode"),
                CustomerNickName =dr.Field<string>("CustomerNickName"),
                CustomerFirstName = dr.Field<string>("CustomerFirstname"),
                CustomerLastName = dr.Field<string>("CustomerLastname") + " (" + dr.Field<string>("ContractNumber") + ") ",
                CustomerAddress1 = dr.Field<string>("CustomerAddress1")
                 + " ตำบล/แขวง" + dr.Field<string>("CustomerSubDistrict")
                 + " อำเภอ/เขต" + dr.Field<string>("CustomerDistrict")
                 + " จังหวัด" + dr.Field<string>("CustomerProvince")
                 + " " + dr.Field<string>("CustomerZipCode"),

                CustomerAddress2 = dr.Field<string>("CustomerAddress1")
                + " ต." + dr.Field<string>("CustomerSubDistrict")
                + " อ." + dr.Field<string>("CustomerDistrict")
                + " จ." + dr.Field<string>("CustomerProvince"),
                CustomerEmail = dr.Field<string>("CustomerEmail"),
                CustomerMobile = dr.Field<string>("CustomerMobile"),
                CustomerTelephone = dr.Field<string>("CustomerTelephone"),
                CustomerIdCard = dr.Field<string>("CustomerIdCard"),
                CustomerCareer = dr.Field<string>("CustomerCareer"),
            }).ToList();
        }
    }
}