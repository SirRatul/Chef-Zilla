using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Chef_Zilla.Models;

namespace Chef_Zilla.ViewModels
{
    public class RoutineCreateViewModel
    {
        [Required]
        public string RoutineName { get; set; }
        public List<Box> Boxes { get; set; }
        public int BoxSelectID { get; set; }
        [Required]
        public int BoxID { get; set; }
        [Required]
        public string RoutineType { get; set; }
        public List<string> DeliveryDay { get; set; }
        public string DeliveryDate { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public string DeliveryAddress { get; set; }
        public int TotalPrice { get; set; }
        public List<int> TotalPriceList { get; set; }

    }
}