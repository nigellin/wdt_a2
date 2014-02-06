using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Hospital.Models;

namespace Hospital.Controllers
{
    public class AccountController : Controller
    {
        private HospitalEntities db = new HospitalEntities();

		public AccountController(){
			Common.Instance.ConnnectionCheck(db);
		}

        /* if user is not logged in method will redirect to loing page */
        public ActionResult Index()
        {
            return RedirectToAction("Login");
        }

        /* this is the functionality for the login requirment */
		public ActionResult Login(string ReturnUrl= ""){
			if(Request.IsAuthenticated)
				return RedirectToAction("Index", "Home");

			ViewBag.ReturnUrl= ReturnUrl;

			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Login(Account account){
			if(ModelState.IsValid)
				if(IsVaildUser(account)){
					FormsAuthentication.SetAuthCookie(account.Username, false);

					/* error checking for login credentials */
                    if(string.IsNullOrEmpty(Request["ReturnUrl"]))
						return RedirectToAction("Index", "Home");
					else
						return Redirect(Request["ReturnUrl"]);

				}else
					ModelState.AddModelError("message", "Invalid username or password");

			return View(account);
		}

        /* logout functionality */
		[Authorize]
		public ActionResult Logout(){
			FormsAuthentication.SignOut();

			return RedirectToAction("Index", "Home");
		}
		
        /* checking for valid input */
		private bool IsVaildUser(Account account){
			if(db.Database.Connection.State!= ConnectionState.Open)
				db.Database.Connection.Open();
			
			var user= db.Accounts.FirstOrDefault(u=> u.Username.Equals(account.Username));

			if(user!= null)
				if(user.Password== account.Password)
					return true;

			return false;
		}

        /* disposing of contents from the database */
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}