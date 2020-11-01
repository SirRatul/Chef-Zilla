using Chef_Zilla.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PagedList.Mvc;
using PagedList;
using Chef_Zilla.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace Chef_Zilla.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private ApplicationDbContext _context;
        private ApplicationUserManager _userManager;

        public AdminController()
        {
            _context = new ApplicationDbContext();
        }

        // GET: Admin
        public ActionResult Dashboard()
        {
            var order = _context.Orders.ToList();
            var item = _context.Products.ToList();
            var routine = _context.Routines.ToList();
            var user = _context.Users.ToList();
            var adminDashBoardViewModel = new AdminDashBoardViewModel();
            adminDashBoardViewModel.ItemNumber = item.Count;
            adminDashBoardViewModel.OrderNumber = order.Count;
            adminDashBoardViewModel.RoutineNumber = routine.Count;
            adminDashBoardViewModel.UserNumber = user.Count;
            return View(adminDashBoardViewModel);
        }

        public ActionResult AddProduct()
        {
            return View();
        }

        [HttpPost]
        public ActionResult OrderStatusUpdate(int orderID, string status)
        {
            var order = _context.Orders.Where(m => m.OrderId == orderID).ToList();
            order[0].status = status;

            _context.SaveChanges();
            return RedirectToAction("OrderList", "Admin");
        }
        public ActionResult OrderListDetails( int id )
        {
            AdminOrderDetailsViewModels adminOrderDetailsViewModels = new AdminOrderDetailsViewModels();

            List<string> ProductName = new List<string>();
            List<int> ProductQuantity = new List<int>();
            List<int> ProductPrice = new List<int>();
            List<string> username = new List<string>();


            adminOrderDetailsViewModels.OrderId = id;


            var orderUserId = _context.Orders.Where(m => m.OrderId == id).Select(m => m.UserId).ToList();
            var oneUserID = orderUserId[0];

            
            var orderProductId = _context.OrderProducts.Where(m => m.OrderId == id).Select(m => m.ProductID).ToList();
            adminOrderDetailsViewModels.ProductId = orderProductId;

            foreach (var item in orderProductId)
            {
                var productName = _context.Products.Where(m => m.ProductID == item).Select(m => m.ProductName).ToList();
                ProductName.Add(productName[0]);

            }
            adminOrderDetailsViewModels.ProductName = ProductName;

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
            adminOrderDetailsViewModels.ProductQuantity = ProductQuantity;
            adminOrderDetailsViewModels.ProductTotalPrice = ProductPrice;

            var totalPrice = _context.Orders.Where(m => m.OrderId == id).Select(m => m.finalTotalPrice).ToList();
            adminOrderDetailsViewModels.finalTotalPrice = totalPrice[0];

            var status = _context.Orders.Where(m => m.OrderId == id).Select(m => m.status).ToList();
            adminOrderDetailsViewModels.status = status[0];

            foreach (var item in orderUserId)
            {
                var firstName = _context.Users.Where(m => m.Id == item).Select(m => m.FirstName).ToList();
                var lastName = _context.Users.Where(m => m.Id == item).Select(m => m.LastName).ToList();

                username.Add(firstName[0] + " " + lastName[0]);
            }

            
            adminOrderDetailsViewModels.UserName = username[0];

            var address = _context.Orders.Where(m => m.OrderId == id).Select(m => m.Address).ToList();
            adminOrderDetailsViewModels.Address = address[0];

            var dateTime = _context.Orders.Where(m => m.OrderId == id).Select(m => m.dateTime).ToList();
            adminOrderDetailsViewModels.dateTime = dateTime[0];

            var type = _context.Orders.Where(m => m.OrderId == id).Select(m => m.Type).ToList();
            adminOrderDetailsViewModels.type = type[0];

            var UserPhn = _context.Users.Where(m => m.Id == oneUserID).Select(m => m.PhoneNumber).ToList();
            adminOrderDetailsViewModels.phn = UserPhn[0].Remove(0,3);

            var UserEmail = _context.Users.Where(m => m.Id == oneUserID).Select(m => m.Email).ToList();
            adminOrderDetailsViewModels.Email = UserEmail[0];
            return View(adminOrderDetailsViewModels);
        }

        public ActionResult OrderList()
        {
            var adminOrderListViewModel = new AdminOrderListViewModel();

            List<string> userName = new List<string>();


            var orderListId = _context.Orders.Select(m => m.OrderId).ToList();
            var orderListUserId = _context.Orders.Select(m => m.UserId).ToList();
            var finaltotalPrice = _context.Orders.Select(m => m.finalTotalPrice).ToList();
            var dateTime = _context.Orders.Select(m => m.dateTime).ToList();
            var Status = _context.Orders.Select(m => m.status).ToList();
            var type = _context.Orders.Select(m => m.Type).ToList();

            adminOrderListViewModel.OrderId = orderListId;

            foreach (var item in orderListUserId)
            {
                var firstName = _context.Users.Where(m => m.Id == item).Select(m => m.FirstName).ToList();
                var lastName = _context.Users.Where(m => m.Id == item).Select(m => m.LastName).ToList();

                userName.Add(firstName[0] + " "+lastName[0]);
            }

            adminOrderListViewModel.UserName = userName;
            adminOrderListViewModel.finalTotalPrice = finaltotalPrice;
            adminOrderListViewModel.dateTime = dateTime;
            adminOrderListViewModel.status = Status;
            adminOrderListViewModel.Type = type;

            return View(adminOrderListViewModel);
        }

        public ActionResult ViewProduct(int id)
        {
            var product = new Product();
            var singleProduct = _context.Products.SingleOrDefault(m => m.ProductID == id);
            product = singleProduct;

            var ingredientName = _context.Ingredients.Where(x => x.ProductID == id)
                .Select(x => x.IngredientName).ToList();

            var ingredientQuantity = _context.Ingredients.Where(x => x.ProductID == id)
                .Select(x => x.IngredientQuantity).ToList();

            var extraIngredientName = _context.ExtraIngredients.Where(x => x.ProductID == id)
                .Select(x => x.ExtraIngredientName).ToList();

            var extraIngredientPrice = _context.ExtraIngredients.Where(x => x.ProductID == id)
                .Select(x => x.ExtraIngredientPrice).ToList();

            product.IngredientName = ingredientName;
            product.IngredientQuantity = ingredientQuantity;
            product.ExtraIngredientName = extraIngredientName;
            product.ExtraIngredientPrice = extraIngredientPrice;
            return View(product);
        }

        public ActionResult EditProduct(int id)
        {
            var product = new Product();
            var singleProduct = _context.Products.SingleOrDefault(m => m.ProductID == id);
            product = singleProduct;

            var ingredientName = _context.Ingredients.Where(x => x.ProductID == id)
                .Select(x => x.IngredientName).ToList();

            var ingredientQuantity = _context.Ingredients.Where(x => x.ProductID == id)
                .Select(x => x.IngredientQuantity).ToList();

            var extraIngredientName = _context.ExtraIngredients.Where(x => x.ProductID == id)
                .Select(x => x.ExtraIngredientName).ToList();

            var extraIngredientPrice = _context.ExtraIngredients.Where(x => x.ProductID == id)
                .Select(x => x.ExtraIngredientPrice).ToList();

            product.IngredientName = ingredientName;
            product.IngredientQuantity = ingredientQuantity;
            product.ExtraIngredientName = extraIngredientName;
            product.ExtraIngredientPrice = extraIngredientPrice;
            return View(product);
        }

        public ActionResult AllProduct()
        {
            var product = _context.Products.ToList();
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddProduct(Product model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            string productFileName = model.ProductName;
            string extension = Path.GetExtension(model.ProductImageFile.FileName);
            productFileName = productFileName + extension;
            model.ProductImageFile.SaveAs(Path.Combine(Server.MapPath("~/Images/Product/"), productFileName));
            Product product = new Product();
            product.ProductType = model.ProductType;
            product.ProductName = model.ProductName;
            product.ProductImage = "~/Images/Product/"+ productFileName;
            product.PrepareTime = model.PrepareTime;
            product.ProductPrice = model.ProductPrice;
            try
            {
                _context.Products.Add(product);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            int productId = product.ProductID;

            for (int i = 0; i < model.IngredientName.Count; i++)
            {
                Ingredient ingredient = new Ingredient();
                ingredient.IngredientName = model.IngredientName[i];
                ingredient.IngredientQuantity = model.IngredientQuantity[i];
                ingredient.ProductID = productId;
                _context.Ingredients.Add(ingredient);
                _context.SaveChanges();
            }

            for (int i = 0; i < model.ExtraIngredientName.Count; i++)
            {
                ExtraIngredient extraIngredient = new ExtraIngredient();
                extraIngredient.ExtraIngredientName = model.ExtraIngredientName[i];
                extraIngredient.ExtraIngredientPrice = model.ExtraIngredientPrice[i];
                extraIngredient.ProductID = productId;
                _context.ExtraIngredients.Add(extraIngredient);
                _context.SaveChanges();
            }

            ModelState.AddModelError("", "Product Created Successfully.");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditProduct(Product model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            Product product = _context.Products.SingleOrDefault(m => m.ProductID == model.ProductID);

            if (model.ProductImageFile != null)
            {
                string productFileName = model.ProductName;
                string extension = Path.GetExtension(model.ProductImageFile.FileName);
                productFileName = productFileName + extension;
                model.ProductImageFile.SaveAs(Path.Combine(Server.MapPath("~/Images/Product/"), productFileName));
                product.ProductImage = "~/Images/Product/" + productFileName;
                model.ProductImage = "~/Images/Product/" + productFileName;
            }
            product.ProductType = model.ProductType;
            product.ProductName = model.ProductName;
            product.PrepareTime = model.PrepareTime;
            product.ProductPrice = model.ProductPrice;
            try
            {
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


            int productId = model.ProductID;
            var ingredient = _context.Ingredients.Where(x => x.ProductID == model.ProductID).ToList();

            if (ingredient.Count == model.IngredientName.Count)
            {
                for (int i = 0; i < model.IngredientName.Count; i++)
                {
                    ingredient[i].IngredientName = model.IngredientName[i];
                    ingredient[i].IngredientQuantity = model.IngredientQuantity[i];
                    ingredient[i].ProductID = productId;
                    _context.SaveChanges();
                }
            }

            if (ingredient.Count != model.IngredientName.Count)
            {
                for (int j = 0; j < ingredient.Count; j++)
                {
                    _context.Ingredients.Remove(ingredient[j]);
                    _context.SaveChanges();
                }
                for (int i = 0; i < model.IngredientName.Count; i++)
                {
                    Ingredient updatedIngredient = new Ingredient();
                    updatedIngredient.IngredientName = model.IngredientName[i];
                    updatedIngredient.IngredientQuantity = model.IngredientQuantity[i];
                    updatedIngredient.ProductID = productId;
                    _context.Ingredients.Add(updatedIngredient);
                    _context.SaveChanges();
                }
            }

            var extraIngredient = _context.ExtraIngredients.Where(x => x.ProductID == model.ProductID).ToList();

            if (extraIngredient.Count == model.ExtraIngredientName.Count)
            {
                for (int i = 0; i < model.ExtraIngredientName.Count; i++)
                {
                    extraIngredient[i].ExtraIngredientName = model.ExtraIngredientName[i];
                    extraIngredient[i].ExtraIngredientPrice = model.ExtraIngredientPrice[i];
                    extraIngredient[i].ProductID = productId;
                    _context.SaveChanges();
                }
            }

            if (extraIngredient.Count != model.ExtraIngredientName.Count)
            {
                for (int j = 0; j < extraIngredient.Count; j++)
                {
                    _context.ExtraIngredients.Remove(extraIngredient[j]);
                    _context.SaveChanges();
                }
                for (int i = 0; i < model.ExtraIngredientName.Count; i++)
                {
                    ExtraIngredient updatedExtraIngredient = new ExtraIngredient();
                    updatedExtraIngredient.ExtraIngredientName = model.ExtraIngredientName[i];
                    updatedExtraIngredient.ExtraIngredientPrice = model.ExtraIngredientPrice[i];
                    updatedExtraIngredient.ProductID = productId;
                    _context.ExtraIngredients.Add(updatedExtraIngredient);
                    _context.SaveChanges();
                }
            }

            ModelState.AddModelError("", "Product Updated Successfully.");
            return View(model);
        }

        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = _context.Products.SingleOrDefault(m => m.ProductID == id);
            var ingredient = _context.Ingredients.Where(x => x.ProductID == id);
            var extraIngredient = _context.ExtraIngredients.Where(x => x.ProductID == id);

            if (product == null)
            {
                TempData["message"] = "Product Not Found.";
                var productList = _context.Products.ToList();
                return View("AllProduct", productList);
            }

            _context.Products.Remove(product);
            _context.Ingredients.RemoveRange(ingredient);
            _context.ExtraIngredients.RemoveRange(extraIngredient);
            _context.SaveChanges();

            TempData["message"] = "Product Deleted Successfully.";
            var productAll = _context.Products.ToList();
            return View("AllProduct", productAll);
        }

        public ActionResult RoutineOrderList()
        {
            var routineOrderList = _context.Routines.Where(x => x.RoutineStatus == "Active").ToList();
            List<int> routineOrderId = new List<int>();
            List<string> routineType = new List<string>();
            List<string> userName = new List<string>();
            List<int> boxPrice = new List<int>();
            List<string> dateTime = new List<string>();
            List<string> status = new List<string>();
            DateTime today = DateTime.Today;
            var nextDate = today.AddDays(1);
            var currentDay = today.Date.ToString("dd-MMM-y");
            if (routineOrderList.Count > 0)
            {
                foreach (var item in routineOrderList)
                {
                    var dailyRoutine = _context.RoutineSchedules.Where(x => x.RoutineID == item.RoutineID && x.RoutineType == "Daily").ToList();
                    if (dailyRoutine.Count > 0)
                    {
                        var routineId = dailyRoutine[0].RoutineID;
                        routineOrderId.Add(routineId);
                        routineType.Add(dailyRoutine[0].RoutineType);

                        var userIdList = _context.Routines.Where(x => x.RoutineID == routineId).Select(x => x.UserId).ToList();
                        var userId = userIdList[0];
                        var firstName = _context.Users.Where(m => m.Id == userId).Select(m => m.FirstName).ToList();
                        var lastName = _context.Users.Where(m => m.Id == userId).Select(m => m.LastName).ToList();

                        userName.Add(firstName[0] + " " + lastName[0]);

                        var boxIdList = _context.Routines.Where(x => x.RoutineID == routineId).Select(x => x.BoxID).ToList();

                        var boxId = boxIdList[0];

                        var totalPrice = _context.BoxProducts.Where(x => x.BoxID == boxId)
                            .Select(x => x.TotalPrice).ToList();

                        var boxPriceOfRoutine = 0;

                        foreach (var item1 in totalPrice)
                        {
                            boxPriceOfRoutine = boxPriceOfRoutine + item1;
                        }
                        boxPrice.Add(boxPriceOfRoutine);
                        dateTime.Add(today.Date.ToString("dddd: dd MMMM yyyy"));
                        string deliveredDate = dailyRoutine[0].DeliveredDate;
                        if (string.IsNullOrEmpty(deliveredDate))
                        {
                            status.Add("pending");
                        }
                        else
                        {
                            string[] fullDeliveredDate = deliveredDate.Split(',');
                            var foundDate = false;
                            foreach (var item1 in fullDeliveredDate)
                            {
                                if ((item1.Contains(currentDay)))
                                {
                                    foundDate = true;
                                    break;
                                }
                            }
                            if (foundDate)
                            {
                                status.Add("delivered");
                            }
                            else
                            {
                                status.Add("pending");
                            }
                        }
                    }
                }
                foreach (var item in routineOrderList)
                {
                    var weeklyRoutine = _context.RoutineSchedules.Where(x => x.RoutineID == item.RoutineID && x.RoutineType == "Weekly").ToList();
                    if (weeklyRoutine.Count > 0)
                    {
                        if (weeklyRoutine[0].DeliveryDay.Contains(today.DayOfWeek.ToString()))
                        {
                            var routineId = weeklyRoutine[0].RoutineID;
                            routineOrderId.Add(weeklyRoutine[0].RoutineID);
                            routineType.Add(weeklyRoutine[0].RoutineType);

                            var userIdList = _context.Routines.Where(x => x.RoutineID == routineId).Select(x => x.UserId).ToList();
                            var userId = userIdList[0];
                            var firstName = _context.Users.Where(m => m.Id == userId).Select(m => m.FirstName).ToList();
                            var lastName = _context.Users.Where(m => m.Id == userId).Select(m => m.LastName).ToList();

                            userName.Add(firstName[0] + " " + lastName[0]);

                            var boxIdList = _context.Routines.Where(x => x.RoutineID == routineId).Select(x => x.BoxID).ToList();
                            var boxId = boxIdList[0];

                            var totalPrice = _context.BoxProducts.Where(x => x.BoxID == boxId)
                                .Select(x => x.TotalPrice).ToList();

                            var boxPriceOfRoutine = 0;

                            foreach (var item1 in totalPrice)
                            {
                                boxPriceOfRoutine = boxPriceOfRoutine + item1;
                            }
                            boxPrice.Add(boxPriceOfRoutine);
                            dateTime.Add(today.Date.ToString("dddd: dd MMMM yyyy"));
                            string deliveredDate = weeklyRoutine[0].DeliveredDate;
                            if (string.IsNullOrEmpty(deliveredDate))
                            {
                                status.Add("pending");
                            }
                            else
                            {
                                string[] fullDeliveredDate = deliveredDate.Split(',');
                                var foundDate = false;
                                foreach (var item1 in fullDeliveredDate)
                                {
                                    if ((item1.Contains(currentDay)))
                                    {
                                        foundDate = true;
                                        break;
                                    }
                                }
                                if (foundDate)
                                {
                                    status.Add("delivered");
                                }
                                else
                                {
                                    status.Add("pending");
                                }
                            }
                        }
                    }
                }
                foreach (var item in routineOrderList)
                {
                    var monthlyRoutine = _context.RoutineSchedules.Where(x => x.RoutineID == item.RoutineID && x.RoutineType == "Monthly").ToList();
                    if (monthlyRoutine.Count > 0)
                    {
                        if (monthlyRoutine[0].DeliveryDate.Contains(today.Day.ToString()))
                        {
                            var routineId = monthlyRoutine[0].RoutineID;
                            routineOrderId.Add(routineId);
                            routineType.Add(monthlyRoutine[0].RoutineType);

                            var userIdList = _context.Routines.Where(x => x.RoutineID == routineId).Select(x => x.UserId).ToList();
                            var userId = userIdList[0];
                            var firstName = _context.Users.Where(m => m.Id == userId).Select(m => m.FirstName).ToList();
                            var lastName = _context.Users.Where(m => m.Id == userId).Select(m => m.LastName).ToList();

                            userName.Add(firstName[0] + " " + lastName[0]);

                            var boxIdList = _context.Routines.Where(x => x.RoutineID == routineId).Select(x => x.BoxID).ToList();
                            var boxId = boxIdList[0];

                            var totalPrice = _context.BoxProducts.Where(x => x.BoxID == boxId)
                                .Select(x => x.TotalPrice).ToList();

                            var boxPriceOfRoutine = 0;

                            foreach (var item1 in totalPrice)
                            {
                                boxPriceOfRoutine = boxPriceOfRoutine + item1;
                            }
                            boxPrice.Add(boxPriceOfRoutine);
                            dateTime.Add(today.Date.ToString("dddd: dd MMMM yyyy"));
                            string deliveredDate = monthlyRoutine[0].DeliveredDate;
                            if (string.IsNullOrEmpty(deliveredDate))
                            {
                                status.Add("pending");
                            }
                            else
                            {
                                string[] fullDeliveredDate = deliveredDate.Split(',');
                                var foundDate = false;
                                foreach (var item1 in fullDeliveredDate)
                                {
                                    if ((item1.Contains(currentDay)))
                                    {
                                        foundDate = true;
                                        break;
                                    }
                                }
                                if (foundDate)
                                {
                                    status.Add("delivered");
                                }
                                else
                                {
                                    status.Add("pending");
                                }
                            }
                        }
                    }
                }
            }
            if (routineOrderList.Count > 0)
            {
                foreach (var item in routineOrderList)
                {
                    var dailyRoutine = _context.RoutineSchedules.Where(x => x.RoutineID == item.RoutineID && x.RoutineType == "Daily").ToList();
                    if (dailyRoutine.Count > 0)
                    {
                        var routineId = dailyRoutine[0].RoutineID;
                        routineOrderId.Add(routineId);
                        routineType.Add(dailyRoutine[0].RoutineType);

                        var userIdList = _context.Routines.Where(x => x.RoutineID == routineId).Select(x => x.UserId).ToList();
                        var userId = userIdList[0];
                        var firstName = _context.Users.Where(m => m.Id == userId).Select(m => m.FirstName).ToList();
                        var lastName = _context.Users.Where(m => m.Id == userId).Select(m => m.LastName).ToList();

                        userName.Add(firstName[0] + " " + lastName[0]);

                        var boxIdList = _context.Routines.Where(x => x.RoutineID == routineId).Select(x => x.BoxID).ToList();

                        var boxId = boxIdList[0];
                        var totalPrice = _context.BoxProducts.Where(x => x.BoxID == boxId)
                            .Select(x => x.TotalPrice).ToList();

                        var boxPriceOfRoutine = 0;

                        foreach (var item1 in totalPrice)
                        {
                            boxPriceOfRoutine = boxPriceOfRoutine + item1;
                        }
                        boxPrice.Add(boxPriceOfRoutine);
                        dateTime.Add(nextDate.Date.ToString("dddd: dd MMMM yyyy"));
                        status.Add("pending");
                    }
                }
                foreach (var item in routineOrderList)
                {
                    var weeklyRoutine = _context.RoutineSchedules.Where(x => x.RoutineID == item.RoutineID && x.RoutineType == "Weekly").ToList();
                    if (weeklyRoutine.Count > 0)
                    {
                        if (weeklyRoutine[0].DeliveryDay.Contains(nextDate.DayOfWeek.ToString()))
                        {
                            var routineId = weeklyRoutine[0].RoutineID;
                            routineOrderId.Add(routineId);
                            routineType.Add(weeklyRoutine[0].RoutineType);

                            var userIdList = _context.Routines.Where(x => x.RoutineID == routineId).Select(x => x.UserId).ToList();
                            var userId = userIdList[0];
                            var firstName = _context.Users.Where(m => m.Id == userId).Select(m => m.FirstName).ToList();
                            var lastName = _context.Users.Where(m => m.Id == userId).Select(m => m.LastName).ToList();

                            userName.Add(firstName[0] + " " + lastName[0]);

                            var boxIdList = _context.Routines.Where(x => x.RoutineID == routineId).Select(x => x.BoxID).ToList();
                            var boxId = boxIdList[0];

                            var totalPrice = _context.BoxProducts.Where(x => x.BoxID == boxId)
                                .Select(x => x.TotalPrice).ToList();

                            var boxPriceOfRoutine = 0;

                            foreach (var item1 in totalPrice)
                            {
                                boxPriceOfRoutine = boxPriceOfRoutine + item1;
                            }
                            boxPrice.Add(boxPriceOfRoutine);
                            dateTime.Add(nextDate.Date.ToString("dddd: dd MMMM yyyy"));
                            status.Add("pending");
                        }
                    }
                }
                foreach (var item in routineOrderList)
                {
                    var monthlyRoutine = _context.RoutineSchedules.Where(x => x.RoutineID == item.RoutineID && x.RoutineType == "Monthly").ToList();
                    if (monthlyRoutine.Count > 0)
                    {
                        if (monthlyRoutine[0].DeliveryDate.Contains(nextDate.Day.ToString()))
                        {
                            var routineId = monthlyRoutine[0].RoutineID;
                            routineOrderId.Add(routineId);
                            routineType.Add(monthlyRoutine[0].RoutineType);

                            var userIdList = _context.Routines.Where(x => x.RoutineID == routineId).Select(x => x.UserId).ToList();
                            var userId = userIdList[0];
                            var firstName = _context.Users.Where(m => m.Id == userId).Select(m => m.FirstName).ToList();
                            var lastName = _context.Users.Where(m => m.Id == userId).Select(m => m.LastName).ToList();

                            userName.Add(firstName[0] + " " + lastName[0]);

                            var boxIdList = _context.Routines.Where(x => x.RoutineID == routineId).Select(x => x.BoxID).ToList();
                            var boxId = boxIdList[0];

                            var totalPrice = _context.BoxProducts.Where(x => x.BoxID == boxId)
                                .Select(x => x.TotalPrice).ToList();

                            var boxPriceOfRoutine = 0;

                            foreach (var item1 in totalPrice)
                            {
                                boxPriceOfRoutine = boxPriceOfRoutine + item1;
                            }
                            boxPrice.Add(boxPriceOfRoutine);
                            dateTime.Add(nextDate.Date.ToString("dddd: dd MMMM yyyy"));
                            status.Add("pending");
                        }
                    }
                }
            }

            var adminRoutineOrderListViewModel = new AdminRoutineOrderListViewModel();
            adminRoutineOrderListViewModel.RoutineOrderId = routineOrderId;
            adminRoutineOrderListViewModel.RoutineType = routineType;
            adminRoutineOrderListViewModel.UserName = userName;
            adminRoutineOrderListViewModel.boxPrice = boxPrice;
            adminRoutineOrderListViewModel.dateTime = dateTime;
            adminRoutineOrderListViewModel.status = status;
            return View(adminRoutineOrderListViewModel);
        }

        public ActionResult RoutineOrderDetails(int id, string deliveryDate)
        {
            var deliveryDateTime = DateTime.Parse(deliveryDate.Substring(deliveryDate.IndexOf(':') + 2));
            string[] deliveryTime = deliveryDateTime.ToString().Split(' ');
            Console.WriteLine(deliveryTime[0]);
            RoutineDetailsViewModel routineDetailsViewModel = new RoutineDetailsViewModel();
            var routine = _context.Routines.Where(x => x.RoutineID == id).ToList();
            var routineSchedule = _context.RoutineSchedules.Where(x => x.RoutineID == id).ToList();
            int boxId = routine[0].BoxID;
            var boxName = _context.Boxes.Where(m => m.BoxID == boxId).Select(x => x.BoxName).ToList();

            if (routineSchedule[0].RoutineType == "Weekly")
            {
                routineDetailsViewModel.DeliveryDay = routineSchedule[0].DeliveryDay;
            }

            if (routineSchedule[0].RoutineType == "Monthly")
            {
                routineDetailsViewModel.DeliveryDate = routineSchedule[0].DeliveryDate;
            }

            routineDetailsViewModel.RoutineID = id;
            routineDetailsViewModel.RoutineName = routine[0].RoutineName;
            routineDetailsViewModel.BoxName = boxName[0];
            routineDetailsViewModel.RoutineType = routineSchedule[0].RoutineType;
            routineDetailsViewModel.DeliveredDate = routineSchedule[0].DeliveredDate;
            string[] fullDeliveredDate = routineSchedule[0].DeliveredDate.Split(',');
            var foundDate = false;
            foreach (var item in fullDeliveredDate)
            {
                if ((item.Contains(deliveryTime[0])))
                {
                    foundDate = true;
                    break;
                }
            }
            if (foundDate)
            {
                routineDetailsViewModel.RoutineStatus = "delivered";
            }
            else
            {
                routineDetailsViewModel.RoutineStatus = "pending";
            }
            var userId = routine[0].UserId;
            var firstName = _context.Users.Where(m => m.Id == userId).Select(x => x.FirstName).ToList();
            var lastName = _context.Users.Where(m => m.Id == userId).Select(x => x.LastName).ToList();
            var email = _context.Users.Where(m => m.Id == userId).Select(x => x.Email).ToList();
            var phone = _context.Users.Where(m => m.Id == userId).Select(x => x.PhoneNumber).ToList();
            routineDetailsViewModel.CustomerName = firstName[0] + " " + lastName[0];
            routineDetailsViewModel.CustomerEmail = email[0];
            routineDetailsViewModel.CustomerPhone = phone[0].Remove(0, 3);
            routineDetailsViewModel.DeliveryAddress = routine[0].DeliveryAddress;
            routineDetailsViewModel.UpcomingDeliveryDate = deliveryDate;
            return View(routineDetailsViewModel);
        }

        [HttpPost]
        public ActionResult RoutineOrderStatusUpdate(int routineOrderID, string status)
        {
            if (status == "")
            {
                status = "pending";
            }
            var routineOrder = _context.RoutineSchedules.Where(m => m.RoutineID == routineOrderID).ToList();
            var deliveredDate = routineOrder[0].DeliveredDate;
            var currentTime = DateTime.Now;
            var currentDay = currentTime.Date.ToString("dd-MMM-y");
            if (status == "delivered")
            {
                if (string.IsNullOrEmpty(deliveredDate))
                {
                    deliveredDate = currentTime.ToString();
                } else if(!(deliveredDate.Contains(currentDay)))
                {
                    deliveredDate = deliveredDate + "," + currentTime.ToString();
                }
                if (deliveredDate[0] == ',')
                {
                    deliveredDate = deliveredDate.Substring(1);
                }
                if (deliveredDate[deliveredDate.Length - 1] == ',')
                {
                    deliveredDate = deliveredDate.Remove(deliveredDate.Length - 1); ;
                }
                routineOrder[0].DeliveredDate = deliveredDate;
                routineOrder[0].OrderStatus = status;
            }

            if (status == "pending")
            {
                if (!(string.IsNullOrEmpty(deliveredDate)))
                {
                    if (deliveredDate.Contains(currentDay))
                    {
                        string[] fullDeliveredDate = deliveredDate.Split(',');
                        deliveredDate = "";
                        foreach (var item in fullDeliveredDate)
                        {
                            if (!(item.Contains(currentDay)))
                            {
                                deliveredDate = deliveredDate + "," + item;
                            }
                        }
                        deliveredDate = deliveredDate + ",";
                        if (deliveredDate[0] == ',')
                        {
                            deliveredDate = deliveredDate.Substring(1);
                        }
                        if (deliveredDate[deliveredDate.Length - 1] == ',')
                        {
                            deliveredDate = deliveredDate.Remove(deliveredDate.Length - 1); ;
                        }
                        routineOrder[0].DeliveredDate = deliveredDate;
                        routineOrder[0].OrderStatus = status;
                    }
                }
            }
            _context.SaveChanges();
            return RedirectToAction("RoutineOrderList", "Admin");
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public async Task<ActionResult> AllUser()
        {
            var user = _context.Users.ToList();
            var userList = new List<ApplicationUser>();
            foreach (var item in user)
            {
                var roles = await UserManager.GetRolesAsync(item.Id);
                if (roles.Contains("Admin"))
                {

                }
                else
                {
                    userList.Add(item);
                }
            }
            return View(userList);
        }

        public ActionResult AllRoutine()
        {
            var routines = _context.Routines.ToList();
            var routineSchedules = _context.RoutineSchedules.ToList();
            List<int> routineOrderId = new List<int>();
            List<string> routineName = new List<string>();
            List<string> routineType = new List<string>();
            List<string> routineStatus = new List<string>();
            List<string> userName = new List<string>();
            List<int> boxPrice = new List<int>();
            foreach (var item in routines)
            {
                routineOrderId.Add(item.RoutineID);
                routineName.Add(item.RoutineName);
                routineStatus.Add(item.RoutineStatus);
                var firstName = _context.Users.Where(m => m.Id == item.UserId).Select(x => x.FirstName).ToList();
                var lastName = _context.Users.Where(m => m.Id == item.UserId).Select(x => x.LastName).ToList();
                userName.Add(firstName[0] + " " + lastName[0]);

                var boxIdList = _context.Routines.Where(x => x.RoutineID == item.RoutineID).Select(x => x.BoxID).ToList();
                var boxId = boxIdList[0];

                var totalPrice = _context.BoxProducts.Where(x => x.BoxID == boxId)
                    .Select(x => x.TotalPrice).ToList();

                var boxPriceOfRoutine = 0;

                foreach (var item1 in totalPrice)
                {
                    boxPriceOfRoutine = boxPriceOfRoutine + item1;
                }
                boxPrice.Add(boxPriceOfRoutine);
            }
            foreach (var item in routineSchedules)
            {
                routineType.Add(item.RoutineType);
            }
            var adminRoutineListViewModel = new AdminRoutineListViewModel();
            adminRoutineListViewModel.RoutineOrderId = routineOrderId;
            adminRoutineListViewModel.RoutineName = routineName;
            adminRoutineListViewModel.RoutineStatus = routineStatus;
            adminRoutineListViewModel.RoutineType = routineType;
            adminRoutineListViewModel.UserName = userName;
            adminRoutineListViewModel.boxPrice = boxPrice;
            return View(adminRoutineListViewModel);
        }
    }
}