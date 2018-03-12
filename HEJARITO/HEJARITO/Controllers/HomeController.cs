using HEJARITO.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HEJARITO.Controllers
{
    public class HomeController : Controller
    {
        //TM 2018-03-09 15:09 Genom raden nedan får vi tillgång till vår databas
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

        //JG 2018-03-12 Sidan som en nyss inloggad lärare hamnar på
        public ActionResult Teacher()
        {
            ViewBag.Message = "Teacher's start page.";

            TeacherViewModel teacherViewModel = new TeacherViewModel();

            DateTime today = DateTime.Now;

            int start = 0;
            int stop = 0;
            if (today.DayOfWeek == DayOfWeek.Monday)
            {
                start = 0; stop = 5;
            }
            if (today.DayOfWeek == DayOfWeek.Tuesday)
            {
                start = -1; stop = 4;
            }
            if (today.DayOfWeek == DayOfWeek.Wednesday)
            {
                start = -2; stop = 3;
            }
            if (today.DayOfWeek == DayOfWeek.Thursday)
            {
                start = -3; stop = 2;
            }
            if (today.DayOfWeek == DayOfWeek.Friday)
            {
                start = -4; stop = 1;
            }
            if (today.DayOfWeek == DayOfWeek.Saturday)
            {
                start = -5; stop = 0;
            }
            if (today.DayOfWeek == DayOfWeek.Sunday)
            {
                start = -6; stop = -1;
            }

            TimeSpan startOffset = new System.TimeSpan(start, 0, 0, 0);
            TimeSpan stopOffset = new System.TimeSpan(stop, 0, 0, 0);
            DateTime startDate = DateTime.Now.Add(startOffset).Date;
            DateTime stopDate = DateTime.Now.Add(stopOffset).Date;

            // To do: Discuss valid dates!
            teacherViewModel.Activities = db.Activities.Where(d => d.StartDate >= startDate && d.StartDate <= stopDate).ToList();

            //teacherViewModel.Activities = db.Activities.ToList();

            teacherViewModel.Courses = db.Courses.ToList();

            teacherViewModel.Users = db.Users.ToList();

            return View(teacherViewModel);
        }

        public ActionResult Student()
        //TM 2018-03-09 23:24 Sidan som en nyss inloggad elev hamnar i
        {
            ViewBag.Message = "Student's start page.";

            var studentViewModel = new StudentViewModel();

            //TM 2018-03-10 02:16 Utan dessa 2 rader kraschar .Add(myCourse) !!!
            studentViewModel.Courses = db.Courses.ToList(); //!!! #1
            studentViewModel.Courses.Clear();               //!!! #2

            var myCourseId = db.Users.Find(User.Identity.GetUserId()).CourseId;

            if (myCourseId != null)
            {
                var myCourse = db.Courses.Find(myCourseId);

                if (myCourse != null)
                {
                    studentViewModel.Courses.Add(myCourse); //!!! #3
                }
            }

            studentViewModel.Modules = db.Modules.ToList();

            studentViewModel.Activities = db.Activities.ToList();

            return View(studentViewModel);
        }
    }
}