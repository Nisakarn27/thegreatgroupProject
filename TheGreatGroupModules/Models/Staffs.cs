using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheGreatGroupModules.Models
{
    public class Staffs
    {
      public int    StaffID { get; set; }
             public int  StaffRoleID { get; set; }
             public string StaffRoleName { get; set; }
             public string  StaffCode { get; set; }
              public string StaffPassword { get; set; }
              public string StaffTitleName { get; set; }
            public string   StaffFirstName { get; set; }
            public string   StaffLastName { get; set; }
            public string StaffName { get; set; }
           public string    StaffAddress1 { get; set; }
          public string     StaffAddress2 { get; set; }
             public int  StaffSubDistrictId { get; set; }
            public int   StaffDistrictId { get; set; }
            public int   StaffProvinceId { get; set; }
            public string StaffZipCode { get; set; }
           public string    StaffTelephone { get; set; }
           public string StaffMobile { get; set; }
            public string   StaffEmail { get; set; }
            public int InsertBy { get; set; }
            public int UpdateBy { get; set; }
          public int     Activated { get; set; }
         public int Deleted { get; set; }
    }

    public class StaffRole
    {

        public int StaffRoleID { get; set; }
        public string StaffRoleName { get; set; }
        public int Activated { get; set; }
         public int Deleted { get; set; }
    }


    public class StaffLogin
    {
        public int StaffID { get; set; }
        public int StaffRoleID { get; set; }
        public string StaffName { get; set; }
        public string StaffCode { get; set; }
        public string StaffPassword { get; set; }
        public string ImageUrl { get; set; }
        public int Activated { get; set; }
        public int Deleted { get; set; }
    }


    public class StaffSettingOTP
    {
        public int StaffID { get; set; }
        public int StaffRoleID { get; set; }
        public string StaffName { get; set; }
        public string StaffCode { get; set; }
        public string OTP { get; set; }
        public string StaffRoleName { get; set; }
        public int Activated { get; set; }
        public int Deleted { get; set; }
    }
    public class StaffPermissionGroup
    {
        public int StaffPermissionGroupID { get; set; }
        public string StaffPermissionGroupName { get; set; }
        public string StaffPermissionGroupIcon { get; set; }
        public List<StaffPermission> ListPermission { get; set; }

    }
    public class StaffPermission {
        public int StaffPermissionID { get; set; }
        public int StaffPermissionGroupID { get; set; }
        public string StaffPermissionName { get; set; }
        public string StaffPermissionIcon { get; set; }
        public string StaffPermissionUrl { get; set; }
        public string StaffPermissionGroupName { get; set; }
      
        public int IsMenu { get; set; }
    
    }


    public class StaffLocation {

        public string CustomerFirstName { get; set; }
        public decimal PriceReceipts { get; set; }
        public string PriceReceipts_str { get { return PriceReceipts.ToString("#,##0.00"); } }
        public string TimePay { get; set; }
        public double Latitude { get; set; }
        public double Logitude { get; set; }
    }


    public class MarkersData {
        public List<double> location { get; set; }
        public Tooltip tooltip { get; set; }
    
    
    }
    public class Tooltip {
        public string text { get; set; }
    }
}