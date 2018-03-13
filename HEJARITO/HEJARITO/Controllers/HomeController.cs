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
            else
            {
                return RedirectToAction("About");
            }
        }

        //JG 2018-03-12 Sidan som en nyss inloggad lärare hamnar på
        public ActionResult Teacher()
        {
            ViewBag.Message = "Teacher's start page.";

            TeacherViewModel teacherViewModel = new TeacherViewModel();

            // Calculating the start and end dates of the current week
            DateTime today = DateTime.Now;
            int start = 0;
            int end = 0;
            if (today.DayOfWeek == DayOfWeek.Monday)
            {
                start = 0; end = 6;
            }
            if (today.DayOfWeek == DayOfWeek.Tuesday)
            {
                start = -1; end = 5;
            }
            if (today.DayOfWeek == DayOfWeek.Wednesday)
            {
                start = -2; end = 4;
            }
            if (today.DayOfWeek == DayOfWeek.Thursday)
            {
                start = -3; end = 3;
            }
            if (today.DayOfWeek == DayOfWeek.Friday)
            {
                start = -4; end = 2;
            }
            if (today.DayOfWeek == DayOfWeek.Saturday)
            {
                start = -5; end = 1;
            }
            if (today.DayOfWeek == DayOfWeek.Sunday)
            {
                start = -6; end = 0;
            }
            TimeSpan startOffset = new TimeSpan(start, 0, 0, 0);
            TimeSpan endOffset = new TimeSpan(end, 0, 0, 0);
            DateTime startDate = DateTime.Now.Add(startOffset).Date;
            DateTime endDate = DateTime.Now.Add(endOffset).Date;

            // Intermediate decision: show starting and ending activities, and deadlines for the current week
            List<Activity> startingActivities = new List<Activity>();
            List<Activity> endingActivities = new List<Activity>();
            List<Activity> deadLines = new List<Activity>();
            startingActivities = db.Activities.Where(d => d.StartDate >= startDate && d.StartDate <= endDate).ToList();
            endingActivities = db.Activities.Where(d => d.EndDate >= startDate && d.EndDate <= endDate).ToList();
            deadLines = db.Activities.Where(d => d.DeadlineDate >= startDate && d.DeadlineDate <= endDate).ToList();
            teacherViewModel.Activities = startingActivities.Concat(endingActivities).Concat(deadLines).ToList();

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