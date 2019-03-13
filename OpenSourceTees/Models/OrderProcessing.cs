using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OpenSourceTees.Models
{
    public class OrderProcessing
    {
        [Key]
        [ForeignKey("Order")]
        public string OrderId { get; set; }

        [Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string CreateDate { get; set; }

        public bool IsEmailSent { get; set; }

        public bool IsAccepted { get; set; }

        public bool IsProcessed { get; set; }

        [ForeignKey("ApplicationUser")]
        public string ProcessorId { get; set; }

        public bool IsShipped { get; set; }

        public bool IsDelivered { get; set; }

        public bool IsCanceled { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual PurchaseOrder Order { get; set; }

        public string GetProcessorUserName()
        {
            ApplicationDbContext db = new ApplicationDbContext();

            return (from user in db.Users
                    where (user.Id == ProcessorId)
                    select user.UserName).ToList()[0].ToString();
        }
    }
}