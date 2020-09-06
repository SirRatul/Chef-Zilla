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
    public class FoodPlanController : Controller
    {
        private ApplicationDbContext _context;

        public FoodPlanController()
        {
            _context = new ApplicationDbContext();
        }

        // GET: FoodPlan
        public ActionResult Index()
        {
            FoodPlanViewModel foodPlanViewModel = new FoodPlanViewModel();
            var userId = User.Identity.GetUserId();
            var box = _context.Boxes.Where(m => m.UserId == userId).ToList();
            var routineId = _context.Routines.Where(x => x.UserId == userId)
                .Select(x => x.RoutineID).ToList();
            var routineName = _context.Routines.Where(x => x.UserId == userId)
                .Select(x => x.RoutineName).ToList();
            var boxId = _context.Routines.Where(x => x.UserId == userId)
                .Select(x => x.BoxID).ToList();
            List<string> BoxName = new List<string>();
            List<string> RoutineType = new List<string>();
            foreach (var item in boxId)
            {
                var boxName = _context.Boxes.Where(x => x.BoxID == item)
                    .Select(x => x.BoxName).ToList();
                BoxName.Add(boxName[0]);
            }
            foreach (var item in routineId)
            {
                var routineType = _context.RoutineSchedules.Where(x => x.RoutineID == item)
                    .Select(x => x.RoutineType).ToList();
                RoutineType.Add(routineType[0]);
            }
            foodPlanViewModel.Boxes = box;
            foodPlanViewModel.RoutineID = routineId;
            foodPlanViewModel.RoutineName = routineName;
            foodPlanViewModel.BoxName = BoxName;
            foodPlanViewModel.RoutineType = RoutineType;
            return View(foodPlanViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RedirectToCreateRoutine(int boxId)
        {
            return RedirectToAction("Create", new RouteValueDictionary(new { controller = "Routine", id = boxId }));
        }
    }
}