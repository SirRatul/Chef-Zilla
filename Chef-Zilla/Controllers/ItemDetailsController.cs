using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Chef_Zilla.Models;
using Chef_Zilla.ViewModels;
using PagedList;

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
            var product = new ItemDetailsViewModels();
            var singleProduct = _context.Products.SingleOrDefault(m => m.ProductID == id);
            //product = singleProduct;

            var ingredientName = _context.Ingredients.Where(x => x.ProductID == id)
                .Select(x => x.IngredientName).ToList();

            var ingredientQuantity = _context.Ingredients.Where(x => x.ProductID == id)
                .Select(x => x.IngredientQuantity).ToList();

            var extraIngredientId = _context.ExtraIngredients.Where(x => x.ProductID == id)
                .Select(x => x.ExtraIngredientID).ToList();

            var extraIngredientName = _context.ExtraIngredients.Where(x => x.ProductID == id)
                .Select(x => x.ExtraIngredientName).ToList();

            var extraIngredientPrice = _context.ExtraIngredients.Where(x => x.ProductID == id)
                .Select(x => x.ExtraIngredientPrice).ToList();

            product.ProductID = id;
            product.ProductType = singleProduct.ProductType;
            product.ProductName = singleProduct.ProductName;
            product.ProductImage = singleProduct.ProductImage;
            product.PrepareTime = singleProduct.PrepareTime;
            product.ProductPrice = singleProduct.ProductPrice;
            product.IngredientName = ingredientName;
            product.IngredientQuantity = ingredientQuantity;
            product.ExtraIngredientId = extraIngredientId;
            product.ExtraIngredientName = extraIngredientName;
            product.ExtraIngredientPrice = extraIngredientPrice;

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddBox(int productId, string productName, int productQuantity, int productPrice, IEnumerable<string> extraIngredientNumber)
        {
            if (Session["productId"] == null)
            {
                List<int> ProductId = new List<int>();
                List<string> ProductName = new List<string>();
                List<int> ProductQuantity = new List<int>();
                List<int> ProductPrice = new List<int>();
                List<int> ExtraIngredientId = new List<int>();
                List<string> ExtraIngredientQuantity = new List<string>();
                ProductId.Add(productId);
                ProductName.Add(productName);
                ProductQuantity.Add(productQuantity);
                ProductPrice.Add(productPrice);
                foreach (var item in extraIngredientNumber)
                {
                    if (item != "false")
                    {
                        String[] splited = item.Split(' ');
                        int extraIngredientId = Convert.ToInt32(splited[0]);
                        string extraIngredientQuantity = splited[1];
                        ExtraIngredientId.Add(extraIngredientId);
                        ExtraIngredientQuantity.Add(extraIngredientQuantity);
                    }
                }
                Session["productId"] = ProductId;
                Session["productName"] = ProductName;
                Session["productQuantity"] = ProductQuantity;
                Session["productPrice"] = ProductPrice;
                Session["extraIngredientId"] = ExtraIngredientId;
                Session["extraIngredientQuantity"] = ExtraIngredientQuantity;
                TempData["message"] = "Item succesfully added in Box.";
            }
            else
            {
                List<int> ProductId = (List<int>)Session["productId"];
                List<string> ProductName = (List<string>)Session["productName"];
                List<int> ProductQuantity = (List<int>)Session["productQuantity"];
                List<int> ProductPrice = (List<int>)Session["productPrice"];
                List<int> ExtraIngredientId = (List<int>)Session["extraIngredientId"];
                List<string> ExtraIngredientQuantity = (List<string>)Session["extraIngredientQuantity"];
                int duplicate = 0;
                foreach (int item in ProductId.ToList())
                {
                    if (item == productId)
                    {
                        duplicate = 1;
                    }
                }

                if (duplicate == 0)
                {
                    ProductId.Add(productId);
                    ProductName.Add(productName);
                    ProductQuantity.Add(productQuantity);
                    ProductPrice.Add(productPrice);
                    foreach (var item in extraIngredientNumber)
                    {
                        if (item != "false")
                        {
                            String[] splited = item.Split(' ');
                            int extraIngredientId = Convert.ToInt32(splited[0]);
                            string extraIngredientQuantity = splited[1];
                            ExtraIngredientId.Add(extraIngredientId);
                            ExtraIngredientQuantity.Add(extraIngredientQuantity);
                        }
                    }
                    TempData["message"] = "Item succesfully added in Box.";
                }
                if (duplicate == 1)
                {
                    TempData["message"] = "Item already added in Box.";
                }
                //ProductId.Add(productId);
                Session["productId"] = ProductId;
                Session["productName"] = ProductName;
                Session["productQuantity"] = ProductQuantity;
                Session["productPrice"] = ProductPrice;
                Session["extraIngredientId"] = ExtraIngredientId;
                Session["extraIngredientQuantity"] = ExtraIngredientQuantity;
            }
            return RedirectToAction("Index", new RouteValueDictionary(new { controller = "ItemDetails", id = productId }));
        }
    }
}