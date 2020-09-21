using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chef_Zilla.Models
{
    public class OrderProductExtraItem
    {
        public int OrderProductExtraItemID { get; set; }
        public int OrderId { get; set; }
        public int ExtraItemID { get; set; }
        public int ExtraItemQuantity { get; set; }
    }
}