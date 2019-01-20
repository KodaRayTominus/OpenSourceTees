using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OpenSourceTees.Models
{
    public class TeeShirtUploadViewModel
    {
        public HttpPostedFileBase File { get; set; }
        public Image Image { get; set; }
    }
}