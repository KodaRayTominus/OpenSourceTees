using OpenSourceTees.Models;
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

        ApplicationDbContext db;

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
            db = new ApplicationDbContext();

            var model = new HomePageModel()
            {
                NewFeed = (from i in db.Images
                           select i).Reverse().Take(6).Reverse().ToList(),

                HotFeed = (from i in db.Images
                           join po in db.PurchaseOrders on i.Id equals po.ImageId into mi
                           orderby mi.Count() descending
                           select i).Take(3).ToList()
            };

            //var products = (from product in Products
            //                join item in items on product equals item.Product into matchingItems
            //                orderby matchingItems.Sum(oi => oi.Qty)
            //                select product).Take(10);

            if (Request.IsAjaxRequest())
                return PartialView(model);

            return RedirectToAction("Index", "Home");
        }
    }
}