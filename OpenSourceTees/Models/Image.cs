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

        [Required]
        public string ImageUrl { get; set; }

        public string UserId { get; set; }

        [Required]
        public string DesignName { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public double Price { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}