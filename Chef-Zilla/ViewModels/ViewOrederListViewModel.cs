using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chef_Zilla.ViewModels
{
    public class ViewOrederListViewModel
    {
        public int ViewOrederListViewModelId { get; set; }
        public List<int> OrderId { get; set; }
        public string UserId { get; set; }
        public List<int> finalTotalPrice { get; set; }
        public List<string> Address { get; set; }
        public List<int> totalItem { get; set; }
        public List<string> dateTime { get; set; }
        public List<string> status { get; set; }

        public List<int> ProductID { get; set; }
        public int ProductQuantity { get; set; }

        public List<string> ProductName { get; set; }


    }
}