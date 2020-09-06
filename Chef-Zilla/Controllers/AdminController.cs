using Chef_Zilla.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PagedList.Mvc;
using PagedList;

namespace Chef_Zilla.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private ApplicationDbContext _context;

        public AdminController()
        {
            _context = new ApplicationDbContext();
        }

        // GET: Admin
        public ActionResult Dashboard()
        {
            return View();
        }

        public ActionResult AddProduct()
        {
            return View();
        }

        public ActionResult ViewProduct(int id)
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

        public ActionResult EditProduct(int id)
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

        public ActionResult AllProduct()
        {
            var product = _context.Products.ToList();
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddProduct(Product model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            //string productFileName = Path.GetFileName(model.ProductImageFile.FileName);
            string productFileName = model.ProductName;
            string extension = Path.GetExtension(model.ProductImageFile.FileName);
            productFileName = productFileName + extension;
            model.ProductImageFile.SaveAs(Path.Combine(Server.MapPath("~/Images/Product/"), productFileName));
            //model.ProductImageFile.SaveAs(Path.Combine(Server.MapPath("~/Images/Product/"), productFile));
            //if (model.ProductImageFile != null)
            //{
            //    model.ProductImageFile.SaveAs(Path.Combine("~/Images/Product/", model.ProductImageFile));
            //}
            Product product = new Product();
            product.ProductType = model.ProductType;
            product.ProductName = model.ProductName;
            product.ProductImage = "~/Images/Product/"+ productFileName;
            product.PrepareTime = model.PrepareTime;
            product.ProductPrice = model.ProductPrice;
            try
            {
                _context.Products.Add(product);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            

            int productId = product.ProductID;

            for (int i = 0; i < model.IngredientName.Count; i++)
            {
                Ingredient ingredient = new Ingredient();
                ingredient.IngredientName = model.IngredientName[i];
                ingredient.IngredientQuantity = model.IngredientQuantity[i];
                ingredient.ProductID = productId;
                _context.Ingredients.Add(ingredient);
                _context.SaveChanges();
            }

            for (int i = 0; i < model.ExtraIngredientName.Count; i++)
            {
                ExtraIngredient extraIngredient = new ExtraIngredient();
                extraIngredient.ExtraIngredientName = model.ExtraIngredientName[i];
                extraIngredient.ExtraIngredientPrice = model.ExtraIngredientPrice[i];
                extraIngredient.ProductID = productId;
                _context.ExtraIngredients.Add(extraIngredient);
                _context.SaveChanges();
            }

            ModelState.AddModelError("", "Product Created Successfully.");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditProduct(Product model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            Product product = _context.Products.SingleOrDefault(m => m.ProductID == model.ProductID);

            if (model.ProductImageFile != null)
            {
                string productFileName = model.ProductName;
                string extension = Path.GetExtension(model.ProductImageFile.FileName);
                productFileName = productFileName + extension;
                model.ProductImageFile.SaveAs(Path.Combine(Server.MapPath("~/Images/Product/"), productFileName));
                product.ProductImage = "~/Images/Product/" + productFileName;
                model.ProductImage = "~/Images/Product/" + productFileName;
            }
            product.ProductType = model.ProductType;
            product.ProductName = model.ProductName;
            product.PrepareTime = model.PrepareTime;
            product.ProductPrice = model.ProductPrice;
            try
            {
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


            int productId = model.ProductID;
            var ingredient = _context.Ingredients.Where(x => x.ProductID == model.ProductID).ToList();

            if (ingredient.Count == model.IngredientName.Count)
            {
                for (int i = 0; i < model.IngredientName.Count; i++)
                {
                    ingredient[i].IngredientName = model.IngredientName[i];
                    ingredient[i].IngredientQuantity = model.IngredientQuantity[i];
                    ingredient[i].ProductID = productId;
                    _context.SaveChanges();
                }
            }

            if (ingredient.Count != model.IngredientName.Count)
            {
                for (int j = 0; j < ingredient.Count; j++)
                {
                    _context.Ingredients.Remove(ingredient[j]);
                    _context.SaveChanges();
                }
                for (int i = 0; i < model.IngredientName.Count; i++)
                {
                    Ingredient updatedIngredient = new Ingredient();
                    updatedIngredient.IngredientName = model.IngredientName[i];
                    updatedIngredient.IngredientQuantity = model.IngredientQuantity[i];
                    updatedIngredient.ProductID = productId;
                    _context.Ingredients.Add(updatedIngredient);
                    _context.SaveChanges();
                }
            }

            var extraIngredient = _context.ExtraIngredients.Where(x => x.ProductID == model.ProductID).ToList();

            if (extraIngredient.Count == model.ExtraIngredientName.Count)
            {
                for (int i = 0; i < model.ExtraIngredientName.Count; i++)
                {
                    extraIngredient[i].ExtraIngredientName = model.ExtraIngredientName[i];
                    extraIngredient[i].ExtraIngredientPrice = model.ExtraIngredientPrice[i];
                    extraIngredient[i].ProductID = productId;
                    _context.SaveChanges();
                }
            }

            if (extraIngredient.Count != model.ExtraIngredientName.Count)
            {
                for (int j = 0; j < extraIngredient.Count; j++)
                {
                    _context.ExtraIngredients.Remove(extraIngredient[j]);
                    _context.SaveChanges();
                }
                for (int i = 0; i < model.ExtraIngredientName.Count; i++)
                {
                    ExtraIngredient updatedExtraIngredient = new ExtraIngredient();
                    updatedExtraIngredient.ExtraIngredientName = model.ExtraIngredientName[i];
                    updatedExtraIngredient.ExtraIngredientPrice = model.ExtraIngredientPrice[i];
                    updatedExtraIngredient.ProductID = productId;
                    _context.ExtraIngredients.Add(updatedExtraIngredient);
                    _context.SaveChanges();
                }
            }

            ModelState.AddModelError("", "Product Updated Successfully.");
            return View(model);
        }

        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = _context.Products.SingleOrDefault(m => m.ProductID == id);
            var ingredient = _context.Ingredients.Where(x => x.ProductID == id);
            var extraIngredient = _context.ExtraIngredients.Where(x => x.ProductID == id);

            if (product == null)
            {
                TempData["message"] = "Product Not Found.";
                var productList = _context.Products.ToList();
                return View("AllProduct", productList);
            }

            _context.Products.Remove(product);
            _context.Ingredients.RemoveRange(ingredient);
            _context.ExtraIngredients.RemoveRange(extraIngredient);
            _context.SaveChanges();

            TempData["message"] = "Product Deleted Successfully.";
            var productAll = _context.Products.ToList();
            return View("AllProduct", productAll);
        }

        
    }
}