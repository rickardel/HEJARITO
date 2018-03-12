﻿using HEJARITO.Models;
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

        public ActionResult Teacher()
        {
            ViewBag.Message = "Teacher's start page.";

            return View();
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