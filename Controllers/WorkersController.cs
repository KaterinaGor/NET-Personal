using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Firma.Models;

namespace Firma.Controllers
{
    public class WorkersController : Controller
    {
        private FirmaPersonalEntities db = new FirmaPersonalEntities();

        // GET: Workers
        public ActionResult Index()
        {
            //var workers = db.Workers.Include(w => w.Job);
            var workers = db.Workers.Include(w => w.Job).OrderBy(m => m.Job.MainOrder).ThenBy(a => a.LastName);
            return View(workers.ToList());
        }

        // GET: Workers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Worker worker = db.Workers.Find(id);
            worker = db.Workers.Include(p => p.Job).FirstOrDefault(t => t.Id == id);
            if (worker == null)
            {
                return HttpNotFound();
            }
            return View(worker);
        }

        public ActionResult WorkersSearch(string WorkerName)
        {
            var workers = db.Workers.Where(w => w.LastName.Contains(WorkerName)).OrderBy(t => t.LastName).ToList(); 
            if (workers.Count <= 0)
            {
                return RedirectToAction("Index", "Workers");
            }
            return View(workers);
        }

        public FileContentResult GetImage(int id)
        {
            Worker worker = db.Workers.FirstOrDefault(g => g.Id == id);

            if (worker != null)
            {
                return File(worker.Photo, worker.PhotoType);
            }

            return null;
        }

        // GET: Workers/Create
        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            ViewBag.JobId = new SelectList(db.Jobs, "Id", "JobName");
            return View();
        }

        // POST: Workers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,LastName,Phone,Photo,PhotoType,JobId,Salary")] Worker worker, HttpPostedFileBase Image)
        {
            if (ModelState.IsValid)
            {
                if (Image != null)
                {
                    worker.PhotoType = Image.ContentType;
                    worker.Photo = new byte[Image.ContentLength];
                    Image.InputStream.Read(worker.Photo, 0, Image.ContentLength);
                }

                db.Workers.Add(worker);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.JobId = new SelectList(db.Jobs, "Id", "JobName", worker.JobId);
            return View(worker);
        }

        // GET: Workers/Edit/5
        [Authorize(Roles = "admin, moderator")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Worker worker = db.Workers.Find(id);
            if (worker == null)
            {
                return HttpNotFound();
            }
            ViewBag.JobId = new SelectList(db.Jobs, "Id", "JobName", worker.JobId);
            return View(worker);
        }

        // POST: Workers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,LastName,Phone,Photo,PhotoType,JobId,Salary")] Worker worker, HttpPostedFileBase Image)
        {
            if (ModelState.IsValid)
            {
                if (Image != null)
                {
                    worker.PhotoType = Image.ContentType;
                    worker.Photo = new byte[Image.ContentLength];
                    Image.InputStream.Read(worker.Photo, 0, Image.ContentLength);
                }
                db.Entry(worker).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.JobId = new SelectList(db.Jobs, "Id", "JobName", worker.JobId);
            return View(worker);
        }

        // GET: Workers/Delete/5
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Worker worker = db.Workers.Find(id);
            worker = db.Workers.Include(w => w.Job).FirstOrDefault(j => j.JobId == id);
            if (worker == null)
            {
                return HttpNotFound();
            }
            return View(worker);
        }

        // POST: Workers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Worker worker = db.Workers.Find(id);
            db.Workers.Remove(worker);
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
