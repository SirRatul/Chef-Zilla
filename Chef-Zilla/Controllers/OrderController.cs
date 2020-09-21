﻿using System;
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
    [OutputCache(NoStore = true, Duration = 0)]    //prevent duplicate form submission
    public class OrderController : Controller
    {
        private ApplicationDbContext _context;        //variable to access database

        //constructor for database work
        public OrderController()
        {
            _context = new ApplicationDbContext();
        }

        public ActionResult FinalOrder()
        {
            return View();
        }

        // GET: Order
        [HttpPost]

        //need to pass a parameter
        public ActionResult Index(string address,int totalItem, int finalTotalPrice, List<int> ProductID, List<int> ProductQuantity, List<int> TotalPrice, string dateTime)
        {
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
            order.status = "pending";    // hardcore to update delivary status

            var cartId = _context.Carts.Where(m => m.UserId == userId).Select(x => x.CartID).ToList();

            foreach (var item in cartId)
            {
                var cartItem = _context.Carts.Where(m => m.CartID == item).ToList();
                var cartExtraItem = _context.CartProductExtraItems.Where(m => m.CartID == item).ToList();
                //cartExtraIngrediant.Add(cartExtraItem[0]);
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
                    //orderProductExtraItem.ExtraItemID = cartExtraItemId[0];
                    for(var i = 0; i < cartExtraItemId.Count; i++)
                    {
                        orderProductExtraItem.ExtraItemID = cartExtraItemId[i];
                        orderProductExtraItem.ExtraItemQuantity = cartExtraQuantity[i];
                        orderProductExtraItem.OrderId = orderId;
                        _context.OrderProductExtraItems.Add(orderProductExtraItem);
                        _context.SaveChanges();
                    }
                    /*foreach (var extraItem in cartExtraItemId)
                    {
                        orderProductExtraItem.ExtraItemID = extraItem;
                        orderProductExtraItem.OrderId = orderId;
                        _context.OrderProductExtraItems.Add(orderProductExtraItem);
                        _context.SaveChanges();
                    }*/
                }

            }

            //_context.Orders.Find(order, "OrderId");




            _context.Carts.RemoveRange(cartMain);
            _context.CartProductExtraItems.RemoveRange(cartExtraIngrediant);
            _context.SaveChanges();


            //_context.OrderProductExtraItems.Add(orderProductExtraItem);
            //_context.SaveChanges();


            return RedirectToAction("ViewOrderList", new RouteValueDictionary(new { controller = "Order"}));
        }

        public ActionResult ViewOrderList()
        {
            Order order = new Order();

            ViewOrederListViewModel viewOrederListViewModel = new ViewOrederListViewModel();

            //order.OrderId = viewOrederListViewModel.OrderId;

            var userId = User.Identity.GetUserId();
            viewOrederListViewModel.UserId = userId;

            var orderId  = _context.Orders.Where(m => m.UserId == userId).Select(x => x.OrderId).ToList();

            viewOrederListViewModel.OrderId = orderId;

            List<int> ProductId = new List<int>();
            List<int> FinalTotalPrice = new List<int>();
            List<int> totalItem = new List<int>();
            List<string> address = new List<string>();
            List<string> productName = new List<string>();
            List<string> status = new List<string>();
            List<string> dateTime = new List<string>();

            //dateTime



            foreach (var item in orderId)
            {
                var productId = _context.OrderProducts.Where(m => m.OrderId == item).Select(x => x.ProductID).ToList();
                ProductId.Add(productId[0]);
            }
            viewOrederListViewModel.ProductID = ProductId;

            foreach (var item in ProductId)
            {
                var PName = _context.Products.Where(m => m.ProductID == item).Select(x => x.ProductName).ToList();
                productName.Add(PName[0]);
            }
            viewOrederListViewModel.ProductName = productName;

            foreach (var item in orderId)
            {
                var finalPrice = _context.Orders.Where(m => m.OrderId == item).Select(x => x.finalTotalPrice).ToList();
                FinalTotalPrice.Add(finalPrice[0]);
            }
            viewOrederListViewModel.finalTotalPrice = FinalTotalPrice;

            foreach (var item in orderId)
            {
                var addressFind = _context.Orders.Where(m => m.OrderId == item).Select(x => x.Address).ToList();
                address.Add(addressFind[0]);
            }
            viewOrederListViewModel.Address = address;

            foreach (var item in orderId)
            {
                var TotalItemFind = _context.Orders.Where(m => m.OrderId == item).Select(x => x.totalItem).ToList();
                totalItem.Add(TotalItemFind[0]);
            }
            viewOrederListViewModel.totalItem = totalItem;

            foreach (var item in orderId)
            {
                var statusFind = _context.Orders.Where(m => m.OrderId == item).Select(x => x.status).ToList();
                status.Add(statusFind[0]);
            }
            viewOrederListViewModel.status = status;

            foreach (var item in orderId)
            {
                var DateTimeFind = _context.Orders.Where(m => m.OrderId == item).Select(x => x.dateTime).ToList();
                dateTime.Add(DateTimeFind[0]);
            }
            viewOrederListViewModel.dateTime = dateTime;



            return View(viewOrederListViewModel);
        }
    }
}