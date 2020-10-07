using Backend.Database;
using Microsoft.EntityFrameworkCore;
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
        readonly DubStatsContext _db;

        public UpdateManager(DubStatsContext db)
        {
            this._db = db;
        }

        public Task CreateUpdateJobsForPackageAsync(Package package, Week week)
        {
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
