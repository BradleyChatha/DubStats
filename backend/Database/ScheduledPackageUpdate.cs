using Backend.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Database
{
    public class ScheduledPackageUpdate
    {
        [Key]
        public int ScheduledPackageUpdateId { get; set; }

        public int WeekId { get; set; }
        public Week Week { get; set; }

        public int PackageId { get; set; }
        public Package Package { get; set; }

        public PackageUpdateMilestone Milestone { get; set; }
    }
}
