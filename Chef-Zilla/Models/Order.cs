using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chef_Zilla.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public string UserId { get; set; }
        public int finalTotalPrice { get; set; } 
        public string Address { get; set; }
        public int totalItem { get; set; }
        public String dateTime { get; set; }
        public string status { get; set; }
        public string Type { get; set; }


       
    }
}