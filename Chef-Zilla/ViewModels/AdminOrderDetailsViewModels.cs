using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chef_Zilla.ViewModels
{
    public class AdminOrderDetailsViewModels
    {
        public int OrderId { get; set; }
        public string UserId { get; set; }
        public List<int> ProductTotalPrice { get; set; }
        public int finalTotalPrice { get; set; }
        public string Address { get; set; }
        public int totalItem { get; set; }
        public string dateTime { get; set; }
        public string status { get; set; }
        public string type { get; set; }
        public List<string> ProductName { get; set; }
        public List<int> ProductId { get; set; }
        public List<int> ProductQuantity { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string phn { get; set; }

    }
}