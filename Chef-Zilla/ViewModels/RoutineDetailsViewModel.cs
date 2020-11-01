using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chef_Zilla.ViewModels
{
    public class RoutineDetailsViewModel
    {
        public int RoutineID { get; set; }
        public string RoutineName { get; set; }
        public string BoxName { get; set; }
        public string RoutineType { get; set; }
        public string RoutineStatus { get; set; }
        public string DeliveryDay { get; set; }
        public string DeliveryDate { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public string DeliveryAddress { get; set; }
        public string UpcomingDeliveryDate { get; set; }
        public string DeliveredDate { get; set; }
    }
}