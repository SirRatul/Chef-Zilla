using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Chef_Zilla.Models
{
    public class Product
    {
        public int ProductID { get; set; }
        public string ProductType { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public string PrepareTime { get; set; }
        public string ProductPrice { get; set; }
        //public int IngredientID { get; set; }
        //public int ExtraIngredientID { get; set; }
        public List<string> IngredientName { get; set; }
        public List<string> IngredientQuantity { get; set; }
        public List<string> ExtraIngredientName { get; set; }
        public List<string> ExtraIngredientPrice { get; set; }
        [NotMapped]
        public HttpPostedFileBase ProductImageFile { get; set; }
    }
}