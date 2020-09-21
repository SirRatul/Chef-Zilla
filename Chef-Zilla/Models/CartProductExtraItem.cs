using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chef_Zilla.Models
{
    public class CartProductExtraItem
    {
        public int CartProductExtraItemID { get; set; }
        public int CartID { get; set; }
        public int ExtraItemID { get; set; }
        public int ExtraItemQuantity { get; set; }

    }
}