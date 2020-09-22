using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Chef_Zilla.Models;
using PagedList;

namespace Chef_Zilla.Controllers
{
    public class GeneralItemController : Controller
    {
        private ApplicationDbContext _context;

        public GeneralItemController()
        {
            _context = new ApplicationDbContext();
        }
        // GET: GeneralItem
        public ActionResult Index(int? page)
        {
            var product = _context.Products.Where(m => m.ProductType == "General").ToList().ToPagedList(page ?? 1, 9);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Search(string product, int? page)
        {
            List<Product> products;
            if (!string.IsNullOrEmpty(product))
            {
                products = _context.Products.Where(m => m.ProductName.Contains(product) && m.ProductType == "General").ToList();
            }
            else
            {
                products = _context.Products.ToList();
            }
            return View("Index", products.ToPagedList(page ?? 1, 1));
        }
    }
}