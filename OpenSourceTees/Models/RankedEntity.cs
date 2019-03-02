using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OpenSourceTees.Models
{
    public class RankedEntity<E>
    {
        public E Entity { get; set; }

        public int Rank { get; set; }
    }
}