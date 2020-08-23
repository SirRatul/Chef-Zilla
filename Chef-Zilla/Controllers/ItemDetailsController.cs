using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Chef_Zilla.Models;

namespace Chef_Zilla.Controllers
{
    public class ItemDetailsController : Controller
    {
        private ApplicationDbContext _context;

        public ItemDetailsController()
        {
            _context = new ApplicationDbContext();
        }
        
        // GET: ItemDetails
        public ActionResult Index(int id)
        {
            var product = new Product();
            var singleProduct = _context.Products.SingleOrDefault(m => m.ProductID == id);
            product = singleProduct;

            var ingredientName = _context.Ingredients.Where(x => x.ProductID == id)
                .Select(x => x.IngredientName).ToList();

            var ingredientQuantity = _context.Ingredients.Where(x => x.ProductID == id)
                .Select(x => x.IngredientQuantity).ToList();

            var extraIngredientName = _context.ExtraIngredients.Where(x => x.ProductID == id)
                .Select(x => x.ExtraIngredientName).ToList();

            var extraIngredientPrice = _context.ExtraIngredients.Where(x => x.ProductID == id)
                .Select(x => x.ExtraIngredientPrice).ToList();

            product.IngredientName = ingredientName;
            product.IngredientQuantity = ingredientQuantity;
            product.ExtraIngredientName = extraIngredientName;
            product.ExtraIngredientPrice = extraIngredientPrice;


            return View(product);
        }
    }
}