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
using Microsoft.AspNet.Identity;
using PagedList;

namespace Chef_Zilla.Controllers
{
    [OutputCache(NoStore = true, Duration = 0)]    //prevent duplicate form submission
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

            List<string> ReviewerUserName = new List<string>();

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

            var reviewId = _context.Reviews.Where(x => x.ProductID == id)
                .Select(x => x.ReviewId).ToList();

            var reviewerUserId = _context.Reviews.Where(x => x.ProductID == id)
                .Select(x => x.UserId).ToList();

            foreach (var item in reviewerUserId)
            {
                var firstName = _context.Users.Where(x => x.Id == item)
                .Select(x => x.FirstName).ToList();
                var lastName = _context.Users.Where(x => x.Id == item)
                .Select(x => x.LastName).ToList();
                var reviewerUserName = firstName[0] + " " + lastName[0];
                ReviewerUserName.Add(reviewerUserName);
            }

            var rating = _context.Reviews.Where(x => x.ProductID == id)
                .Select(x => x.Ratings).ToList();

            var reviewText = _context.Reviews.Where(x => x.ProductID == id)
                .Select(x => x.ReviewText).ToList();

            var reviewDateTime = _context.Reviews.Where(x => x.ProductID == id)
                .Select(x => x.dateTime).ToList();

            for (int i = 0; i < reviewDateTime.Count; i++)
            {
                DateTime date = DateTime.ParseExact(reviewDateTime[i], "dd/MM/yyyy hh:mm tt", null);
                reviewDateTime[i] = date.ToString("dd MMM, yyyy : hh:mm tt");
            }

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
            product.ReviewId = reviewId;
            product.ReviewerUserName = ReviewerUserName;
            product.Ratings = rating;
            product.ReviewText = reviewText;
            product.ReviewDateTime = reviewDateTime;

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddBox(int productId, string productName, int productQuantity, int productPrice, IEnumerable<string> extraIngredientNumber, string command)
        {
            if (command.Equals("Add To Box"))
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
            else if (command.Equals("Add To Cart"))
            {
                Cart cart = new Cart();

                var userId = User.Identity.GetUserId();

                if (userId == null)
                {
                    TempData["message"] = "You have to login to add this product in cart";
                    return RedirectToAction("Index", new RouteValueDictionary(new { controller = "ItemDetails", id = productId }));
                }

                cart.UserId = userId;
                cart.ProductID = productId;
                cart.ProductName = productName;
                cart.ProductQuantity = productQuantity;
                cart.totalProductPrice = productPrice;

                List<int> ExtraIngredientId = new List<int>();
                List<int> ExtraIngredientQuantity = new List<int>();

                
                //chek if the product is already in cart or not
                var checkDuplicate = _context.Carts.Where(m => m.ProductID == productId && m.UserId == userId).Select(x => x.ProductID).ToList();

                if(checkDuplicate.Count>=1)
                {
                    TempData["message"] = "Item is already in CART.";
                    return RedirectToAction("Index", new RouteValueDictionary(new { controller = "ItemDetails", id = productId }));

                }
                else
                {
                    _context.Carts.Add(cart);
                    _context.SaveChanges();
                    int cartId = cart.CartID;

                    foreach (var item in extraIngredientNumber)
                    {
                        Console.WriteLine(item);
                        if (item != "false")
                        {
                            String[] splited = item.Split(' ');
                            int extraIngredientId = Convert.ToInt32(splited[0]);
                            int extraIngredientQuantity = Convert.ToInt32(splited[1]);
                            ExtraIngredientId.Add(extraIngredientId);
                            ExtraIngredientQuantity.Add(extraIngredientQuantity);
                        }
                    }

                    for (int i = 0; i < ExtraIngredientId.Count; i++)
                    {
                        CartProductExtraItem cartProductExtraItem = new CartProductExtraItem();
                        cartProductExtraItem.ExtraItemID = ExtraIngredientId[i];
                        cartProductExtraItem.ExtraItemQuantity = ExtraIngredientQuantity[i];
                        cartProductExtraItem.CartID = cartId;
                        _context.CartProductExtraItems.Add(cartProductExtraItem);
                        _context.SaveChanges();
                    }
                    TempData["message"] = "Item succesfully added in CART.";
                    return RedirectToAction("Index", new RouteValueDictionary(new { controller = "ItemDetails", id = productId }));
                }

                
            }
            return RedirectToAction("Index", new RouteValueDictionary(new { controller = "ItemDetails", id = productId }));
        }
    }
}