using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OpenSourceTees.Models
{
    public class HomePageModel
    {
        public List<Image> HotFeed { get; set; }

        public List<Image> NewFeed { get; set; }

        public HomePageModel()
        {
            HotFeed = new List<Image>();
            NewFeed = new List<Image>();
        }
    }
}