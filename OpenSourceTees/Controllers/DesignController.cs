using Microsoft.AspNet.Identity;
using OpenSourceTees.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http.Cors;
using System.Web.Mvc;

namespace OpenSourceTees.Controllers
{
    public class DesignController : Controller
    {
        BlobUtility utility;
        ApplicationDbContext db;
        string ContainerName = ConfigurationManager.AppSettings["BlobStorageBlobName"];
        public DesignController()
        {
            utility = new BlobUtility();
            db = new ApplicationDbContext();
        }

        // GET: Design
        public ActionResult Index()
        {
            string loggedInUserId = User.Identity.GetUserId();
            List<Image> userImages = (from r in db.Images where r.UserId == loggedInUserId select r).ToList();
            ViewBag.PhotoCount = userImages.Count;
            if (Request.IsAjaxRequest())
                return PartialView(userImages);

            return RedirectToAction("Index", "Home");
        }

        // Post
        public ActionResult DeleteImage(string id)
        {
            if (Request.IsAjaxRequest()){
                Image userImage = db.Images.Find(id);
                db.Images.Remove(userImage);
                db.SaveChanges();
                string BlobNameToDelete = userImage.ImageUrl.Split('/').Last();
                utility.DeleteBlob(BlobNameToDelete, "blobs");
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index", "Home");

        }

        // GET
        [HttpGet]
        public ActionResult UploadImage()
        {
            if (Request.IsAjaxRequest())
                return PartialView();

            return RedirectToAction("Index", "Home");
        }

        // GET
        [HttpPost]
        public ActionResult UploadImage(TeeShirtUploadViewModel tee)
        {
            if (Request.IsAjaxRequest())
            {
                
                if (tee.File != null)
                { 
                    tee.File = tee.File ?? Request.Files["file"];
                    string fileName = Path.GetFileName(tee.File.FileName);
                    Stream imageStream = tee.File.InputStream;
                    var result = utility.UploadBlob(fileName, ContainerName, imageStream);
                    if (result != null)
                    {
                        string loggedInUserId = User.Identity.GetUserId();
                        Image userimage = new Image();
                        userimage.Id = new Random().Next().ToString();
                        userimage.ImageUrl = result.Uri.ToString();
                        userimage.UserId = loggedInUserId;
                        userimage.Description = tee.Image.Description;
                        userimage.DesignName = tee.Image.DesignName;
                        userimage.Price = tee.Image.Price;
                        db.Images.Add(userimage);
                        db.SaveChanges();
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return PartialView(tee);
                    }
                }
                else
                {
                    return PartialView(tee);
                }
            }
            return RedirectToAction("Index", "Home");

        }

        // POST
        [EnableCors(origins: "https://opensourceteeblob.blob.core.windows.net/", headers: "*", methods: "*")]
        public ActionResult Search(string keywords, int? SkipN, int? TakeN)
        {
            //Console.WriteLine(db.udf_imageSearch(keywords, SkipN, TakeN).ToList());
            
            //var SearchList = from m in db.udf_imageSearch(keywords, SkipN, TakeN)
            //                 select m;
            //var SearchList = from m in db.Images
            //                 select m;
            if (TakeN == 0 || TakeN == null)
            {
                TakeN = 10;
            }
            if (SkipN == null || SkipN == 10)
            {
                SkipN = 0;
            }
            if (String.IsNullOrEmpty(keywords) && Request.IsAjaxRequest())
            {
                return PartialView(from s in db.Images
                            select new RankedEntity<Image> { Entity = s, Rank = 1 });
            }
            var SearchList = from s in db.Images

                             join fts in db.udf_imageSearch(keywords, SkipN, TakeN) on s.Id equals fts.Id 

                             select new RankedEntity<Image>
                             {
                                 Entity = s,
                                 Rank = fts.Ranking
                             };
            if(keywords.Length > 3)
            {
                var list = SearchList;
            }
            if (Request.IsAjaxRequest())
                return PartialView(SearchList.ToList());


            return RedirectToAction("Index", "Home");
        }

        // GET
        public ActionResult EditImage(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Image image = db.Images.Find(id);
            if (image == null)
            {
                return HttpNotFound();
            }
            if (Request.IsAjaxRequest())
                return PartialView(image);


            return RedirectToAction("Index", "Home");
        }

        // POST: Images/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditImage([Bind(Include = "Id,ImageUrl,UserId,DesignName,Description,Price")] Image image)
        {
            if (ModelState.IsValid)
            {
                db.Entry(image).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index", "Home");
            }

            return PartialView(image);
        }

        // GET: Images/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Image image = db.Images.Find(id);
            if (image == null)
            {
                return HttpNotFound();
            }
            if (Request.IsAjaxRequest())
                return PartialView(image);


            return RedirectToAction("Index", "Home");
        }

        // GET
        public ActionResult Hot()
        {
            var list = (from i in db.Images
                        join po in db.PurchaseOrders on i.Id equals po.ImageId into mi
                        orderby mi.Count() descending
                        select i).Take(20).ToList();

            if (Request.IsAjaxRequest())
                return PartialView(list);

            return RedirectToAction("Index", "Home");
        }

        // GET
        public ActionResult New()
        {

            var list = (from i in db.Images
                        orderby i.CreatedDate descending
                        select i).Take(20).ToList();

            if (Request.IsAjaxRequest())
                return PartialView(list);

            return RedirectToAction("Index", "Home");
        }
        // GET
        public ActionResult ByUser(string id)
        {
            List<Image> userImages = (from r in db.Images where r.UserId == id select r).ToList();
            ViewBag.PhotoCount = userImages.Count;
            if (Request.IsAjaxRequest())
                return PartialView(userImages);
            ViewBag.UserName = db.Users.Find(id).UserName;

            return RedirectToAction("Index", "Home");
        }
    }
}