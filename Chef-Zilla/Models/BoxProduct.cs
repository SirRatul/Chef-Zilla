using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chef_Zilla.Models
{
    public class BoxProduct
    {
        public int BoxProductID { get; set; }
        public int ProductID { get; set; }
        public int ProductQuantity { get; set; }
        public int TotalPrice { get; set; }
        public int BoxID { get; set; }
    }
}