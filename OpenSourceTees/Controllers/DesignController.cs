using Microsoft.AspNet.Identity;
using OpenSourceTees.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            return View(userImages);
        }

        public ActionResult DeleteImage(string id)
        {
            Image userImage = db.Images.Find(id);
            db.Images.Remove(userImage);
            db.SaveChanges();
            string BlobNameToDelete = userImage.ImageUrl.Split('/').Last();
            utility.DeleteBlob(BlobNameToDelete, "blobs");
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UploadImage(HttpPostedFileBase file)
        {
            if (file != null)
            {
                string ContainerName = "blobs"; //hardcoded container name. 
                file = file ?? Request.Files["file"];
                string fileName = Path.GetFileName(file.FileName);
                Stream imageStream = file.InputStream;
                var result = utility.UploadBlob(fileName, ContainerName, imageStream);
                if (result != null)
                {
                    string loggedInUserId = User.Identity.GetUserId();
                    Image userimage = new Image();
                    userimage.Id = new Random().Next().ToString();
                    userimage.UserId = loggedInUserId;
                    userimage.ImageUrl = result.Uri.ToString();
                    db.Images.Add(userimage);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return RedirectToAction("Index");
            }


        }
    }
}