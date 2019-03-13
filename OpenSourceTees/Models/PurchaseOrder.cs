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
        /// <summary>
        /// Primary Key of the purchase order object
        /// </summary>
        [Key]
        public string Id { get; set; }

        /// <summary>
        /// total price of the purchase order
        /// </summary>
        [Required]
        public double TotalPrice { get; set; }

        /// <summary>
        /// individual item price
        /// </summary>
        [Required]
        public double ItemPrice { get; set; }

        /// <summary>
        /// quantity of the items on the order
        /// </summary>
        [Required]
        public int Quantity { get; set; }

        /// <summary>
        /// Buyers Id, foreign key
        /// </summary>
        [Required(ErrorMessage = "You must be signed in to purchase!")]
        [ForeignKey("ApplicationUser")]
        public string BuyerId { get; set; }
        
        /// <summary>
        /// Image Id, foreign key
        /// </summary>
        [ForeignKey("Image")]
        public string ImageId { get; set; }

        /// <summary>
        /// foreign key image
        /// </summary>
        public virtual Image Image { get; set; }
        
        /// <summary>
        /// foreign key user
        /// </summary>
        public virtual ApplicationUser ApplicationUser { get; set; }

        /// <summary>
        /// no arg constructor
        /// </summary>
        public PurchaseOrder()
        {

        }
        
        /// <summary>
        /// Object representation of a Purchase Order
        /// </summary>
        /// <param name="itemPrice">price of the item</param>
        /// <param name="qty">wuantity of item</param>
        /// <param name="totalPrice">total price of the order</param>
        /// <param name="image">string id of the image</param>
        /// <param name="buyer">string if of the buyer</param>
        public PurchaseOrder(double itemPrice, int qty, double totalPrice, string image, string buyer)
        {
            TotalPrice = totalPrice;

            ItemPrice = itemPrice;

            Quantity = qty;

            BuyerId = buyer;

            ImageId = image;
        }

        /// <summary>
        /// Object representation of a Purchase Order
        /// </summary>
        /// <param name="itemPrice">price of the item</param>
        /// <param name="qty">wuantity of item</param>
        /// <param name="buyer">string if of the buyer</param>
        /// <param name="image">string id of the image</param>
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