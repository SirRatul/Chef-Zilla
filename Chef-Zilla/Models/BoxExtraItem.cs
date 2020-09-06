using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chef_Zilla.Models
{
    public class BoxExtraItem
    {
        public int BoxExtraItemID { get; set; }
        public int ExtraIngredientID { get; set; }
        public string ExtraIngredientQuantity { get; set; }
        public int BoxID { get; set; }
    }
}