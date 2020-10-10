using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Database
{
    public class Package
    {
        [Key]
        public int PackageId { get; set; }

        [Required]
        [StringLength(128)]
        public string Name { get; set; }

        public ICollection<PackageWeekInfo> WeekInfos { get; set; }
    }
}
