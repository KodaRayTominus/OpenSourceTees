using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using OpenSourceTees.Models;

namespace OpenSourceTees.Controllers
{
    public class PurchaseOrdersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: PurchaseOrders
        public ActionResult Index()
        {
            var purchaseOrders = from order in db.PurchaseOrders
                                 where(order.BuyerId == User.Identity.GetUserId())
                                 select order;
            if (Request.IsAjaxRequest())
                return PartialView(purchaseOrders.ToList());

            return RedirectToAction("Index", "Home");
        }

        // GET: PurchaseOrders/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchaseOrder purchaseOrder = db.PurchaseOrders.Find(id);
            if (purchaseOrder == null)
            {
                return HttpNotFound();
            }
            if (Request.IsAjaxRequest())
                return PartialView(purchaseOrder);

            return RedirectToAction("Index", "Home");
        }

        // GET: PurchaseOrders/Create
        public ActionResult Create()
        {
            if (Request.IsAjaxRequest())
                return PartialView();

            return RedirectToAction("Index", "Home");
        }

        // POST: PurchaseOrders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ItemPrice,Quantity,TotalPrice,ImageId,BuyerId")] PurchaseOrder purchaseOrder)
        {
            if (ModelState.IsValid)
            {
                //send emails

                db.PurchaseOrders.Add(purchaseOrder);
                db.SaveChanges();

                return RedirectToAction("Index", "Home");
            }

            if (Request.IsAjaxRequest())
                return PartialView(purchaseOrder);

            return RedirectToAction("Index", "Home");
        }

        // GET: PurchaseOrders/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchaseOrder purchaseOrder = db.PurchaseOrders.Find(id);
            if (purchaseOrder == null)
            {
                return HttpNotFound();
            }
            if (Request.IsAjaxRequest())
                return PartialView(purchaseOrder);

            return RedirectToAction("Index", "Home");
        }

        // POST: PurchaseOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            PurchaseOrder purchaseOrder = db.PurchaseOrders.Find(id);
            db.PurchaseOrders.Remove(purchaseOrder);
            db.SaveChanges();
            return RedirectToAction("Index");
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
