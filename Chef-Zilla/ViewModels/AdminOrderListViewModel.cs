using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chef_Zilla.ViewModels
{
    public class AdminOrderListViewModel
    {
        public List<int> OrderId { get; set; }
        public List<string> UserName { get; set; }
        public List<int> finalTotalPrice { get; set; }
        public List<string> dateTime { get; set; }
        public List<string> status { get; set; }
        public List<string> Type { get; set; }
    }
}