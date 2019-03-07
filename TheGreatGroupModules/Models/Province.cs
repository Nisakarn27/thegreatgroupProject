using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace TheGreatGroupModules.Models
{
    public class Province
    {

        public int ProvinceID { get; set; }
        public string ProvinceName { get; set; }

        public static List<Province> ToObjectList(DataTable dt)
        {
            return dt.AsEnumerable().Select(dr => new Province()
            {
                ProvinceID = dr.Field<int>("ProvinceId"),
                ProvinceName = dr.Field<string>("ProvinceName"),
            
            }).ToList();
        }
    }

    public class District
    {
        public int DistrictID { get; set; }
        public int ProvinceID { get; set; }
        public string DistrictName { get; set; }

        public static List<District> ToObjectList(DataTable dt)
        {
            return dt.AsEnumerable().Select(dr => new District()
            {
                DistrictID = dr.Field<int>("DistrictId"),
                ProvinceID = dr.Field<int>("ProvinceId"),
                DistrictName = dr.Field<string>("DistrictName"),

            }).ToList();
        }
    }

    public class SubDistrict
    {

        public int SubDistrictID { get; set; }
        public int DistrictID { get; set; }
        public string SubDistrictName { get; set; }
        public string SubDistrictZipCode { get; set; }
        
        public static List<SubDistrict> ToObjectList(DataTable dt)
        {
            return dt.AsEnumerable().Select(dr => new SubDistrict()
            {
                SubDistrictID = dr.Field<int>("SubDistrictId"),
                DistrictID = dr.Field<int>("DistrictId"),
                SubDistrictName = dr.Field<string>("SubDistrictName"),
                SubDistrictZipCode = dr.Field<string>("SubDistrictZipCode"),
            }).ToList();
        }
    }
}