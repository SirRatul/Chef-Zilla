using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chef_Zilla.Models
{
    public class Review
    {
        public int ReviewId { get; set; }
        public List<int> OrderId { get; set; }
        public string UserId { get; set; }
        public int Ratings { get; set; }
        public string ReviewText { get; set; }
        public int ProductID { get; set; }
        public String dateTime { get; set; }
    }
}