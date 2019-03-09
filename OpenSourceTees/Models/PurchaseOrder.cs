using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OpenSourceTees.Models
{
    public class PurchaseOrder
    {
        public string Id { get; set; }

        public double TotalPrice { get; set; }

        public string BuyerId { get; set; }

        public string ImageId { get; set; }

        public virtual Image Image { get; set; }
        
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}