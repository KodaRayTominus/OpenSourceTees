using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
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
        public async Task<ActionResult> Create([Bind(Include = "ItemPrice,Quantity,TotalPrice,ImageId,BuyerId")] PurchaseOrder purchaseOrder)
        {
            if (ModelState.IsValid)
            {
                //create order processing object
                CreateOrder(purchaseOrder.Id);

                //send emails
                await SendConfirmationEmails(purchaseOrder);

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

        /// <summary>
        /// sends emails to buyer and creator confirming the order
        /// </summary>
        /// <param name="purchaseOrder">purchase order emails need to be sent out on</param>
        /// <returns>object representation of the task handled</returns>
        private async Task SendConfirmationEmails(PurchaseOrder purchaseOrder)
        {
            EmailService email = new EmailService();

            IdentityMessage buyerMessage = new IdentityMessage
            {
                Destination = purchaseOrder.ApplicationUser.Email,
                Body = "",
                Subject = "Your Order!"
            };

            //send email to buyer
            await email.SendAsync(buyerMessage);


            IdentityMessage sellerMessage = new IdentityMessage()
            {
                Destination = db.Users.Find(db.Images.Find(purchaseOrder.ImageId).UserId).Email,
                Body = "",
                Subject = "Someone Placed an Order!"

            };

            //send email to seller
            await email.SendAsync(sellerMessage);
        }

        /// <summary>
        /// creates the OrderProcess object to keep track of the order
        /// </summary>
        /// <param name="orderId">Id of the order </param>
        private void CreateOrder(string orderId)
        {
            OrderProcessing order = new OrderProcessing()
            {
                OrderId = orderId,
                IsAccepted = false,
                IsCanceled = false,
                IsDelivered = false,
                IsProcessed = false,
                IsShipped = false,
                IsEmailSent = false
            };

            db.OrderProcessings.Add(order);
            db.SaveChanges();
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
