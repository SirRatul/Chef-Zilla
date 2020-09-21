using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Chef_Zilla.ViewModels
{
    public class ItemDetailsViewModels
    {
        public int ProductID { get; set; }
        public string ProductType { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public string PrepareTime { get; set; }
        public string ProductPrice { get; set; }
        public List<string> IngredientName { get; set; }
        public List<string> IngredientQuantity { get; set; }
        public List<int> ExtraIngredientId { get; set; }
        public List<string> ExtraIngredientName { get; set; }
        public List<string> ExtraIngredientPrice { get; set; }
        public List<int> ReviewId { get; set; }
        public List<string> ReviewerUserName { get; set; }
        public List<int> Ratings { get; set; }
        public List<string> ReviewText { get; set; }
        public List<string> ReviewDateTime { get; set; }
    }
}