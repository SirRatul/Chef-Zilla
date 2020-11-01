using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Chef_Zilla.Models;
using Chef_Zilla.ViewModels;
using Microsoft.AspNet.Identity;

namespace Chef_Zilla.Controllers
{
    public class ReviewController : Controller
    {

        private ApplicationDbContext _context;

        public ReviewController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpPost]
        public ActionResult Index(string reviewText, int Ratings, int productId)
        {
            var userId = User.Identity.GetUserId();
            if (userId == null)
            {
                TempData["message"] = "You have to login to review this product";
                return RedirectToAction("Index", new RouteValueDictionary(new { controller = "ItemDetails", id = productId }));
            }

            Review review = new Review();

            string dateTime = DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");

            var findOrderId = _context.Orders.Where(m => m.UserId == userId).Select(x=>x.OrderId).ToList();

            bool productDetected = false;
            if(findOrderId.Count>0)
            {
                foreach (var item in findOrderId)
                {
                    var findProductId = _context.OrderProducts.Where(m => m.OrderId == item).Select(x => x.ProductID).ToList();
                    if(findProductId.Count > 0)
                    {
                        foreach (var item1 in findProductId)
                        {
                            if (item1 == productId)
                            {
                                productDetected = true;
                                break;
                            }
                        }

                    }
                }
            }

            if(!productDetected)
            {
                TempData["message"] = "You can not review until you order this product";
                return RedirectToAction("Index", new RouteValueDictionary(new { controller = "ItemDetails", id = productId }));
            }
            if(productDetected)
            {
                TempData["message"] = "Your review has been added successfully";

                review.UserId = userId;
                review.Ratings = Ratings;
                review.ReviewText = reviewText;
                review.ProductID = productId;
                review.dateTime = dateTime;
            }

            _context.Reviews.Add(review);
            _context.SaveChanges();

            return RedirectToAction("Index", new RouteValueDictionary(new { controller = "ItemDetails", id = productId }));
        }
    }
}