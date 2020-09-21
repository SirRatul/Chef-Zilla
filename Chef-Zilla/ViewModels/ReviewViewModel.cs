using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chef_Zilla.ViewModels
{
    public class ReviewViewModel
    {
        public int ReviewViewModelId { get; set; }
        public int ReviewId { get; set; }
        public int OrderId { get; set; }
        public string UserId { get; set; }
        public int Ratings { get; set; }
        public string ReviewText { get; set; }
        public string UserName { get; set; }
        public int ProductID { get; set; }

    }
}