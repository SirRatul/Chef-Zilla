using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chef_Zilla.ViewModels
{
    public class CartViewModel
    {
        public int CartViewModelID { get; set; }
        public List<int> CartID { get; set; }
        //public string BoxName { get; set; }
        public String UserId { get; set; }
        public List<int> ProductID { get; set; }
        public List<string> ProductName { get; set; }
        public List<int> ProductQuantity { get; set; }
        public List<int> TotalPrice { get; set; }
        public int FinalTotalPrice { get; set; }
        public String Name { get; set; }
        public String Address { get; set; }
        public String PhnNo { get; set; }
        public String Email { get; set; }
        public String dateTime { get; set; }

        public List<int> extraIngrediantID { get; set; }
        public List<int> extraIngrediantQuantity { get; set; }

        
    }
}