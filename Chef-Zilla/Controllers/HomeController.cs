using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Chef_Zilla.Models;
using PagedList;

namespace Chef_Zilla.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext _context;

        public HomeController()
        {
            _context = new ApplicationDbContext();
        }
        public ActionResult Index()
        {
            var product = _context.Products.Where(m => m.ProductType == "Featured").ToList();
            return View(product);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}