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
    public class DoctorController : Controller
    {
        private HospitalEntities db = new HospitalEntities();

		public DoctorController(){
			Common.Instance.ConnnectionCheck(db);
		}

        public ActionResult Index()
        {
            return View(db.Doctors.ToList());
        }

        //
        // GET: /Doctor/Details/5

        public ActionResult Details(int id = 0)
        {
            Doctor doctor = db.Doctors.Find(id);
            if (doctor == null)
            {
                return RedirectToAction("_404", "Error");
            }
            return View(doctor);
        }

        //
        // GET: /Doctor/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Doctor/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Doctor doctor)
        {
            if (ModelState.IsValid)
            {
                db.Doctors.Add(doctor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(doctor);
        }

        //
        // GET: /Doctor/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Doctor doctor = db.Doctors.Find(id);
            if (doctor == null)
            {
                return RedirectToAction("_404", "Error");
            }
            return View(doctor);
        }

        //
        // POST: /Doctor/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Doctor doctor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(doctor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(doctor);
        }

        //
        // GET: /Doctor/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Doctor doctor = db.Doctors.Find(id);
            if (doctor == null)
            {
                return RedirectToAction("_404", "Error");
            }
            return View(doctor);
        }

        //
        // POST: /Doctor/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Doctor doctor = db.Doctors.Find(id);
            db.Doctors.Remove(doctor);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        /* method gives the history of a patient that has been treated */
        public ActionResult History(int id)
        {
            IEnumerable<Patient> treatedPatients = null;
            var visits = from m in db.Visits
                         where m.DoctorId == id
                         select m.PatientId;
            foreach (int patientID in visits)
            {
                treatedPatients = from p in db.Patients
                                  where p.Id == patientID
                                  select p;
            }
            ViewData["Doctor"] = db.Doctors.Find(id);
            return View(treatedPatients);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}