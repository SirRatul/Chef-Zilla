using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chef_Zilla.Models
{
    public class Routine
    {
        public int RoutineID { get; set; }
        public string RoutineName { get; set; }
        public int BoxID { get; set; }
        public string UserId { get; set; }
        public string DeliveryAddress { get; set; }
        public string RoutineStatus { get; set; }
    }
}