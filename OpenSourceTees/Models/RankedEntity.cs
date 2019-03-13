using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OpenSourceTees.Models
{
    /// <summary>
    /// Represents a ranked entity from the Database
    /// </summary>
    /// <typeparam name="E">Type of Entity to be ranked</typeparam>
    public class RankedEntity<E>
    {
        /// <summary>
        /// Ranked entity object
        /// </summary>
        public E Entity { get; set; }

        /// <summary>
        /// Rank if the entity
        /// </summary>
        public int? Rank { get; set; }
    }
}