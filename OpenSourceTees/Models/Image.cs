using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OpenSourceTees.Models
{
    /// <summary>
    /// object representation of an image that was uploaded
    /// </summary>
    public class Image
    {
        /// <summary>
        /// string representation of a the Id of the image
        /// </summary>
        [Key]
        public string Id { get; set; }

        /// <summary>
        /// string URL/URI of the actual image
        /// </summary>
        [Required]
        public string ImageUrl { get; set; }

        /// <summary>
        /// UserId of the poster of image, foreign key
        /// </summary>
        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }

        /// <summary>
        /// image designs name
        /// </summary>
        [Required]
        public string DesignName { get; set; }

        /// <summary>
        /// image designs description
        /// </summary>
        [Required]
        public string Description { get; set; }

        /// <summary>
        /// the price of the design
        /// </summary>
        [Required]
        public double Price { get; set; }

        /// <summary>
        /// date the design was created
        /// </summary>
        [Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// foreign key of UserId
        /// </summary>
        public virtual ApplicationUser ApplicationUser { get; set; }

        /// <summary>
        /// gets the designers UserName and returns it
        /// </summary>
        /// <returns>designers UserName</returns>
        public string GetDesignerUserName()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return (from user in db.Users
                    where (user.Id == UserId)
                    select user.UserName).ToList()[0].ToString();
        }
    }
}