using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheGreatGroupModules.Models
{
    public class Setting
    {
    }


    public class ListItems
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Value { get; set; }
    }

    public class Zone
    {
        public int ZoneID { get; set; }
        public string ZoneCode { get; set; }
        public string ZoneName{ get; set; }
        public int Activated { get; set; }
        public int Deleted { get; set; }

    }

    public class StaffZone
    {
        public int StaffID { get; set; }
        public int ZoneID { get; set; }
        public string ZoneName { get; set; }
        public string StaffName { get; set; }

    }


    public class Holidays
    {
        public int ID { get; set; }
        public string HolidayName { get; set; }
        public DateTime Date { get; set; }
        public string Date_str { get { return Date.ToString(@"dd\/MM\/yyyy"); } }
        public int Activated { get; set; }
        public int Deleted { get; set; }


    }
}