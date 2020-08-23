using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chef_Zilla.Models
{
    public class ExtraIngredient
    {
        public int ExtraIngredientID { get; set; }
        public string ExtraIngredientName { get; set; }
        public string ExtraIngredientPrice { get; set; }
        public int ProductID { get; set; }
    }
}