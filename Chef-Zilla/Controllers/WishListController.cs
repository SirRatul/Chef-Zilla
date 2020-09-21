using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Chef_Zilla.Models;
using Chef_Zilla.ViewModels;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;

namespace Chef_Zilla.Controllers
{
    [Authorize]
    [OutputCache(NoStore = true, Duration = 0)]    //prevent duplicate form submission
    public class WishListController : Controller


    {

        private ApplicationDbContext _context;        //variable to access database

        //constructor of wishlistController for database work
        public WishListController()
        {
            _context = new ApplicationDbContext();
        }

        // GET: WishList
        
        public ActionResult Create(WishList model)
        {
            WishList wishList = new WishList();

            wishList.UserId = model.UserId;
            wishList.ProductID = model.ProductID;

            var checkProductID = _context.WishLists.Where(m => m.ProductID == model.ProductID && m.UserId == model.UserId).Select(x => x.ProductID).ToList();

            if (checkProductID.Count >= 1)
            {
                TempData["message"] = "You already added this product in your wishlist.";
                return RedirectToAction("Index", new RouteValueDictionary(new { controller = "ItemDetails", id = model.ProductID }));
            }
            else
            {
                _context.WishLists.Add(wishList);
                _context.SaveChanges();
                int WishListID = model.WishListID;

                TempData["message"] = "This product is added in your wishlist.";
                return RedirectToAction("Index", new RouteValueDictionary(new { controller = "ItemDetails", id = model.ProductID }));
            }

             

            //return RedirectToAction("Details");
            //return RedirectToAction("Details", "WishList");
            //return View();
        }

        public ActionResult Details()
        {
            //var allWishList = _context.WishLists;
             WishListViewModel wishList = new WishListViewModel();

            var userId = User.Identity.GetUserId();
            wishList.UserId = userId;
            List<string> ProductName = new List<string>();
            var productId = _context.WishLists.Where(m => m.UserId == userId).Select(x => x.ProductID).ToList();
            var wishListId = _context.WishLists.Where(m => m.UserId == userId).Select(x => x.WishListID).ToList();

            foreach (var item in productId)
            {
                var productName = _context.Products.Where(m => m.ProductID == item).Select(x => x.ProductName).ToList();
                ProductName.Add(productName[0]);
            }
            //var productList = _context.Products.Where(m => m.ProductID == userId).Select(x => x).ToList();
            wishList.WishListID = wishListId;
            wishList.ProductID = productId;
            wishList.ProductName = ProductName;
            return View(wishList);
        }
        public ActionResult Delete(int wId)
        {

            var wishList = _context.WishLists.Where(m => m.WishListID == wId).ToList();

            //var wishListId = _context.WishLists.Where(m => m.WishListID == wishlistID);
            _context.WishLists.RemoveRange(wishList);
            _context.SaveChanges();

            TempData["message"] = "This product is deleted from your wishlist.";
            return RedirectToAction("Details", new RouteValueDictionary(new { controller = "WishList"}));

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