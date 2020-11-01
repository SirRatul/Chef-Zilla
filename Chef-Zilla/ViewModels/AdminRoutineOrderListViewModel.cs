using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chef_Zilla.ViewModels
{
    public class AdminRoutineOrderListViewModel
    {
        public List<int> RoutineOrderId { get; set; }
        public List<string> RoutineType { get; set; }
        public List<string> UserName { get; set; }
        public List<int> boxPrice { get; set; }
        public List<string> dateTime { get; set; }
        public List<string> status { get; set; }
    }
}