using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Chef_Zilla.Models;

namespace Chef_Zilla.ViewModels
{
    public class FoodPlanViewModel
    {
        public List<Box> Boxes { get; set; }
        public List<int> RoutineID { get; set; }
        public List<string> RoutineName { get; set; }
        public List<string> RoutineType { get; set; }
        public List<string> BoxName { get; set; }
    }
}