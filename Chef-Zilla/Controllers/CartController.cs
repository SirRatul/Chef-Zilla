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
    [OutputCache(NoStore = true, Duration = 0)]    //prevent duplicate form submission
    public class CartController : Controller
    {
        private ApplicationDbContext _context;        //variable to access database

        //constructor for database work
        public CartController()
        {
            _context = new ApplicationDbContext();
        }


        // GET: Cart
        public ActionResult Index()
        {
            return View();
        }

        //[HttpPost]
        public ActionResult ViewCart()
        {
            CartViewModel cart = new CartViewModel();

            var userId = User.Identity.GetUserId();
            cart.UserId = userId;

            List<string> ProductName = new List<string>();

            var productId = _context.Carts.Where(m => m.UserId == userId).Select(x => x.ProductID).ToList();

            foreach (var item in productId)
            {
                var productName = _context.Products.Where(m => m.ProductID == item).Select(x => x.ProductName).ToList();
                ProductName.Add(productName[0]);
            }

            var cartId = _context.Carts.Where(m => m.UserId == userId).Select(x => x.CartID).ToList();

            //var productquantity = _context.Carts.Where(m => m.UserId == userId &&  m.ProductID == Convert.ToInt32(productId) ).Select(x => x.ProductQuantity).ToList();

            var productquantity = _context.Carts.Where(m => m.UserId == userId).Select(x => x.ProductQuantity).ToList();

            var productPrice = _context.Carts.Where(m => m.UserId == userId).Select(x => x.totalProductPrice).ToList();

            cart.ProductQuantity = productquantity;
            cart.TotalPrice = productPrice;
            cart.ProductID = productId;
            cart.ProductName = ProductName;
            cart.CartID = cartId;



            //for billing and shipping below code


            // var firstName = _context.Users.Where(m => m.Id == userId).Select(x => x.ProductID).ToList();

            var firstName = _context.Users.Where(m => m.Id == userId).Select(x => x.FirstName).ToList();
            var lastName = _context.Users.Where(m => m.Id == userId).Select(x => x.LastName).ToList();
            var email = _context.Users.Where(m => m.Id == userId).Select(x => x.Email).ToList();
            var phone = _context.Users.Where(m => m.Id == userId).Select(x => x.PhoneNumber).ToList();

            cart.Name = firstName[0].ToString() + " " + lastName[0].ToString();
            cart.PhnNo = phone[0].Remove(0, 3);
            cart.Email = email[0].ToString();





            return View(cart);
        }

        //[HttpPost]
        //public async Task<ActionResult> AddCart(int productId, string productName, int productQuantity, int productPrice, IEnumerable<string> extraIngredientNumber)
        //{

        //    //we can store all cart in a single table. then when we need to retrive then we search with userId and fatch data with that userId.

        //    Cart cart = new Cart();

        //    var userId = @User.Identity.GetUserId();

        //    cart.UserId = userId;
        //    cart.ProductID = productId;
        //    cart.ProductName = productName;
        //    cart.ProductQuantity = productQuantity;
        //    cart.totalProductPrice = productPrice;

        //    List<int> ExtraIngredientId = new List<int>();
        //    List<int> ExtraIngredientQuantity = new List<int>();

        //    _context.Carts.Add(cart);
        //    _context.SaveChanges();
        //    int cartId = cart.CartID;

        //    foreach (var item in extraIngredientNumber)
        //    {
        //        Console.WriteLine(item);
        //        if (item != "false")
        //        {
        //            String[] splited = item.Split(' ');
        //            int extraIngredientId = Convert.ToInt32(splited[0]);
        //            int extraIngredientQuantity = Convert.ToInt32(splited[1]);
        //            ExtraIngredientId.Add(extraIngredientId);
        //            ExtraIngredientQuantity.Add(extraIngredientQuantity);
        //        }
        //    }

        //    for (int i = 0; i < ExtraIngredientId.Count; i++)
        //    {
        //        CartProductExtraItem cartProductExtraItem = new CartProductExtraItem();
        //        cartProductExtraItem.ExtraItemID = ExtraIngredientId[i];
        //        cartProductExtraItem.ExtraItemQuantity = ExtraIngredientQuantity[i];
        //        cartProductExtraItem.CartID = cartId;
        //        _context.CartProductExtraItems.Add(cartProductExtraItem);
        //        _context.SaveChanges();
        //    }

        //    //for (int i = 0; i <= extraIngredientNumber.Count(); i++)
        //    //{
        //    //    //extraIngredient[i] = extraIngredientNumber[i];

        //    //}

        //    //for (int i = 0; i < extraIngredientNumber.Count(); i++)
        //    //{
        //    //    BoxExtraItem boxExtraItem = new BoxExtraItem();
        //    //    boxExtraItem.ExtraIngredientID = ExtraIngredientId[i];
        //    //    boxExtraItem.ExtraIngredientQuantity = ExtraIngredientQuantity[i];
        //    //    boxExtraItem.BoxID = boxId;
        //    //    _context.BoxExtraItems.Add(boxExtraItem);
        //    //    _context.SaveChanges();
        //    //}

        //    //Now i have to store extraingridient to model. then update database.




        //    ///////////////////////////////////////

        //    return RedirectToAction("Index", new RouteValueDictionary(new { controller = "ItemDetails", id = productId }));
        //}






        public ActionResult Delete(int cId)
        {

            var cartid = _context.Carts.Where(m => m.CartID == cId).ToList();

            //var wishListId = _context.WishLists.Where(m => m.WishListID == wishlistID);
            _context.Carts.RemoveRange(cartid);
            _context.SaveChanges();

            TempData["message"] = "This product is deleted from your Cart.";
            return RedirectToAction("ViewCart", new RouteValueDictionary(new { controller = "Cart" }));

            //var wishListDeleteID =_context.WishLists.SingleOrDefault((m => m.WishListID == wishlistID));

            //var wishListId = _context.WishLists.Where(m => m.UserId == userId).Select(x => x.WishListID);


            //wishList.WishListID = wishListId;

            // var deleteId = new WishListViewModel { WishListID =wishListId };
            //_context.Entry(deleteId).State = EntityState.Deleted;
            //try
            //{
            //_context.SaveChanges();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    ModelState.AddModelError("",
            //      String.Format("Item with id {0} no longer exists in the database.", wishListId));
            //}



            return View();
        }


    }
}
