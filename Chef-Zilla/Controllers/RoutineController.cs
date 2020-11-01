using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class RoutineController : Controller
    {
        private ApplicationDbContext _context;

        public RoutineController()
        {
            _context = new ApplicationDbContext();
        }
        // GET: Routine
        public ActionResult Create(int id)
        {
            RoutineCreateViewModel routineCreateViewModel = new RoutineCreateViewModel();
            var userId = User.Identity.GetUserId();
            var firstName = _context.Users.Where(m => m.Id == userId).Select(x => x.FirstName).ToList();
            var lastName = _context.Users.Where(m => m.Id == userId).Select(x => x.LastName).ToList();
            var email = _context.Users.Where(m => m.Id == userId).Select(x => x.Email).ToList();
            var phone = _context.Users.Where(m => m.Id == userId).Select(x => x.PhoneNumber).ToList();
            var box = _context.Boxes.Where(m => m.UserId == userId).ToList();
            var price = _context.BoxProducts.Where(m => m.BoxID == id).Select(x => x.TotalPrice).ToList();
            List<int> TotalPrice = new List<int>();
            foreach (var item in box)
            {
                var Price = _context.BoxProducts.Where(m => m.BoxID == item.BoxID).Select(x => x.TotalPrice).ToList();
                var totalPrice = 0;
                foreach (var item1 in Price)
                {
                    totalPrice = totalPrice + item1;
                }
                TotalPrice.Add(totalPrice);
            }
            var totalPriceOfCurrentItem = 0;
            foreach (var item in price)
            {
                totalPriceOfCurrentItem = totalPriceOfCurrentItem + item;
            }
            Console.WriteLine(TotalPrice);
            routineCreateViewModel.Boxes = box;
            routineCreateViewModel.BoxSelectID = id;
            routineCreateViewModel.CustomerName = firstName[0] + " "+ lastName[0];
            routineCreateViewModel.CustomerEmail = email[0];
            routineCreateViewModel.CustomerPhone = phone[0].Remove(0, 3);
            routineCreateViewModel.TotalPrice = totalPriceOfCurrentItem;
            routineCreateViewModel.TotalPriceList = TotalPrice;
            Session["BoxList"] = box;
            Session["TotalPriceList"] = TotalPrice;
            Session["BoxSelectID"] = id;
            return View(routineCreateViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RoutineCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            Console.WriteLine(model.RoutineName);
            Console.WriteLine(model.BoxID);
            Console.WriteLine(model.RoutineType);
            Console.WriteLine(model.DeliveryDay);
            Console.WriteLine(model.DeliveryDate);
            Console.WriteLine(model.CustomerName);
            Console.WriteLine(model.CustomerEmail);
            Console.WriteLine(model.CustomerPhone);
            Console.WriteLine(model.DeliveryAddress);
            Console.WriteLine(model.TotalPrice);
            var userId = User.Identity.GetUserId();
            Routine routine = new Routine();
            routine.RoutineName = model.RoutineName;
            routine.BoxID = model.BoxID;
            routine.UserId = userId;
            routine.DeliveryAddress = model.DeliveryAddress;
            routine.RoutineStatus = "Active";
            _context.Routines.Add(routine);
            _context.SaveChanges();
            int routineId = routine.RoutineID;
            RoutineSchedule routineSchedule = new RoutineSchedule();
            routineSchedule.RoutineType = model.RoutineType;
            var deliveryDay = "";
            if (model.RoutineType == "Weekly")
            {
                for (int i = 0; i < model.DeliveryDay.Count; i++)
                {
                    if (i == model.DeliveryDay.Count - 1)
                    {
                        deliveryDay += model.DeliveryDay[i];
                    }
                    else
                    {
                        deliveryDay += model.DeliveryDay[i] + ",";
                    }
                }
                routineSchedule.DeliveryDay = deliveryDay;
            }
            if (model.RoutineType == "Monthly")
            {
                string[] date = model.DeliveryDate.Split(',');
                var day = "";
                List<int> DayList = new List<int>();

                for (int i = 0; i < date.Length; i++)
                {
                    string[] item = date[i].Split('/');
                    DayList.Add(Int32.Parse(item[1]));
                }
                DayList.Sort();
                for (int i = 0; i < DayList.Count; i++)
                {
                    if (i == DayList.Count - 1)
                    {
                        day += DayList[i];
                    }
                    else
                    {
                        day += DayList[i] + ",";
                    }
                }
                Console.WriteLine(DayList);
                routineSchedule.DeliveryDate = day;
            }
            routineSchedule.RoutineID = routineId;
            routineSchedule.OrderStatus = "pending";
            _context.RoutineSchedules.Add(routineSchedule);
            _context.SaveChanges();
            TempData["message"] = "Routine succesfully created.";
            Session["BoxList"] = null;
            Session["TotalPriceList"] = null;
            Session["BoxSelectID"] = null;
            return RedirectToAction("Index", new RouteValueDictionary(new { controller = "FoodPlan"}));
        }

        public ActionResult Details(int id)
        {
            RoutineDetailsViewModel routineDetailsViewModel = new RoutineDetailsViewModel();
            var routine = _context.Routines.Where(x => x.RoutineID == id).ToList();
            var routineSchedule = _context.RoutineSchedules.Where(x => x.RoutineID == id).ToList();
            int boxId = routine[0].BoxID;
            var boxName = _context.Boxes.Where(m => m.BoxID == boxId).Select(x => x.BoxName).ToList();
            routineDetailsViewModel.RoutineID = id;
            routineDetailsViewModel.RoutineName = routine[0].RoutineName;
            routineDetailsViewModel.BoxName = boxName[0];
            routineDetailsViewModel.RoutineType = routineSchedule[0].RoutineType;
            routineDetailsViewModel.RoutineStatus = routine[0].RoutineStatus;
            routineDetailsViewModel.DeliveredDate = routineSchedule[0].DeliveredDate;
            if (routine[0].RoutineStatus == "Active")
            {
                DateTime today = DateTime.Today;
                Console.WriteLine(today.Day);
                if (routineSchedule[0].RoutineType == "Daily")
                {
                    var newDate = today.AddDays(1);
                    routineDetailsViewModel.UpcomingDeliveryDate = newDate.ToString("dddd: dd MMMM yyyy");
                }
                if (routineSchedule[0].RoutineType == "Weekly")
                {
                    routineDetailsViewModel.DeliveryDay = routineSchedule[0].DeliveryDay;
                    var dayMatch = routineSchedule[0].DeliveryDay.Contains(today.DayOfWeek.ToString());
                    Console.WriteLine(dayMatch);
                    if (dayMatch && routineSchedule[0].OrderStatus == "pending")
                    {
                        routineDetailsViewModel.UpcomingDeliveryDate = today.ToString("dddd: dd MMMM yyyy");
                    }
                    else
                    {
                        string[] day = routineSchedule[0].DeliveryDay.Split(',');
                        for (int i = 1; i < 7; i++)
                        {
                            var newDate = today.AddDays(i);
                            if (routineSchedule[0].DeliveryDay.Contains(newDate.DayOfWeek.ToString()))
                            {
                                routineDetailsViewModel.UpcomingDeliveryDate = newDate.ToString("dddd: dd MMMM yyyy");
                                break;
                            }
                        }
                    }

                }
                if (routineSchedule[0].RoutineType == "Monthly")
                {
                    routineDetailsViewModel.DeliveryDate = routineSchedule[0].DeliveryDate;
                    var dayMatch = routineSchedule[0].DeliveryDate.Contains(today.Day.ToString());
                    Console.WriteLine(dayMatch);
                    if (dayMatch && routineSchedule[0].OrderStatus == "pending")
                    {
                        routineDetailsViewModel.UpcomingDeliveryDate = today.ToString("dddd: dd MMMM yyyy");
                    }
                    else
                    {
                        string[] date = routineSchedule[0].DeliveryDate.Split(',');
                        for (var i = 0; i < date.Length; i++)
                        {
                            var nextDate = Int32.Parse(date[i]);
                            if (nextDate > today.Day)
                            {
                                var newDate = today.AddDays(nextDate - today.Day);
                                routineDetailsViewModel.UpcomingDeliveryDate = newDate.ToString("dddd: dd MMMM yyyy");
                                break;
                            }

                            if (i == date.Length - 1 && !(nextDate > today.Day))
                            {
                                var newDate = today.AddDays(Int32.Parse(date[0]) - today.Day);
                                newDate = newDate.AddMonths(1);
                                routineDetailsViewModel.UpcomingDeliveryDate = newDate.ToString("dddd: dd MMMM yyyy");
                                break;
                            }
                        }
                    }

                }
            }
            else
            {
                if (routineSchedule[0].RoutineType == "Weekly")
                {
                    routineDetailsViewModel.DeliveryDay = routineSchedule[0].DeliveryDay;
                }

                if (routineSchedule[0].RoutineType == "Monthly")
                {
                    routineDetailsViewModel.DeliveryDate = routineSchedule[0].DeliveryDate;
                }
            }
            var userId = User.Identity.GetUserId();
            var firstName = _context.Users.Where(m => m.Id == userId).Select(x => x.FirstName).ToList();
            var lastName = _context.Users.Where(m => m.Id == userId).Select(x => x.LastName).ToList();
            var email = _context.Users.Where(m => m.Id == userId).Select(x => x.Email).ToList();
            var phone = _context.Users.Where(m => m.Id == userId).Select(x => x.PhoneNumber).ToList();
            routineDetailsViewModel.CustomerName = firstName[0] + " " + lastName[0];
            routineDetailsViewModel.CustomerEmail = email[0];
            routineDetailsViewModel.CustomerPhone = phone[0].Remove(0, 3);
            routineDetailsViewModel.DeliveryAddress = routine[0].DeliveryAddress;
            return View(routineDetailsViewModel);
        }

        public async Task<ActionResult> PauseRoutine(int routineId)
        {
            var routine = _context.Routines.Where(x => x.RoutineID == routineId).ToList();
            routine[0].RoutineStatus = "Pause";
            _context.SaveChanges();
            TempData["message"] = "Your Routine has been paused Successfully.";
            return RedirectToAction("Details", new RouteValueDictionary(new { controller = "Routine", id = routineId }));
        }

        public async Task<ActionResult> ActivateRoutine(int routineId)
        {
            var routine = _context.Routines.Where(x => x.RoutineID == routineId).ToList();
            routine[0].RoutineStatus = "Active";
            _context.SaveChanges();
            TempData["message"] = "Routine Activated Successfully.";
            return RedirectToAction("Details", new RouteValueDictionary(new { controller = "Routine", id = routineId }));
        }

        public async Task<ActionResult> DeleteRoutine(int routineId)
        {
            var routine = _context.Routines.Where(x => x.RoutineID == routineId).ToList();
            var routineSchedule = _context.RoutineSchedules.Where(x => x.RoutineID == routineId).ToList();
            _context.Routines.RemoveRange(routine);
            _context.RoutineSchedules.RemoveRange(routineSchedule);
            _context.SaveChanges();
            TempData["message"] = "Routine Deleted Successfully.";
            return RedirectToAction("Index", "FoodPlan"); 
        }
    }
}