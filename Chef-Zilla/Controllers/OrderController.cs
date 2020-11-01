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
    [Authorize]
    [OutputCache(NoStore = true, Duration = 0)]
    public class OrderController : Controller
    {
        private ApplicationDbContext _context;
        public OrderController()
        {
            _context = new ApplicationDbContext();
        }

        public ActionResult FinalOrder()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string address,int totalItem, int finalTotalPrice, List<int> ProductID, List<int> ProductQuantity, List<int> TotalPrice, string dateTime)
        {
            if (totalItem == 0)
            {
                TempData["message"] = "You have no Item for Order.";
                return RedirectToAction("ViewCart", "Cart");
            }
            List<int> extraIngrediantID = new List<int>();
            List<int> extraIngrediantQuantity = new List<int>();


            Order order = new Order();

            CartViewModel cart = new CartViewModel();

           
            var userId = User.Identity.GetUserId();
            order.UserId = userId;

            var FinalTotalPrice = finalTotalPrice;
            order.finalTotalPrice = Convert.ToInt32(FinalTotalPrice) ;

            order.dateTime = dateTime;

            var Address = address;
            cart.Address = Address;

            var TotalItem = totalItem;
            order.totalItem = TotalItem;

            var cartId = _context.Carts.Where(m => m.UserId == userId).Select(x => x.CartID).ToList();

            foreach (var item in cartId)
            {
                var cartExtraItemId = _context.CartProductExtraItems.Where(m => m.CartID == item).Select(x => x.ExtraItemID).ToList();
                if(cartExtraItemId.Count > 0)
                {
                    extraIngrediantID.Add(cartExtraItemId[0]);
                }
                var cartExtraQuantity = _context.CartProductExtraItems.Where(m => m.CartID == item).Select(x => x.ExtraItemQuantity).ToList();
                if (cartExtraQuantity.Count > 0)
                {
                    extraIngrediantQuantity.Add(cartExtraQuantity[0]);
                }
            }

            
            cart.ProductID = ProductID;
            cart.ProductQuantity = ProductQuantity;
            cart.TotalPrice = TotalPrice;
            cart.FinalTotalPrice = finalTotalPrice;
            cart.extraIngrediantID = extraIngrediantID;
            cart.extraIngrediantQuantity = extraIngrediantQuantity;


            

            return View("FinalOrder", cart);
        }

        public ActionResult AddOrder(int FinalTotalPrice, string address,int TotalItem, string dateTime)
        {
            List<CartProductExtraItem> cartExtraIngrediant = new List<CartProductExtraItem>();
            List<Cart> cartMain = new List<Cart>();
            List<int> cartID = new List<int>();

            Order order = new Order();
            OrderProductExtraItem orderProductExtraItem = new OrderProductExtraItem();
            OrderProduct orderProduct = new OrderProduct();

            CartViewModel cart = new CartViewModel();


            var userId = User.Identity.GetUserId();
            order.UserId = userId;

            order.finalTotalPrice = Convert.ToInt32(FinalTotalPrice);
            

            order.Address = address;
            order.dateTime = dateTime;

            order.totalItem = TotalItem;
            order.status = "pending";
            order.Type = "Regular";

            var cartId = _context.Carts.Where(m => m.UserId == userId).Select(x => x.CartID).ToList();

            foreach (var item in cartId)
            {
                var cartItem = _context.Carts.Where(m => m.CartID == item).ToList();
                var cartExtraItem = _context.CartProductExtraItems.Where(m => m.CartID == item).ToList();
                foreach (var extraItem in cartExtraItem)
                {
                    cartExtraIngrediant.Add(extraItem);
                }
                foreach (var mainItem in cartItem)
                {
                    cartMain.Add(mainItem);
                }
            }
            _context.Orders.Add(order);
            _context.SaveChanges();

            int orderId = order.OrderId;
            foreach (var item in cartId)
            {
                var cartProductId = _context.Carts.Where(m => m.CartID == item).Select(x => x.ProductID).ToList();
                if (cartProductId.Count > 0)
                {
                    orderProduct.ProductID = cartProductId[0];
                }
                var cartProductQuantity = _context.Carts.Where(m => m.CartID == item).Select(x => x.ProductQuantity).ToList();
                if (cartProductQuantity.Count > 0)
                {
                    orderProduct.ProductQuantity = cartProductQuantity[0];
                }
                var cartProductPrice = _context.Carts.Where(m => m.CartID == item).Select(x => x.totalProductPrice).ToList();
                if (cartProductPrice.Count > 0)
                {
                    orderProduct.ProductPrice = cartProductPrice[0];
                }
                orderProduct.OrderId = orderId;
                _context.OrderProducts.Add(orderProduct);
                _context.SaveChanges();
            }

            foreach (var item in cartId)
            {
                var cartExtraItemId = _context.CartProductExtraItems.Where(m => m.CartID == item).Select(x => x.ExtraItemID).ToList();
                var cartExtraQuantity = _context.CartProductExtraItems.Where(m => m.CartID == item).Select(x => x.ExtraItemQuantity).ToList();
                if (cartExtraItemId.Count > 0)
                {
                    for(var i = 0; i < cartExtraItemId.Count; i++)
                    {
                        orderProductExtraItem.ExtraItemID = cartExtraItemId[i];
                        orderProductExtraItem.ExtraItemQuantity = cartExtraQuantity[i];
                        orderProductExtraItem.OrderId = orderId;
                        _context.OrderProductExtraItems.Add(orderProductExtraItem);
                        _context.SaveChanges();
                    }
                }

            }

            _context.Carts.RemoveRange(cartMain);
            _context.CartProductExtraItems.RemoveRange(cartExtraIngrediant);
            _context.SaveChanges();

            return RedirectToAction("ViewOrderList", new RouteValueDictionary(new { controller = "Order"}));
        }

        public ActionResult ViewOrderList(int? clickIndex)
        {
            var userOrderListViewModel = new ViewOrederListViewModel();

            var userId = User.Identity.GetUserId();

            List<AdminOrderDetailsViewModels> adminOrderDetailsViewModels = new List<AdminOrderDetailsViewModels>();
            //var orderList = _context.Orders.Where(m=> m.UserId == userId).ToList();

            var orderListId = _context.Orders.Where(m => m.UserId == userId).Select(m => m.OrderId).ToList();
            
            var finaltotalPrice = _context.Orders.Where(m => m.UserId == userId).Select(m => m.finalTotalPrice).ToList();
            var dateTime = _context.Orders.Where(m => m.UserId == userId).Select(m => m.dateTime).ToList();
            var Status = _context.Orders.Where(m => m.UserId == userId).Select(m => m.status).ToList();
            var type= _context.Orders.Where(m => m.UserId == userId).Select(m => m.Type).ToList();


            userOrderListViewModel.OrderId = orderListId;
            for (int i = 0; i < orderListId.Count; i++)
            {
                AdminOrderDetailsViewModels singleOrderDetails = new AdminOrderDetailsViewModels();
                List<string> ProductName = new List<string>();
                List<int> ProductQuantity = new List<int>();
                List<int> ProductPrice = new List<int>();
                List<string> username = new List<string>();
                singleOrderDetails.OrderId = orderListId[i];
                var orderId = orderListId[i];
                var orderUserId = _context.Orders.Where(m => m.OrderId == orderId).Select(m => m.UserId).ToList();
                var oneUserID = orderUserId[0];
                var orderProductId = _context.OrderProducts.Where(m => m.OrderId == orderId).Select(m => m.ProductID).ToList();
                singleOrderDetails.ProductId = orderProductId;
                foreach (var item in orderProductId)
                {
                    var productName = _context.Products.Where(m => m.ProductID == item).Select(m => m.ProductName).ToList();
                    ProductName.Add(productName[0]);

                }
                singleOrderDetails.ProductName = ProductName;
                foreach (var item in orderProductId)
                {
                    var orderProductquantity = _context.OrderProducts.Where(m => m.ProductID == item).Select(m => m.ProductQuantity).ToList();
                    ProductQuantity.Add(orderProductquantity[0]);
                    var orderProductPrice = _context.OrderProducts.Where(m => m.ProductID == item).Select(m => m.ProductPrice).ToList();
                    foreach (var item1 in orderProductPrice)
                    {
                        ProductPrice.Add(item1);
                    }
                }
                singleOrderDetails.ProductQuantity = ProductQuantity;
                singleOrderDetails.ProductTotalPrice = ProductPrice;
                var totalPrice = _context.Orders.Where(m => m.OrderId == orderId).Select(m => m.finalTotalPrice).ToList();
                singleOrderDetails.finalTotalPrice = totalPrice[0];

                var status = _context.Orders.Where(m => m.OrderId == orderId).Select(m => m.status).FirstOrDefault();
                singleOrderDetails.status = status;

                foreach (var item in orderUserId)
                {
                    var firstName = _context.Users.Where(m => m.Id == item).Select(m => m.FirstName).ToList();
                    var lastName = _context.Users.Where(m => m.Id == item).Select(m => m.LastName).ToList();

                    username.Add(firstName[0] + " " + lastName[0]);
                }


                singleOrderDetails.UserName = username[0];
                var address = _context.Orders.Where(m => m.OrderId == orderId).Select(m => m.Address).ToList();
                singleOrderDetails.Address = address[0];

                var dateTimeOfSingleOrder  = _context.Orders.Where(m => m.OrderId == orderId).Select(m => m.dateTime).ToList();
                singleOrderDetails.dateTime = dateTimeOfSingleOrder[0];

                var typeOfSingleOrder = _context.Orders.Where(m => m.OrderId == orderId).Select(m => m.Type).ToList();
                singleOrderDetails.type = typeOfSingleOrder[0];

                var UserPhn = _context.Users.Where(m => m.Id == oneUserID).Select(m => m.PhoneNumber).ToList();
                singleOrderDetails.phn = UserPhn[0].Remove(0, 3);

                var UserEmail = _context.Users.Where(m => m.Id == oneUserID).Select(m => m.Email).ToList();
                singleOrderDetails.Email = UserEmail[0];
                adminOrderDetailsViewModels.Add(singleOrderDetails);
            }
            userOrderListViewModel.finalTotalPrice = finaltotalPrice;
            userOrderListViewModel.dateTime = dateTime;
            userOrderListViewModel.status = Status;
            userOrderListViewModel.Type = type;
            if (clickIndex != null)
            {
                userOrderListViewModel.clickIndex = (int)clickIndex;
            }
            else
            {
                userOrderListViewModel.clickIndex = 0;
            }
            userOrderListViewModel.FullOrderInformation = adminOrderDetailsViewModels;

            return View(userOrderListViewModel);
        }

        public ActionResult ItemDetails(int orderId, int productId)
        {
            var boxItemViewModel = new BoxItemViewModel();
            var singleProduct = _context.Products.SingleOrDefault(m => m.ProductID == productId);
            var productPrice = _context.OrderProducts.Where(m => m.ProductID == productId && m.OrderId == orderId).Select(x => x.ProductPrice).ToList();
            var productQuantity = _context.OrderProducts.Where(m => m.ProductID == productId && m.OrderId == orderId).Select(x => x.ProductQuantity).ToList();
            var ExtraIngredientSelectedId = _context.OrderProductExtraItems.Where(m => m.OrderId == orderId).Select(x => x.ExtraItemID).ToList();
            var ExtraIngredientSelectedQuantityInt = _context.OrderProductExtraItems.Where(m => m.OrderId == orderId).Select(x => x.ExtraItemQuantity).ToList();
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