using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hospital.Models;

namespace Hospital.Controllers
{
    [Authorize]
	public class BedController : Controller
    {
        private HospitalEntities db = new HospitalEntities();

        //
        // GET: /Bed/
		public BedController(){
			Common.Instance.ConnnectionCheck(db);
		}

        public ActionResult Index()
        {
            return View(db.Beds.ToList());
        }

        //
        // GET: /Bed/Details/5

        public ActionResult Details(int id = 0)
        {
            Bed bed = db.Beds.Find(id);
            if (bed == null)
            {
                return RedirectToAction("_404", "Error");
            }
            return View(bed);
        }

        //
        // GET: /Bed/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Bed/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Bed bed)
        {
            if (ModelState.IsValid)
            {
                db.Beds.Add(bed);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(bed);
        }

        //
        // GET: /Bed/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Bed bed = db.Beds.Find(id);
            if (bed == null)
            {
                return HttpNotFound();
            }
            return View(bed);
        }

        //
        // POST: /Bed/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Bed bed)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bed).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bed);
        }

        //
        // GET: /Bed/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Bed bed = db.Beds.Find(id);
            if (bed == null)
                return RedirectToAction("_404", "Error");
            
            return View(bed);
        }

        //
        // POST: /Bed/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Bed bed = db.Beds.Find(id);
            db.Beds.Remove(bed);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}