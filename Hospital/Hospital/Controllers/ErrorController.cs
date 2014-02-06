using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hospital.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Index(){
			return RedirectToAction("_403");
        }
		
		public ActionResult _401(){
			Response.StatusCode= 401;
			ViewBag.HeaderText= "401 Unathorized";
			ViewBag.Message= "The requested resource requires user authentication";

			return View("Index");
		}


		public ActionResult _403(){
			Response.StatusCode= 403;
			ViewBag.HeaderText= "403 Forbidden";
			ViewBag.Message= "You don't have permission to access";

			return View("Index");
		}

		public ActionResult _404(){
			Response.StatusCode= 404;
			ViewBag.HeaderText= "404 Not Found";
			ViewBag.Message= "The resource cannot be found";

			return View("Index");
		}

		
		public ActionResult _500(){
			Response.StatusCode= 500;
			ViewBag.HeaderText= "500 Internal Error";
			ViewBag.Message= "The server encountered unexpected error";

			return View("Index");
		}
    }
}
