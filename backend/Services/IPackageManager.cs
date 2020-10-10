using Backend.Database;
using Backend.Misc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Backend.Services
{
    public struct DubStatsRootJson
    {
        public DubStatsDownloadsJson Downloads { get; set; }
        public DubStatsRepoJson Repo { get; set; }
        public double Score { get; set; }
    }

    public struct DubStatsDownloadsJson
    {
        public int Total { get; set; }
        public int Monthly { get; set; }
        public int Weekly { get; set; }
        public int Daily { get; set; }
    }

    public struct DubStatsRepoJson
    {
        public int Stars { get; set; }
        public int Watchers { get; set; }
        public int Forks { get; set; }
        public int Issues { get; set; }
    }

    public enum PackageUpdateMilestone
    {
        StartOfWeek,
        EndOfWeek
    }

    public interface IPackageManager
    {
        Task<Package> GetPackageByNameOrNullAsync(string name);
        Task<PackageWeekInfo> GetPackageStatsAsync(Package package, Week week);
        Task<bool> UpdatePackageAsync(Package package, Week week, PackageUpdateMilestone milestone);
        IQueryable<Package> QueryAll();
    }

    public sealed class DubRegistryStatFetcher : IPackageManager
    {
        readonly HttpClient      _client;
        readonly DubStatsContext _db;
        readonly ILogger         _logger;
        readonly IWeekManager    _weeks;
        readonly IUpdateManager  _updates;

        public DubRegistryStatFetcher(
            DubStatsContext db, 
            ILogger<DubRegistryStatFetcher> logger, 
            IWeekManager weeks,
            IUpdateManager updates
        )
        {
            this._db      = db;
            this._logger  = logger;
            this._weeks   = weeks;
            this._updates = updates;

            this._client = new HttpClient();
            this._client.BaseAddress = Constants.STAT_FETCHER_BASE_ADDRESS;
            this._client.DefaultRequestHeaders.Add("user-agent", Constants.STAT_FETCHER_USER_AGENT);
        }

        public async Task<Package> GetPackageByNameOrNullAsync(string name)
        {
            var result = await this._db.Packages.FirstOrDefaultAsync(p => p.Name == name);
            if(result != null)
                return result;

            result = new Package 
            {
                Name = name
            };

            using(var response = await this._client.GetAsync(Constants.GetPackageStatsUri(result)))
            {
                if(!response.IsSuccessStatusCode)
                    return null;
            }

            this._db.Add(result);
            await this._db.SaveChangesAsync();
            await this._updates.CreateUpdateJobsForPackageAsync(result, await this._weeks.GetCurrentWeekAsync());
            return result;
        }

        public async Task<PackageWeekInfo> GetPackageStatsAsync(Package package, Week week)
        {
            var result = await this._db.WeekInfos
                .Include(w => w.PackageStatsAtStart)
                .Include(w => w.PackageStatsAtEnd)
                .FirstOrDefaultAsync(
                    w => w.WeekId == week.WeekId && w.PackageId == package.PackageId
                );

            if(result == null)
            {
                this._logger.LogInformation(
                    "Creating Week Info for Package {PackageName} for week of {WeekStart}",
                    package.Name, week.WeekStart
                );
                result = new PackageWeekInfo 
                {
                    Package             = package,
                    Week                = week,
                    PackageStatsAtStart = new PackageStats(),
                    PackageStatsAtEnd   = new PackageStats()
                };
                this._db.Add(result);
                await this._db.SaveChangesAsync();
            }

            return result;
        }

        public IQueryable<Package> QueryAll()
        {
            return this._db.Packages;
        }

        public async Task<bool> UpdatePackageAsync(Package package, Week week, PackageUpdateMilestone milestone)
        {
            var info = await this.GetPackageStatsAsync(package, week);

            var stats = 
                (milestone == PackageUpdateMilestone.EndOfWeek)
                ? info.PackageStatsAtEnd
                : info.PackageStatsAtStart;

            if(stats.HasBeenModified)
                return true;

            this._logger.LogInformation("Updating {Milestone} stats for package {PackageName}", milestone, package.Name);

            var dubJson = default(DubStatsRootJson);
            using(var response = await this._client.GetAsync(Constants.GetPackageStatsUri(package)))
            {
                if(!response.IsSuccessStatusCode)
                    return false;

                dubJson = JsonSerializer.Deserialize<DubStatsRootJson>(
                    await response.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions 
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );
            }

            stats.Downloads = dubJson.Downloads.Weekly;
            stats.Forks     = dubJson.Repo.Forks;
            stats.Issues    = dubJson.Repo.Issues;
            stats.Stars     = dubJson.Repo.Stars;
            stats.Watchers  = dubJson.Repo.Watchers;
            
            stats.HasBeenModified = true;
            this._db.Update(stats);
            await this._db.SaveChangesAsync();

            return true;
        }
    }
}
