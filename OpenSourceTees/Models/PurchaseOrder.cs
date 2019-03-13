using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        [Required(ErrorMessage = "You must be signed in to purchase!")]
        [ForeignKey("ApplicationUser")]
        public string BuyerId { get; set; }
        
        [ForeignKey("Image")]
        public string ImageId { get; set; }

        public virtual Image Image { get; set; }
        
        public virtual ApplicationUser ApplicationUser { get; set; }

        public PurchaseOrder()
        {

        }

        //"ItemPrice,Quantity,TotalPrice,ImageId,BuyerId")
        public PurchaseOrder(double itemPrice, int qty, double totalPrice, string image, string buyer)
        {
            TotalPrice = totalPrice;

            ItemPrice = itemPrice;

            Quantity = qty;

            BuyerId = buyer;

            ImageId = image;
        }

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