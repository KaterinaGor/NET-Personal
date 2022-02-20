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
    public class StatisticsController : Controller
    {
        private FirmaPersonalEntities db = new FirmaPersonalEntities();

        // GET: Statistics
        [Authorize(Roles = "admin")]
        public ActionResult Index()
        {
            var workers = db.Workers.ToList();
            ViewBag.Workers = workers;
            return View(db.Jobs.ToList());
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
