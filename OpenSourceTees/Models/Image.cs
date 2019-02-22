using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OpenSourceTees.Models
{
    public class Image
    {
        public string Id { get; set; }

        public string ImageUrl { get; set; }

        public string UserId { get; set; }

        public string DesignName { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}