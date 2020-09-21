using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chef_Zilla.Models
{
    public class OrderProduct
    {
        public int OrderProductId { get; set; }
        public int OrderId { get; set; }
        public int ProductID { get; set; }
        public int ProductQuantity { get; set; }
        public int ProductPrice { get; set; }
    }
}