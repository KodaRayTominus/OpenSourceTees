using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Cors;
using System.Web.Mvc;

namespace OpenSourceTees.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            if (Request.IsAjaxRequest())
                return PartialView();

            return RedirectToAction("Index", "Home"); ;
        }

        [Authorize]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            if (Request.IsAjaxRequest())
                return PartialView();

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Home()
        {
            if (Request.IsAjaxRequest())
                return PartialView();

            return RedirectToAction("Index", "Home");
        }
    }
}