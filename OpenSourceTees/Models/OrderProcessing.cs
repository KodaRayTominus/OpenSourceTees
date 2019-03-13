using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OpenSourceTees.Models
{
    /// <summary>
    /// Object representation of the Order Process
    /// </summary>
    public class OrderProcessing
    {
        /// <summary>
        /// Primary key of the Order
        /// </summary>
        [Key]
        [ForeignKey("Order")]
        public string OrderId { get; set; }

        /// <summary>
        /// date the order was created
        /// </summary>
        [Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string CreateDate { get; set; }

        /// <summary>
        /// boolean representation of emails being sent
        /// </summary>
        public bool IsEmailSent { get; set; }

        /// <summary>
        /// boolean representation of order being accepted
        /// </summary>
        public bool IsAccepted { get; set; }

        /// <summary>
        /// boolean representation of order being processed
        /// </summary>
        public bool IsProcessed { get; set; }

        /// <summary>
        /// String Id of the processor handling the order, foreign key
        /// </summary>
        [ForeignKey("ApplicationUser")]
        public string ProcessorId { get; set; }

        /// <summary>
        /// boolean representation of order being shipped
        /// </summary>
        public bool IsShipped { get; set; }

        /// <summary>
        /// boolean representation of order being delivered
        /// </summary>
        public bool IsDelivered { get; set; }

        /// <summary>
        /// boolean representation of order being canceled
        /// </summary>
        public bool IsCanceled { get; set; }

        /// <summary>
        /// foreign key application user of processor
        /// </summary>
        public virtual ApplicationUser ApplicationUser { get; set; }

        /// <summary>
        /// foreign key order of the OrderId
        /// </summary>
        public virtual PurchaseOrder Order { get; set; }

        /// <summary>
        /// gets and returns the Processors UserName
        /// </summary>
        /// <returns>UserName of the processor</returns>
        public string GetProcessorUserName()
        {
            ApplicationDbContext db = new ApplicationDbContext();

            return (from user in db.Users
                    where (user.Id == ProcessorId)
                    select user.UserName).ToList()[0].ToString();
        }
    }
}