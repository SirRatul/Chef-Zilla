using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chef_Zilla.Models
{
    public class WishList
    {
        public int WishListID { get; set; }
        public int ProductID { get; set; }
        public string UserId { get; set; }
    }
}