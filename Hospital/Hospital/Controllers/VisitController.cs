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
    public class VisitController : Controller
    {
        private HospitalEntities db = new HospitalEntities();

		public VisitController(){
			Common.Instance.ConnnectionCheck(db);
		}

        public ActionResult Index()
        {
            var visits = db.Visits.Include(v => v.Bed).Include(v => v.Doctor).Include(v => v.Patient);
            return View(visits.ToList());
        }

        //
        // GET: /Visit/Details/5

        public ActionResult Details(int id = 0)
        {
            Visit visit = db.Visits.Find(id);
            if (visit == null)
            {
                return RedirectToAction("_404", "Error");
            }
            return View(visit);
        }

        //
        // GET: /Visit/Create

        public ActionResult Create()
        {
			
			CreateSelectList();

            return View();
        }

        //
        // POST: /Visit/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Visit visit)
        {
            if (ModelState.IsValid)
            {
				visit.IsPay			= false;
				visit.DateOfVisit	= DateTime.Now;
				
				AssignBed(visit);

				if(ModelState.IsValid){					
					db.Visits.Add(visit);
					db.SaveChanges();
					return RedirectToAction("Index");
				}
            }

            ViewBag.BedId		= new SelectList(db.Beds.OrderBy(b=> b.Name), "Id", "Name", visit.BedId);
            ViewBag.DoctorId	= new SelectList(db.Doctors.OrderBy(d=> d.Name), "Id", "Name", visit.DoctorId);
            ViewBag.PatientId	= new SelectList(db.Patients.OrderBy(p=> p.Name), "Id", "Name", visit.PatientId);

            return View(visit);
        }

        //
        // GET: /Visit/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Visit visit = db.Visits.Find(id);
            if (visit == null)
                return RedirectToAction("_404", "Error");
            
			//AssignBed(visit);

            CreateSelectList(visit);

			return View(visit);
        }

        //
        // POST: /Visit/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Visit visit)
        {
			AssignBed(visit);

            if (ModelState.IsValid)
            {
                db.Entry(visit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

			CreateSelectList(visit);

            return View(visit);
        }

        //
        // GET: /Visit/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Visit visit = db.Visits.Find(id);
            if (visit == null)
            {
                return RedirectToAction("_404", "Error");
            }
            return View(visit);
        }

        //
        // POST: /Visit/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Visit visit = db.Visits.Find(id);
            db.Visits.Remove(visit);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Search(Search search)
        {
            return View(search);
        }

        /* search methods for the patient that has excerption handling*/
        [HttpPost]
        public PartialViewResult SearchResult(Search search)
        {
            IEnumerable<Visit> visits = null;
            if (!string.IsNullOrEmpty(search.Keyword))
            {
                try
                {
                    switch (search.Type)
                    {
                        case "Patient Name":
                            visits = db.Visits.Where(p => p.Patient.Name.Contains(search.Keyword)).OrderByDescending(p => p.Patient.Name);
                            break;
                        case "Date of Visit":
                            DateTime date = Convert.ToDateTime(search.Keyword);
                            visits = db.Visits.Where(v => v.DateOfVisit.Equals(date));
                            break;
                        case "Discharge Date":
                            date = Convert.ToDateTime(search.Keyword);
                            visits = db.Visits.Where(v => v.DateOfVisit.Equals(date));
                            break;
                        default:
                            int id = Convert.ToInt16(search.Keyword);
                            visits = db.Visits.Where(p => p.Id == id);
                            break;
                    }
                   
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Invalid input data");
                    visits = null;
                }
            }
            else
                ModelState.AddModelError("", "Keyword field is required");

            return PartialView("_List", visits);
        }

		private void AssignBed(Visit visit){
			visit.DateOfDischarge	= null;

			if(visit.PatientType && visit.BedId== null)
				ModelState.AddModelError("BedId", "Bed Id field is required for IN patient");
			else if(!visit.PatientType && visit.BedId!= null)
				ModelState.AddModelError("BedId", "Bed Id field is not required for OUT patient");
			else if(!visit.PatientType)
				visit.DateOfDischarge= visit.DateOfVisit.Date;
		}

		private void CreateSelectList(Visit visit){
			ViewBag.PatientId	= new SelectList(db.Patients.OrderBy(p=> p.Name), "Id", "Name", visit.PatientId);

			ViewBag.BedId = new SelectList(
				db.Beds.
				Where(b=> b.Id== visit.BedId || b.Visits.FirstOrDefault(v=> v.DateOfDischarge== null)== null).
				OrderBy(b => b.Name), "Id", "Name", visit.BedId);

            ViewBag.DoctorId = new SelectList(
				db.Doctors.OrderBy(d=> d.Name), "Id", "Name", visit.DoctorId);
		}

		private void CreateSelectList(){
			ViewBag.BedId = new SelectList(
				db.Beds.
				Where(b=> b.Visits.FirstOrDefault(v=> v.DateOfDischarge== null)== null).
				OrderBy(b => b.Name), "Id", "Name");

            ViewBag.DoctorId = new SelectList(
				db.Doctors.OrderBy(d=> d.Name), "Id", "Name");
            
			ViewBag.PatientId = new SelectList(
				db.Patients.
				Where(p=> p.Visits.FirstOrDefault(v=> v.DateOfDischarge== null || !v.IsPay)== null).
				OrderBy(p=> p.Name), "Id", "Name");
		}

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}