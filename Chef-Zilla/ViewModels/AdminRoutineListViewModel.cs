using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chef_Zilla.ViewModels
{
    public class AdminRoutineListViewModel
    {
        public List<int> RoutineOrderId { get; set; }
        public List<string> RoutineName { get; set; }
        public List<string> RoutineType { get; set; }
        public List<string> RoutineStatus { get; set; }
        public List<string> UserName { get; set; }
        public List<int> boxPrice { get; set; }
    }
}