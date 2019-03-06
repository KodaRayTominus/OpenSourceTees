﻿using Microsoft.AspNet.Identity;
using OpenSourceTees.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace OpenSourceTees.Controllers
{
    public class DesignController : Controller
    {
        BlobUtility utility;
        ApplicationDbContext db;
        string accountName = "opensourceteeblob";
        string accountKey = "88B22cx6S2sdVOJ2jGWtThftow3LFKA+fpkh+DxBW6Oy48U/0Pn/iuUi3TmHiVl+9vE+4Jq4thqtn6qpaBxY8w==";
        public DesignController()
        {
            utility = new BlobUtility(accountName, accountKey);
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

        [HttpGet]
        public ActionResult UploadImage()
        {
            if (Request.IsAjaxRequest())
                return PartialView();

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult UploadImage(TeeShirtUploadViewModel tee)
        {
            if (Request.IsAjaxRequest())
            {
                
                if (tee.File != null && ModelState.IsValid)
                {
                    string ContainerName = "blobs"; //hardcoded container name. 
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

        public ActionResult Search(string keywords, int? SkipN, int? TakeN)
        {
            //Console.WriteLine(db.udf_imageSearch(keywords, SkipN, TakeN).ToList());

            db = new ApplicationDbContext();
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

            return View(image);
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
    }
}