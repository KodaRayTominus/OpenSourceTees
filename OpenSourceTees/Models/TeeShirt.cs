using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OpenSourceTees.Models
{
    public class TeeShirt
    {
        string DesignName { get; set; }
        string[] Tags { get; set; }
        string DesignerName { get; set; }
        string DesignerId { get; set; }
        string Description { get; set; }
        Image design { get; set; }
    }
}