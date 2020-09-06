using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chef_Zilla.ViewModels
{
    public class BoxItemViewModel
    {
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public string PrepareTime { get; set; }
        public int ProductPrice { get; set; }
        public int ProductQuantity { get; set; }
        public List<string> IngredientName { get; set; }
        public List<string> IngredientQuantity { get; set; }
        public List<int> ExtraIngredientId { get; set; }
        public List<string> ExtraIngredientName { get; set; }
        public List<string> ExtraIngredientPrice { get; set; }
        public List<int> ExtraIngredientSelecetedID { get; set; }
        public List<string> ExtraIngredientSelecetedQuantity { get; set; }
    }
}