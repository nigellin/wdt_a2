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
    public class PatientController : Controller
    {
        private HospitalEntities db = new HospitalEntities();

        //
        // GET: /Patient/
		
		public PatientController(){
			Common.Instance.ConnnectionCheck(db);
		}

        public ActionResult Index()
        {
            return View(db.Patients.ToList());
        }

        //
        // GET: /Patient/Details/5

        public ActionResult Details(int id = 0)
        {
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return RedirectToAction("_404", "Error");
            }
            return View(patient);
        }

        //
        // GET: /Patient/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Patient/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Patient patient)
        {
            if (ModelState.IsValid)
            {
				patient.DateOfRegistration= DateTime.Now;
                
				db.Patients.Add(patient);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(patient);
        }

        //
        // GET: /Patient/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return RedirectToAction("_404", "Error");
            }
            return View(patient);
        }

        //
        // POST: /Patient/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Patient patient)
        {
            if (ModelState.IsValid)
            {
                db.Entry(patient).State = EntityState.Modified;
                db.SaveChanges();
				
                return RedirectToAction("Index");
            }
            return View(patient);
        }

        //
        // GET: /Patient/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return RedirectToAction("_404", "Error");
            }
            return View(patient);
        }

        //
        // POST: /Patient/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Patient patient = db.Patients.Find(id);
            db.Patients.Remove(patient);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

		[HttpGet]
        public ActionResult Search(Search search)
        {
            return View(search);
        }

		[HttpPost]
		public PartialViewResult SearchResult(Search search){
			IEnumerable<Patient> patients= null;
            DateTime date;
			if(!string.IsNullOrEmpty(search.Keyword)){
				try{
					switch(search.Type){
						case "Name":
							patients= db.Patients.Where(p=> p.Name.Contains(search.Keyword)).OrderByDescending(p=> p.Name);
							break;
						case "Admission Date":
                            date = Convert.ToDateTime(search.Keyword);
                            patients = from p in db.Patients
                                       where p.DateOfRegistration == date
                                       select p;
							break;
						case "Discharge Date":
							 date= Convert.ToDateTime(search.Keyword);
							
                            var visits = from v in db.Visits
                                         where v.DateOfDischarge == date
                                         select v;

                            patients = from p in visits
                                       select p.Patient;
							break;
						default:
							int id= Convert.ToInt16(search.Keyword);
							patients= db.Patients.Where(p=> p.Id== id);
							break;
					}
				
				}catch(Exception){
					ModelState.AddModelError("", "Invalid input data");
					patients= null;
				}
			}else
				ModelState.AddModelError("", "Keyword field is required");
			
			return PartialView("_PatientList", patients);
		}

		public ActionResult DischargeList(){
            var NotDischargeVisit = db.Visits.Where(v => !v.IsPay);

            var patients = from p in NotDischargeVisit
                           select p.Patient;


			return View("Index", patients);
		}

        public ActionResult Discharge(int id)
        {
            if(db.Patients.Find(id)== null)
				return RedirectToAction("_404", "Error");

			Visit CurrVisit = db.Visits.FirstOrDefault(v => v.PatientId == id && !v.IsPay);
            
			if(CurrVisit== null)
				return RedirectToAction("_404", "Error");

            Random Cost = new Random();
            ViewBag.TreatmentAmount = Math.Round(Cost.NextDouble()*1000 + 1, 2);

            DateTime today = DateTime.Now;
            TimeSpan DaysOfStay = today.Subtract(CurrVisit.DateOfVisit);
			
			double bedRate= 0;

			if(CurrVisit.Bed!= null)
				bedRate= (double)CurrVisit.Bed.RatePerDay;

			ViewBag.BedRate = Math.Round(((double)DaysOfStay.TotalDays * bedRate), 2);
			
            ViewBag.Total = ViewBag.TreatmentAmount + ViewBag.BedRate;
            
            return View("Discharge", CurrVisit);
        }

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Discharge(Visit visit, int id){
			var v= db.Visits.FirstOrDefault(_v=> _v.PatientId== id);
			
			if(v!= null){
				v.IsPay= true;

				if(visit.DateOfDischarge== null)
					v.DateOfDischarge= DateTime.Now;

				db.Entry(v).State = EntityState.Modified;
				db.SaveChanges();
			}

			return RedirectToAction("DischargeList");
		}

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}