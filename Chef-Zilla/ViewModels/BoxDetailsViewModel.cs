using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chef_Zilla.ViewModels
{
    public class BoxDetailsViewModel
    {
        public int BoxID { get; set; }
        public string BoxName { get; set; }
        public List<int> ProductID { get; set; }
        public List<string> ProductName { get; set; }
        public List<int> ProductQuantity { get; set; }
        public List<int> TotalPrice { get; set; }
    }
}