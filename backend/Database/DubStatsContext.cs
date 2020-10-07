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
            ? "/etc/dubstats/db"
            : "./db.sqlite";

        public DbSet<CacheEntry> CacheEntries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
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
