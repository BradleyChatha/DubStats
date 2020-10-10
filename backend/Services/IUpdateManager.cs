using Backend.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Services
{
    public interface IUpdateManager
    {
        Task CreateUpdateJobsForPackageAsync(Package package, Week week);
        Task<ScheduledPackageUpdate> GetNextUpdateAsync(int? lastId);
        Task FinaliseUpdateAsync(ScheduledPackageUpdate update);
    }

    public sealed class UpdateManager : IUpdateManager
    {
        readonly ILogger         _logger;
        readonly DubStatsContext _db;

        public UpdateManager(DubStatsContext db, ILogger<UpdateManager> logger)
        {
            this._db = db;
            this._logger = logger;
        }

        public Task CreateUpdateJobsForPackageAsync(Package package, Week week)
        {
            this._logger.LogInformation(
                $"Creating weekly update jobs for package '{package.Name}' for week starting on {week.WeekStart}"
            );

            var jobs = new[] 
            {
                new ScheduledPackageUpdate(),
                new ScheduledPackageUpdate()
            };

            foreach(var job in jobs)
            {
                job.Week = week;
                job.Package = package;
            }

            jobs[0].Milestone = PackageUpdateMilestone.StartOfWeek;
            jobs[1].Milestone = PackageUpdateMilestone.EndOfWeek;

            this._db.AddRange(jobs);
            return this._db.SaveChangesAsync();
        }

        public Task FinaliseUpdateAsync(ScheduledPackageUpdate update)
        {
            this._db.Remove(update);
            return this._db.SaveChangesAsync();
        }

        public Task<ScheduledPackageUpdate> GetNextUpdateAsync(int? lastId)
        {
            return this._db.PackageUpdates
                           .Include(u => u.Week)
                           .Include(u => u.Package)
                           .OrderBy(u => u.ScheduledPackageUpdateId)
                           .FirstOrDefaultAsync(u => u.ScheduledPackageUpdateId > (lastId ?? 0));
        }
    }
}
