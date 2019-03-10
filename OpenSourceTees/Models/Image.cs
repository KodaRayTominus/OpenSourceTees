using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OpenSourceTees.Models
{
    public class Image
    {
        [Key]
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

        [Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedDate { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public string GetDesignerUserName()
        {
            ApplicationDbContext db = new ApplicationDbContext();

            return (from user in db.Users
                    where (user.Id == UserId)
                    select user.UserName).ToList()[0].ToString();
        }
    }
}