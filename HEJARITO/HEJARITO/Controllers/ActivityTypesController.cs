using HEJARITO.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace HEJARITO.Controllers
{
    [Authorize] //TM 2018-03-12 13:38
    public class ActivityTypesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ActivityTypes
        public ActionResult Index()
        {
            return View(db.ActivityTypes.ToList());
        }

        // GET: ActivityTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ActivityType activityType = db.ActivityTypes.Find(id);
            if (activityType == null)
            {
                return HttpNotFound();
            }
            return View(activityType);
        }

        // GET: ActivityTypes/Create
        [Authorize(Roles = "Teacher")] //TM 2018-03-12 13:38
        public ActionResult Create()
        {
            return View();
        }

        // POST: ActivityTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Teacher")] //TM 2018-03-12 13:38
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description")] ActivityType activityType)
        {
            if (ModelState.IsValid)
            {
                db.ActivityTypes.Add(activityType);
                db.SaveChanges();

                //TM 2018-03-19 16-19 Ska visas i nästa vy
                ViewBag.KvittoMeddelande = "Skapandet av en ny aktivitetstyp genomfördes";

                return RedirectToAction("Index");
            }

            return View(activityType);
        }

        // GET: ActivityTypes/Edit/5
        [Authorize(Roles = "Teacher")] //TM 2018-03-12 13:38
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ActivityType activityType = db.ActivityTypes.Find(id);
            if (activityType == null)
            {
                return HttpNotFound();
            }
            return View(activityType);
        }

        // POST: ActivityTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Teacher")] //TM 2018-03-12 13:38
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description")] ActivityType activityType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(activityType).State = EntityState.Modified;
                db.SaveChanges();

                //TM 2018-03-19 16-19 Ska visas i nästa vy
                ViewBag.KvittoMeddelande = "Redigering av en aktivitetstyp genomfördes";

                return RedirectToAction("Index");
            }
            return View(activityType);
        }

        // GET: ActivityTypes/Delete/5
        [Authorize(Roles = "Teacher")] //TM 2018-03-12 13:38
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ActivityType activityType = db.ActivityTypes.Find(id);
            if (activityType == null)
            {
                return HttpNotFound();
            }
            return View(activityType);
        }

        // POST: ActivityTypes/Delete/5
        [Authorize(Roles = "Teacher")] //TM 2018-03-12 13:38
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ActivityType activityType = db.ActivityTypes.Find(id);
            db.ActivityTypes.Remove(activityType);
            db.SaveChanges();

            //TM 2018-03-19 16-19 Ska visas i nästa vy
            ViewBag.KvittoMeddelande = "Borttagning av en aktivitetstyp genomfördes";

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