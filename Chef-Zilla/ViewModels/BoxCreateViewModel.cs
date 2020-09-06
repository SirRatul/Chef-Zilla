using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Chef_Zilla.ViewModels
{
    public class BoxCreateViewModel
    {
        public int BoxID { get; set; }
        [Required]
        public string BoxName { get; set; }
        public List<int> ProductID { get; set; }
        public List<string> ProductQuantity { get; set; }
        public List<string> TotalPrice { get; set; }
        public List<int> ExtraIngredientID { get; set; }
        public List<int> ExtraIngredientQuantity { get; set; }
        public string UserId { get; set; }
    }
}