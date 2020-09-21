using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chef_Zilla.ViewModels
{
    public class WishListViewModel
    {
        public List<int> WishListID { get; set; }
        public List<int> ProductID { get; set; }
        public string UserId { get; set; }
        public List<string> ProductName { get; set; }
    }
}