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

        private ApplicationDbContext _context;        //variable to access database

        //constructor for database work
        public ReviewController()
        {
            _context = new ApplicationDbContext();
        }

        // GET: Review
        [HttpPost]
        public ActionResult Index(string reviewText, int Ratings, int productId)
        {

            Review review = new Review();
            ReviewViewModel reviewViewModel = new ReviewViewModel();

            string dateTime = DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");



            var userId = User.Identity.GetUserId();

            List<int> FProductId = new List<int>();
            var findOrderId = _context.Orders.Where(m => m.UserId == userId).Select(x=>x.OrderId).ToList();
            //FProductId = findOrderId[0];
            //int findOrderID2 = _context.Orders.SingleOrDefault(m => m.UserId == userId).Select(x => x.OrderId);

            bool productDetected = false;
            if(findOrderId.Count>0)
            {
                foreach (var item in findOrderId)
                {
                    var findProductId = _context.OrderProducts.Where(m => m.OrderId == item).Select(x => x.ProductID).ToList();
                    if(findProductId.Count > 0)
                    {
                        productDetected = true;
                        break;
                    }
                }
                /*for (int i = 0; i < findOrderId.Count; i++)
                {
                    var findProductId = _context.OrderProducts.Where(m => m.OrderId == findOrderId[i]).Select(x => x.ProductID).ToList();

                    productDetected = true;

                }*/
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

            //var findProductId = _context.OrderProducts.Where(m => m.OrderId == Convert.ToInt32( findOrderId)).Select(x => x.ProductID).ToList();



            //return View(review);
            return RedirectToAction("Index", new RouteValueDictionary(new { controller = "ItemDetails", id = productId }));
        }
    }
}