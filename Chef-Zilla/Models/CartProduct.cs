using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chef_Zilla.Models
{
    public class CartProduct
    {
        public int CartProductID { get; set; }
        public int ProductID { get; set; }
        public int CartID { get; set; }
        public int ProductQuantity { get; set; }
        public int TotalPrice { get; set; }



    }
}