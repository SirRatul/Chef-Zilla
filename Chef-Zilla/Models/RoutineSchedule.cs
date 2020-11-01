using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chef_Zilla.Models
{
    public class RoutineSchedule
    {
        public int RoutineScheduleID { get; set; }
        public string RoutineType { get; set; }
        public string DeliveryDay { get; set; }
        public string DeliveryDate { get; set; }
        public int RoutineID { get; set; }
        public string DeliveredDate { get; set; }
        public string OrderStatus { get; set; }
    }
}