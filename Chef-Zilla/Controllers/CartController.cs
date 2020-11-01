using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Chef_Zilla.Models;
using Microsoft.AspNet.Identity;
using Chef_Zilla.ViewModels;

namespace Chef_Zilla.Controllers
{
    [OutputCache(NoStore = true, Duration = 0)]
    public class CartController : Controller
    {
        private ApplicationDbContext _context;

        public CartController()
        {
            _context = new ApplicationDbContext();
        }


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewCart()
        {
            CartViewModel cart = new CartViewModel();

            var userId = User.Identity.GetUserId();
            if (userId == null)
            {
                return RedirectToAction("Login", new RouteValueDictionary(new { controller = "Account", ReturnUrl = "/Cart/ViewCart" }));
            }
            cart.UserId = userId;

            List<string> ProductName = new List<string>();

            var productId = _context.Carts.Where(m => m.UserId == userId).Select(x => x.ProductID).ToList();

            foreach (var item in productId)
            {
                var productName = _context.Products.Where(m => m.ProductID == item).Select(x => x.ProductName).ToList();
                ProductName.Add(productName[0]);
            }

            var cartId = _context.Carts.Where(m => m.UserId == userId).Select(x => x.CartID).ToList();

            var productquantity = _context.Carts.Where(m => m.UserId == userId).Select(x => x.ProductQuantity).ToList();

            var productPrice = _context.Carts.Where(m => m.UserId == userId).Select(x => x.totalProductPrice).ToList();

            cart.ProductQuantity = productquantity;
            cart.TotalPrice = productPrice;
            cart.ProductID = productId;
            cart.ProductName = ProductName;
            cart.CartID = cartId;

            var firstName = _context.Users.Where(m => m.Id == userId).Select(x => x.FirstName).ToList();
            var lastName = _context.Users.Where(m => m.Id == userId).Select(x => x.LastName).ToList();
            var email = _context.Users.Where(m => m.Id == userId).Select(x => x.Email).ToList();
            var phone = _context.Users.Where(m => m.Id == userId).Select(x => x.PhoneNumber).ToList();

            cart.Name = firstName[0].ToString() + " " + lastName[0].ToString();
            cart.PhnNo = phone[0].Remove(0, 3);
            cart.Email = email[0].ToString();

            return View(cart);
        }

        public ActionResult Delete(int cId)
        {

            var cartid = _context.Carts.Where(m => m.CartID == cId).ToList();

            _context.Carts.RemoveRange(cartid);
            _context.SaveChanges();

            TempData["message"] = "This product is deleted from your Cart.";
            return RedirectToAction("ViewCart", new RouteValueDictionary(new { controller = "Cart" }));
        }

        public ActionResult ItemDetails(int cartId, int productId)
        {
            var boxItemViewModel = new BoxItemViewModel();
            var singleProduct = _context.Products.SingleOrDefault(m => m.ProductID == productId);
            var productPrice = _context.Carts.Where(m => m.ProductID == productId && m.CartID == cartId).Select(x => x.totalProductPrice).ToList();
            var productQuantity = _context.Carts.Where(m => m.ProductID == productId && m.CartID == cartId).Select(x => x.ProductQuantity).ToList();
            var ExtraIngredientSelectedId = _context.CartProductExtraItems.Where(m => m.CartID == cartId).Select(x => x.ExtraItemID).ToList();
            var ExtraIngredientSelectedQuantityInt = _context.CartProductExtraItems.Where(m => m.CartID == cartId).Select(x => x.ExtraItemQuantity).ToList();
            List<string> ExtraIngredientSelectedQuantity = ExtraIngredientSelectedQuantityInt.ConvertAll<string>(delegate (int i) { return i.ToString(); });

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
    }
}