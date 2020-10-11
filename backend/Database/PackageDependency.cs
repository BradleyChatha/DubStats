using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Database
{
    public class PackageDependency
    {
        [Key]
        public int PackageDependencyId { get; set; }

        public int DependantPackageId { get; set; }
        public Package DependantPackage { get; set; }

        public int DependencyPackageId { get; set; }
        public Package DependencyPackage { get; set; }
    }
}
