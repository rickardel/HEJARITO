using HEJARITO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HEJARITO.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult SelectStartView()
        {
            if (User.IsInRole("Teacher"))
            {
                return RedirectToAction("Teacher");
            }
            if (User.IsInRole("Student"))
            {
                return RedirectToAction("Student");
            }
            return View();
        }

        public ActionResult Teacher()
        {
            ViewBag.Message = "Teacher's start page.";

            TeacherViewModel teacherViewModel = new TeacherViewModel();

            //var allCourses = db.Courses.ToList();
            //var allActivities = new List<Activity>();
            //foreach (var course in allCourses)
            //{
            //    var courseModules = course.Modules.ToList();
            //    foreach (var module in courseModules)
            //    {
            //        var moduleActivities = module.Activities.ToList();
            //        foreach(var activity in moduleActivities)
            //        {
            //            allActivities.Add(activity);
            //        }
            //    }
            //}
            teacherViewModel.Activities = db.Activities.ToList();

            teacherViewModel.Courses = db.Courses.ToList();

            teacherViewModel.Users = db.Users.ToList();

            return View(teacherViewModel);
        }

        public ActionResult Student()
        {
            ViewBag.Message = "Student's start page.";

            return View();
        }
    }
}