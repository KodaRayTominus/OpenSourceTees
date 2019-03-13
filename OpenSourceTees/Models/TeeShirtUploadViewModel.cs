using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OpenSourceTees.Models
{
    /// <summary>
    /// Represents the modle for uploading a TeeShirt image
    /// </summary>
    public class TeeShirtUploadViewModel
    {
        /// <summary>
        /// file to be uploaded
        /// </summary>
        public HttpPostedFileBase File { get; set; }

        /// <summary>
        /// Image object to be palced in the db
        /// </summary>
        public Image Image { get; set; }
    }
}