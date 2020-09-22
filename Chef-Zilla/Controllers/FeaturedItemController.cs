using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Chef_Zilla.Models;
using PagedList.Mvc;
using PagedList;

namespace Chef_Zilla.Controllers
{
    public class FeaturedItemController : Controller
    {
        private ApplicationDbContext _context;

        public FeaturedItemController()
        {
            _context = new ApplicationDbContext();
        }

        // GET: FeaturedItem
        public ActionResult Index(int? page)
        {
            var product = _context.Products.Where(m => m.ProductType == "Featured").ToList().ToPagedList(page ?? 1, 9);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Search(string product, int? page)
        {
            List<Product> products;
            if (!string.IsNullOrEmpty(product))
            {
                products = _context.Products.Where(m => m.ProductName.Contains(product) && m.ProductType == "Featured").ToList();
            }
            else
            {
                products = _context.Products.ToList();
            }
            return View("Index", products.ToPagedList(page ?? 1, 1));
        }
    }
}