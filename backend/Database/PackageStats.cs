using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Database
{
    public class PackageStats
    {
        [Key]
        public int PackageStatsId { get; set; }

        public int Downloads { get; set; }
        public int Stars     { get; set; }
        public int Watchers  { get; set; }
        public int Forks     { get; set; }
        public int Issues    { get; set; }

        /// <summary>
        /// Whether this PackageStats has been modified since creation.
        /// </summary>
        public bool HasBeenModified { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }
    }
}
