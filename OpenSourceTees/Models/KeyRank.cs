using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OpenSourceTees.Models
{
    [ComplexType]
    public class KeyRank
    {
        public virtual string Id { get; set; }

        public virtual int Rank { get; set; }
    }
}