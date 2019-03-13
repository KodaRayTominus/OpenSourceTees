using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OpenSourceTees.Models
{
    /// <summary>
    /// object representation of a ranking in the database
    /// </summary>
    public class KeyRank
    {
        /// <summary>
        /// string Id representation of the object being ranked
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual string Id { get; set; }

        /// <summary>
        /// ranking of the object being ranked
        /// </summary>
        public virtual int? Ranking { get; set; }
    }
}