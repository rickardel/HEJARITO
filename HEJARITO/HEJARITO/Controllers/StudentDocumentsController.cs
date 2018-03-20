using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HEJARITO.Models;

namespace HEJARITO.Controllers
{
    public class StudentDocumentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: StudentDocuments
        public ActionResult Index()
        {
            var studentDocuments = db.StudentDocuments.Include(s => s.Activity).Include(s => s.ApplicationUser);
            return View(studentDocuments.ToList());
        }

        // GET: StudentDocuments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentDocument studentDocument = db.StudentDocuments.Find(id);
            if (studentDocument == null)
            {
                return HttpNotFound();
            }
            return View(studentDocument);
        }

        public ActionResult Download(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentDocument studentDocument = db.StudentDocuments.Find(id);
            if (studentDocument == null)
            {
                return HttpNotFound();
            }
            string _FileName = Path.GetFileName(studentDocument.FileName);
            string localPath = "~/UploadedFiles";
            localPath += "/Course_" + studentDocument.Activity.Module.CourseId + "/Module_" + studentDocument.Activity.ModuleId + "/Activity_" + studentDocument.ActivityId + "/" + User.Identity.Name;


            string _path = Path.Combine(Server.MapPath(localPath), _FileName);

            return new FilePathResult(_path, studentDocument.ContentType);
        }

        // GET: StudentDocuments/Create
        public ActionResult Create()
        {
            ViewBag.Activities = db.Activities.ToList();
            return View();
        }

        // POST: StudentDocuments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,ActivityId,FileName,ContentLength,ContentType")] StudentDocument studentDocument, HttpPostedFileBase fileName)
        {
            studentDocument.UploadDate = DateTime.Now;
            studentDocument.ApplicationUserId = User.Identity.GetUserId();
                        
            if (ModelState.IsValid)
            {
                  
                try
                {
                    if (fileName.ContentLength > 0)
                    {
                        string _FileName = Path.GetFileName(fileName.FileName);
                        string localPath = "~/UploadedFiles";

                        studentDocument.Activity = db.Activities.Find(studentDocument.ActivityId);
                        studentDocument.ApplicationUser = db.Users.Find(studentDocument.ApplicationUserId);

                        localPath += "/Course_" + studentDocument.Activity.Module.CourseId + "/Module_" + studentDocument.Activity.ModuleId + "/Activity_" + studentDocument.ActivityId + "/" + User.Identity.GetUserName();
                        if (!Directory.Exists(Server.MapPath(localPath)))
                            Directory.CreateDirectory(Server.MapPath(localPath));
                        string _path = Path.Combine(Server.MapPath(localPath), _FileName);
                        studentDocument.ContentLength = fileName.ContentLength;
                        studentDocument.FileName = fileName.FileName;
                        studentDocument.ContentType = fileName.ContentType;
                        studentDocument.ApplicationUserId = User.Identity.Name;
                        fileName.SaveAs(_path);
                    }
                    db.StudentDocuments.Add(studentDocument);
                    db.SaveChanges();
                    ViewBag.Message = "Filen: " + fileName.FileName + " är nu uppladdad!!";
                    return RedirectToAction("Index");
                }
                catch
                {
                    ViewBag.Message = "Kunde inte ladda upp filen!!";
                }
                return RedirectToAction("Create");
            }

            //ViewBag.ActivityId = new SelectList(db.Activities, "Id", "Name", studentDocument.ActivityId);
            return View(studentDocument);
        }

        // GET: StudentDocuments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentDocument studentDocument = db.StudentDocuments.Find(id);
            if (studentDocument == null)
            {
                return HttpNotFound();
            }
            ViewBag.ActivityId = new SelectList(db.Activities, "Id", "Name", studentDocument.ActivityId);
            return View(studentDocument);
        }

        // POST: StudentDocuments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,UploadDate,ActivityId,FileName,ContentLength,ContentType")] StudentDocument studentDocument)
        {
            if (ModelState.IsValid)
            {
                db.Entry(studentDocument).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ActivityId = new SelectList(db.Activities, "Id", "Name", studentDocument.ActivityId);
            return View(studentDocument);
        }

        // GET: StudentDocuments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentDocument studentDocument = db.StudentDocuments.Find(id);
            if (studentDocument == null)
            {
                return HttpNotFound();
            }
            return View(studentDocument);
        }

        // POST: StudentDocuments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StudentDocument studentDocument = db.StudentDocuments.Find(id);
            db.StudentDocuments.Remove(studentDocument);
            db.SaveChanges();
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
