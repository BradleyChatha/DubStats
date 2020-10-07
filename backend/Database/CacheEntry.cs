using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Database
{
    public class CacheEntry
    {
        public static readonly string CACHE_TYPE_PACKAGE_DUMP = "PackageDump";

        [Key]
        public string Type { get; set; }

        // PackageDump's size will only ever increase, so the string length should be revised eventually.
        [Required]
        [StringLength(1024 * 1024 * 1024 * 1)]
        public string Data { get; set; }

        [Required]
        public DateTimeOffset NextFetch { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }
    }
}
