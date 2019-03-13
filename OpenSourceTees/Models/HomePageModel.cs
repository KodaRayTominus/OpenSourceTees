using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OpenSourceTees.Models
{
    /// <summary>
    /// object representation of the HomePageModel
    /// </summary>
    public class HomePageModel
    {
        /// <summary>
        /// List of images to be displayed in the "Hot Feed"
        /// </summary>
        public List<Image> HotFeed { get; set; }

        /// <summary>
        /// List of images to be displayed in the "New Feed"
        /// </summary>
        public List<Image> NewFeed { get; set; }

        /// <summary>
        /// no arg constructor
        /// </summary>
        public HomePageModel()
        {
            HotFeed = new List<Image>();
            NewFeed = new List<Image>();
        }
    }
}