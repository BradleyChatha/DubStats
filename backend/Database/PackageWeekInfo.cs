using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Database
{
    public class PackageWeekInfo
    {
        [Key]
        public int PackageWeekInfoId { get; set; }

        public int PackageId { get; set; }
        public Package Package { get; set; }

        public int WeekId { get; set; }
        public Week Week { get; set; }
        
        public int PackageStatsAtStartId { get; set; }
        public PackageStats PackageStatsAtStart { get; set; }

        public int PackageStatsAtEndId { get; set; }
        public PackageStats PackageStatsAtEnd { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }
    }
}
