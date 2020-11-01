using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Chef_Zilla.Models;
using Chef_Zilla.ViewModels;
using Microsoft.AspNet.Identity;

namespace Chef_Zilla.Controllers
{
    [Authorize]
    [OutputCache(NoStore = true, Duration = 0)]
    public class BoxController : Controller
    {
        // GET: Box
        private ApplicationDbContext _context;

        public BoxController()
        {
            _context = new ApplicationDbContext();
        }
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Details(int id)
        {
            var boxDetailsViewModel = new BoxDetailsViewModel();
            var singleBox = _context.Boxes.SingleOrDefault(m => m.BoxID == id);
            var productId = _context.BoxProducts.Where(x => x.BoxID == id)
                .Select(x => x.ProductID).ToList();
            List<string> ProductName = new List<string>();

            foreach (var item in productId)
            {
                var singleProduct = _context.Products.SingleOrDefault(m => m.ProductID == item);
                ProductName.Add(singleProduct.ProductName);
            }

            var productQuantity = _context.BoxProducts.Where(x => x.BoxID == id)
                .Select(x => x.ProductQuantity).ToList();
            var totalPrice = _context.BoxProducts.Where(x => x.BoxID == id)
                .Select(x => x.TotalPrice).ToList();

            boxDetailsViewModel.BoxID = id;
            boxDetailsViewModel.BoxName = singleBox.BoxName;
            boxDetailsViewModel.ProductID = productId;
            boxDetailsViewModel.ProductName = ProductName;
            boxDetailsViewModel.ProductQuantity = productQuantity;
            boxDetailsViewModel.TotalPrice = totalPrice;
            return View(boxDetailsViewModel);
        }

        public ActionResult ItemView(int id)
        {
            var boxItemViewModel = new BoxItemViewModel();
            List<int> ProductId = (List<int>)Session["productId"];
            List<int> ProductPrice = (List<int>)Session["productPrice"];
            List<int> ProductQuantity = (List<int>)Session["productQuantity"];
            List<int> ExtraIngredientId = (List<int>)Session["extraIngredientId"];
            List<string> ExtraIngredientQuantity = (List<string>)Session["extraIngredientQuantity"];
            List<int> ExtraIngredientSelectedId = new List<int>();
            List<string> ExtraIngredientSelectedQuantity = new List<string>();
            int pricePositionOfList = 0;
            foreach (var item in ProductId)
            {
                int position = ProductId.IndexOf(id);
                if (position != -1)
                {
                    pricePositionOfList = position;
                }
                Console.WriteLine(position);
            }

            var singleProduct = _context.Products.SingleOrDefault(m => m.ProductID == id);
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

            foreach (var item in extraIngredientId)
            {
                int position = ExtraIngredientId.IndexOf(item);
                if (position != -1)
                {
                    ExtraIngredientSelectedId.Add(item);
                }
                Console.WriteLine(position);
            }

            for (int i = 0; i < ExtraIngredientSelectedId.Count; i++)
            {
                int position = ExtraIngredientId.IndexOf(ExtraIngredientSelectedId[i]);
                if (position != -1)
                {
                    ExtraIngredientSelectedQuantity.Add(ExtraIngredientQuantity[position]);
                }
                Console.WriteLine(position);
            }

            boxItemViewModel.ProductName = singleProduct.ProductName;
            boxItemViewModel.ProductImage = singleProduct.ProductImage;
            boxItemViewModel.PrepareTime = singleProduct.PrepareTime;
            boxItemViewModel.ProductPrice = ProductPrice[pricePositionOfList];
            boxItemViewModel.ProductQuantity = ProductQuantity[pricePositionOfList];
            boxItemViewModel.IngredientName = ingredientName;
            boxItemViewModel.IngredientQuantity = ingredientQuantity;
            boxItemViewModel.ExtraIngredientId = extraIngredientId;
            boxItemViewModel.ExtraIngredientName = extraIngredientName;
            boxItemViewModel.ExtraIngredientPrice = extraIngredientPrice;
            boxItemViewModel.ExtraIngredientSelecetedID = ExtraIngredientSelectedId;
            boxItemViewModel.ExtraIngredientSelecetedQuantity = ExtraIngredientSelectedQuantity;
            return View(boxItemViewModel);
        }

        public ActionResult ItemDetails(int boxId, int productId)
        {
            var boxItemViewModel = new BoxItemViewModel();
            var singleProduct = _context.Products.SingleOrDefault(m => m.ProductID == productId);
            var productPrice = _context.BoxProducts.Where(m => m.ProductID == productId && m.BoxID == boxId).Select(x => x.TotalPrice).ToList();
            var productQuantity = _context.BoxProducts.Where(m => m.ProductID == productId && m.BoxID == boxId).Select(x => x.ProductQuantity).ToList();
            var ExtraIngredientSelectedId = _context.BoxExtraItems.Where(m => m.BoxID == boxId).Select(x => x.ExtraIngredientID).ToList();
            var ExtraIngredientSelectedQuantity = _context.BoxExtraItems.Where(m => m.BoxID == boxId).Select(x => x.ExtraIngredientQuantity).ToList();
            var ingredientName = _context.Ingredients.Where(x => x.ProductID == productId)
                .Select(x => x.IngredientName).ToList();

            var ingredientQuantity = _context.Ingredients.Where(x => x.ProductID == productId)
                .Select(x => x.IngredientQuantity).ToList();

            var extraIngredientId = _context.ExtraIngredients.Where(x => x.ProductID == productId)
                .Select(x => x.ExtraIngredientID).ToList();

            var extraIngredientName = _context.ExtraIngredients.Where(x => x.ProductID == productId)
                .Select(x => x.ExtraIngredientName).ToList();

            var extraIngredientPrice = _context.ExtraIngredients.Where(x => x.ProductID == productId)
                .Select(x => x.ExtraIngredientPrice).ToList();
            boxItemViewModel.ProductName = singleProduct.ProductName;
            boxItemViewModel.ProductImage = singleProduct.ProductImage;
            boxItemViewModel.PrepareTime = singleProduct.PrepareTime;
            boxItemViewModel.ProductPrice = productPrice[0];
            boxItemViewModel.ProductQuantity = productQuantity[0];
            boxItemViewModel.IngredientName = ingredientName;
            boxItemViewModel.IngredientQuantity = ingredientQuantity;
            boxItemViewModel.ExtraIngredientId = extraIngredientId;
            boxItemViewModel.ExtraIngredientName = extraIngredientName;
            boxItemViewModel.ExtraIngredientPrice = extraIngredientPrice;
            boxItemViewModel.ExtraIngredientSelecetedID = ExtraIngredientSelectedId;
            boxItemViewModel.ExtraIngredientSelecetedQuantity = ExtraIngredientSelectedQuantity;
            return View(boxItemViewModel);
        }

        public async Task<ActionResult> RemoveList(int productId, string productName, int productQuantity, int productPrice)
        {
            List<int> ProductId = (List<int>)Session["productId"];
            List<string> ProductName = (List<string>)Session["productName"];
            List<int> ProductQuantity = (List<int>)Session["productQuantity"];
            List<int> ProductPrice = (List<int>)Session["productPrice"];
            List<int> ExtraIngredientId = (List<int>)Session["extraIngredientId"];
            List<string> ExtraIngredientQuantity = (List<string>)Session["extraIngredientQuantity"];
            var extraIngredientId = _context.ExtraIngredients.Where(x => x.ProductID == productId)
                .Select(x => x.ExtraIngredientID).ToList();
            foreach (var item in extraIngredientId)
            {
                int position = ExtraIngredientId.IndexOf(item);
                if (position != -1)
                {
                    ExtraIngredientId.RemoveAt(position);
                    ExtraIngredientQuantity.RemoveAt(position);
                }
                Console.WriteLine(position);
            }
            ProductId.Remove(productId);
            ProductName.Remove(productName);
            ProductQuantity.Remove(productQuantity);
            ProductPrice.Remove(productPrice);
            Session["productId"] = ProductId;
            Session["productName"] = ProductName;
            Session["productQuantity"] = ProductQuantity;
            Session["productPrice"] = ProductPrice;
            Session["extraIngredientId"] = ExtraIngredientId;
            Session["extraIngredientQuantity"] = ExtraIngredientQuantity;
            return RedirectToAction("Create", new RouteValueDictionary(new {controller = "Box"}));
        }

        public async Task<ActionResult> RemoveProductFromBox(int productId, int boxId)
        {
            var product = _context.BoxProducts.Where(m => m.ProductID == productId && m.BoxID == boxId).ToList();
            var ProductId = _context.BoxProducts.Where(x => x.BoxID == boxId)
                .Select(x => x.ProductID).ToList();
            if (ProductId.Count == 1)
            {
                TempData["message"] = "You only have this item in your box.You have to keep at least one item in a box.";
                return RedirectToAction("Details", new RouteValueDictionary(new { controller = "Box", id = boxId }));
            }
            var extraIngredientId = _context.ExtraIngredients.Where(m => m.ProductID == productId).Select(x => x.ExtraIngredientID).ToList();
            foreach (var item in extraIngredientId)
            {
                var extraIngredient = _context.BoxExtraItems.Where(x => x.BoxID == boxId && x.ExtraIngredientID == item)
                    .ToList();
                _context.BoxExtraItems.RemoveRange(extraIngredient);
                _context.SaveChanges();
            }
            _context.BoxProducts.RemoveRange(product);
            _context.SaveChanges();
            TempData["message"] = "Item Successfully Removed From your box.";
            return RedirectToAction("Details", new RouteValueDictionary(new { controller = "Box", id = boxId }));
        }

        public async Task<ActionResult> DeleteBox(int boxId)
        {
            var box = _context.Boxes.Where(m => m.BoxID == boxId).ToList();
            _context.Boxes.RemoveRange(box);
            var routineNumber = _context.Routines.Where(x => x.BoxID == boxId)
                .Select(x => x.RoutineID).ToList();
            if (routineNumber.Count > 0)
            {
                TempData["message"] = "You can not delete this box because You already create a food plan with this box.";
                return RedirectToAction("Details", new RouteValueDictionary(new { controller = "Box", id = boxId }));
            }
            var boxProduct = _context.BoxProducts.Where(m => m.BoxID == boxId).ToList();
            var boxExtraItem = _context.BoxExtraItems.Where(m => m.BoxID == boxId).ToList();
            _context.Boxes.RemoveRange(box);
            _context.BoxProducts.RemoveRange(boxProduct);
            _context.BoxExtraItems.RemoveRange(boxExtraItem);
            _context.SaveChanges();
            TempData["message"] = "Box Deleted Successfully.";
            return RedirectToAction("Index", "FoodPlan");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(BoxCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (Session["productId"] == null && Session["productName"] == null && Session["productQuantity"] == null && Session["productPrice"] == null && Session["extraIngredientId"] == null && Session["extraIngredientQuantity"] == null)
            {
                ModelState.AddModelError("", "Please select product to create a box.");
            }
            else
            {
                List<int> ProductId = (List<int>) (Session["productId"]);
                List<string> ProductName = (List<string>)Session["productName"]; 
                List<int> ProductQuantity = (List<int>) Session["productQuantity"];
                List<int> ProductPrice = (List<int>) Session["productPrice"];
                List<int> ExtraIngredientId = (List<int>) Session["extraIngredientId"];
                List<string> ExtraIngredientQuantity = (List<string>) Session["extraIngredientQuantity"];
                Box box = new Box();
                box.BoxName = model.BoxName;
                box.UserId = model.UserId;
                _context.Boxes.Add(box);
                _context.SaveChanges();
                int boxId = box.BoxID;

                for (int i = 0; i < ProductId.Count; i++)
                {
                    BoxProduct boxProduct = new BoxProduct();
                    boxProduct.ProductID = ProductId[i];
                    boxProduct.ProductQuantity = ProductQuantity[i];
                    boxProduct.TotalPrice = ProductPrice[i];
                    boxProduct.BoxID = boxId;
                    _context.BoxProducts.Add(boxProduct);
                    _context.SaveChanges();
                }

                for (int i = 0; i < ExtraIngredientId.Count; i++)
                {
                    BoxExtraItem boxExtraItem = new BoxExtraItem();
                    boxExtraItem.ExtraIngredientID = ExtraIngredientId[i];
                    boxExtraItem.ExtraIngredientQuantity = ExtraIngredientQuantity[i];
                    boxExtraItem.BoxID = boxId;
                    _context.BoxExtraItems.Add(boxExtraItem);
                    _context.SaveChanges();
                }
                ProductId.Clear();
                ProductName.Clear();
                ProductQuantity.Clear();
                ProductPrice.Clear();
                ExtraIngredientId.Clear();
                ExtraIngredientQuantity.Clear();
                Session["productId"] = ProductId;
                Session["productName"] = ProductName;
                Session["productQuantity"] = ProductQuantity;
                Session["productPrice"] = ProductPrice;
                Session["extraIngredientId"] = ExtraIngredientId;
                Session["extraIngredientQuantity"] = ExtraIngredientQuantity;
                ModelState.AddModelError("", "Box Created Successfully.");
            }
            return View();
        }
    }
}