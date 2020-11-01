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
    [OutputCache(NoStore = true, Duration = 0)]
    public class WishListController : Controller
    {
        private ApplicationDbContext _context; 

        public WishListController()
        {
            _context = new ApplicationDbContext();
        }

        
        public ActionResult Create(WishList model)
        {
            WishList wishList = new WishList();

            if (model.UserId == null)
            {
                TempData["message"] = "You have to login to add this product in wishlist";
                return RedirectToAction("Index", new RouteValueDictionary(new { controller = "ItemDetails", id = model.ProductID }));
            }

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
        }

        public ActionResult Details()
        {
             WishListViewModel wishList = new WishListViewModel();

             var userId = User.Identity.GetUserId();
             if (userId == null)
             {
                 return RedirectToAction("Login", new RouteValueDictionary(new { controller = "Account", ReturnUrl = "/WishList/Details" }));
             }

            wishList.UserId = userId;
            List<string> ProductName = new List<string>();
            var productId = _context.WishLists.Where(m => m.UserId == userId).Select(x => x.ProductID).ToList();
            var wishListId = _context.WishLists.Where(m => m.UserId == userId).Select(x => x.WishListID).ToList();

            foreach (var item in productId)
            {
                var productName = _context.Products.Where(m => m.ProductID == item).Select(x => x.ProductName).ToList();
                ProductName.Add(productName[0]);
            }
            wishList.WishListID = wishListId;
            wishList.ProductID = productId;
            wishList.ProductName = ProductName;
            return View(wishList);
        }
        public ActionResult Delete(int wId)
        {

            var wishList = _context.WishLists.Where(m => m.WishListID == wId).ToList();
            _context.WishLists.RemoveRange(wishList);
            _context.SaveChanges();

            TempData["message"] = "This product is deleted from your wishlist.";
            return RedirectToAction("Details", new RouteValueDictionary(new { controller = "WishList"}));
        }
    }
}