using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HEJARITO.Models;

namespace HEJARITO.Controllers
{
    [Authorize] //TM 2018-03-12 13:33
    public class ActivitiesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Activities
        public ActionResult Index()
        {
            var activities = db.Activities.Include(a => a.ActivityType).Include(a => a.Module);
            return View(activities.ToList());
        }

        // GET: Activities/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity activity = db.Activities.Find(id);
            ViewBag.documents = db.Documents.Where(d => d.ActivityId == activity.Id).ToList();

            if (activity == null)
            {
                return HttpNotFound();
            }
            return View(activity);
        }

        // GET: Activities/Create
        [Authorize(Roles = "Teacher")] //TM 2018-03-12 13:34
        public ActionResult Create()
        {
            ViewBag.ActivityTypeId = new SelectList(db.ActivityTypes, "Id", "Name");
            ViewBag.ModuleId = new SelectList(db.Modules, "Id", "Name");
            return View();
        }

        // POST: Activities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Teacher")] //TM 2018-03-12 13:34
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,ActivityTypeId,Description,StartDate,DeadlineDate,EndDate,ModuleId")] Activity activity)
        {
            if (ModelState.IsValid)
            {
                db.Activities.Add(activity);
                db.SaveChanges();

                //TM 2018-03-19 16-19 Ska visas i nästa vy
                ViewBag.StatusMessage = "Skapande av en ny aktivitet genomfördes";

                int Id = Int32.Parse(Request["ModuleId"]);
                return RedirectToAction("Details", "Modules", new { id = Id });
            }

            ViewBag.ActivityTypeId = new SelectList(db.ActivityTypes, "Id", "Name", activity.ActivityTypeId);
            ViewBag.ModuleId = new SelectList(db.Modules, "Id", "Name", activity.ModuleId);
            return View(activity);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public PartialViewResult CreateAjax([Bind(Include = "Id,Name,ActivityTypeId,Description,StartDate,DeadlineDate,EndDate,ModuleId")] Activity activity)
        {
            Module module = db.Modules.Find(activity.ModuleId);
            if (module != null && ModelState.IsValid)
            {
                activity.Documents = new List<Document>();
                db.Activities.Add(activity);
                db.SaveChanges();

                //TM 2018-03-19 16-19 Ska visas i nästa vy
                ViewBag.KvittoMeddelande = "Skapande av en ny aktivitet genomfördes";

                return PartialView("_CourseModulesEditor", module.Course.Modules.OrderBy(m => m.StartDate).ToList());
            }
            ViewBag.ActivityTypeId = new SelectList(db.ActivityTypes, "Id", "Name", activity.ActivityTypeId);
            ViewBag.ModuleId = new SelectList(db.Modules, "Id", "Name", activity.ModuleId);
            return PartialView("_CourseModulesEditor");
        }


        public PartialViewResult GetActivityForm()
        {
            //Course course = db.Courses.Find(id);
            
            ViewBag.Test = db.ActivityTypes.ToList();
            ViewBag.Module = db.Modules.ToList();
            return PartialView("_CreateModuleActivity");
        }

        // GET: Activities/Edit/5
        [Authorize(Roles = "Teacher")] //TM 2018-03-12 13:37
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity activity = db.Activities.Find(id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            ViewBag.ActivityTypeId = new SelectList(db.ActivityTypes, "Id", "Name", activity.ActivityTypeId);
            ViewBag.ModuleId = new SelectList(db.Modules, "Id", "Name", activity.ModuleId);
            return View(activity);
        }

        // POST: Activities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Teacher")] //TM 2018-03-12 13:37
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,ActivityTypeId,Description,StartDate,DeadlineDate,EndDate,ModuleId")] Activity activity)
        {
            if (ModelState.IsValid)
            {
                db.Entry(activity).State = EntityState.Modified;
                db.SaveChanges();

                //TM 2018-03-19 16-19 Ska visas i nästa vy
                ViewBag.KvittoMeddelande = "Redigering av en aktivitet genomfördes";

                //return RedirectToAction("Index");
                int Id = Int32.Parse(Request["ModuleId"]);
                return RedirectToAction("Details", "Activities", new {id = Id });
            }
            ViewBag.ActivityTypeId = new SelectList(db.ActivityTypes, "Id", "Name", activity.ActivityTypeId);
            ViewBag.ModuleId = new SelectList(db.Modules, "Id", "Name", activity.ModuleId);
            return View(activity);
        }

        // GET: Activities/Delete/5
        [Authorize(Roles = "Teacher")] //TM 2018-03-12 13:37
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity activity = db.Activities.Find(id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            return View(activity);
        }

        // POST: Activities/Delete/5
        [Authorize(Roles = "Teacher")] //TM 2018-03-12 13:37
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Activity activity = db.Activities.Find(id);
            db.Activities.Remove(activity);
            db.SaveChanges();

            //TM 2018-03-19 16-19 Ska visas i nästa vy
            ViewBag.KvittoMeddelande = "Borttagning av en aktivitet genomfördes";

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
