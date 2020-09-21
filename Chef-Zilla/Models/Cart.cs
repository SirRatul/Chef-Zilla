using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chef_Zilla.Models
{
    public class Cart
    {
        public int CartID { get; set; }
        public String UserId { get; set; }

        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int totalProductPrice { get; set; }
        public int ProductQuantity { get; set; }
        public List<string> forExtraIngredient { get; set; }

    }
}