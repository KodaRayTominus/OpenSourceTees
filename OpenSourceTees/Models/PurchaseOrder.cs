using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OpenSourceTees.Models
{
    public class PurchaseOrder
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public double TotalPrice { get; set; }

        [Required]
        public double ItemPrice { get; set; }

        [Required]
        public int Quantity { get; set; }

        public string BuyerId { get; set; }

        public string ImageId { get; set; }

        public virtual Image Image { get; set; }
        
        public virtual ApplicationUser ApplicationUser { get; set; }

        public PurchaseOrder(double itemPrice, int qty, string buyer, string image)
        {
            TotalPrice = qty * TotalPrice;

            ItemPrice = itemPrice;

            Quantity = qty;

            BuyerId = buyer;

            ImageId = image;
        }
    }
}