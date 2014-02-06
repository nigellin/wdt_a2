using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hospital.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        /* allows the view to access the controller */
        public ActionResult Index()
        {

            return View();
        }

    }
}
