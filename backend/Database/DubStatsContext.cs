using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Backend.Database
{
    public class DubStatsContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder builder) =>
            builder.UseSqlite($"Data Source={DubStatsContext.PlatformDbFilePath}");
        
        private static string PlatformDbFilePath =>
            (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            ? "/etc/dubstats/db.sqlite"
            : "./db.sqlite";

        public DbSet<CacheEntry>                CacheEntries        { get; set; }
        public DbSet<Week>                      Weeks               { get; set; }
        public DbSet<Package>                   Packages            { get; set; }
        public DbSet<PackageWeekInfo>           WeekInfos           { get; set; }
        public DbSet<PackageStats>              PackageStats        { get; set; }
        public DbSet<ScheduledPackageUpdate>    PackageUpdates      { get; set; }
        public DbSet<PackageDependency>         PackageDependencies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .Entity<PackageWeekInfo>()
                .HasIndex(nameof(PackageWeekInfo.PackageId), nameof(PackageWeekInfo.WeekId))
                .IsUnique();

            modelBuilder
                .Entity<Week>()
                .HasIndex(w => w.WeekStart)
                .IsUnique();
            modelBuilder
                .Entity<Week>()
                .HasIndex(w => w.WeekEnd)
                .IsUnique();

            modelBuilder
                .Entity<Package>()
                .HasIndex(p => p.Name)
                .IsUnique();

            modelBuilder
                .Entity<ScheduledPackageUpdate>()
                .HasIndex(
                    nameof(ScheduledPackageUpdate.PackageId), nameof(ScheduledPackageUpdate.WeekId),
                    nameof(ScheduledPackageUpdate.Milestone)
                )
                .IsUnique();
            modelBuilder
                .Entity<ScheduledPackageUpdate>()
                .Property(u => u.Milestone)
                .HasConversion<string>();

            modelBuilder
                .Entity<PackageDependency>()
                .HasOne(pd => pd.DependencyPackage)
                .WithMany(p => p.PackagesThatDependOnMe);
            modelBuilder
                .Entity<PackageDependency>()
                .HasOne(pd => pd.DependantPackage)
                .WithMany(p => p.PackagesIDependOn);
            modelBuilder
                .Entity<PackageDependency>()
                .HasIndex(nameof(PackageDependency.DependantPackageId), nameof(PackageDependency.DependencyPackageId))
                .IsUnique();
        }

        public async Task<CacheEntry> GetPackageCacheAsync()
        {
            var result = 
                (this.CacheEntries == null)
                ? null
                : await this.CacheEntries.FirstOrDefaultAsync(e => e.Type == CacheEntry.CACHE_TYPE_PACKAGE_DUMP);

            if(result == null)
            {
                result = new CacheEntry() 
                {
                    Type = CacheEntry.CACHE_TYPE_PACKAGE_DUMP,
                    NextFetch = DateTimeOffset.UtcNow,
                    Data = "N/A"
                };
                this.CacheEntries.Add(result);
                await this.SaveChangesAsync();
            }

            return result;
        }
    }
}
