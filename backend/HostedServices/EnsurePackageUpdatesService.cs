using Backend.Database;
using Backend.Misc;
using Backend.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Backend.HostedServices
{
    // It's entirely possible for packages to be left in limbo if we weren't doing this check from time-to-time
    // (e.g. closing the server mid-way through job creation).
    //
    // So that's why this is a service.
    public class EnsurePackageUpdatesService : BackgroundService
    {
        readonly IServiceProvider _services;
        readonly ILogger _logger;

        public EnsurePackageUpdatesService(IServiceProvider services, ILogger<PackageUpdaterService> logger)
        {
            this._services = services;
            this._logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                this._logger.LogInformation("Checking that all packages have been given update jobs for this week.");

                using (var scope = this._services.CreateScope())
                {
                    var updateManager  = scope.ServiceProvider.GetRequiredService<IUpdateManager>();
                    var packageManager = scope.ServiceProvider.GetRequiredService<IPackageManager>();
                    var weekManager    = scope.ServiceProvider.GetRequiredService<IWeekManager>();

                    // Any package that doesn't have at least one update job for this week,
                    // and hasn't had their "EndOfWeek" stats modified, will be packages that
                    // haven't had their jobs created.
                    var thisWeek = await weekManager.GetCurrentWeekAsync();
                    var query = packageManager
                        .QueryAll()
                        .Include(p => p.PackageUpdates)
                            .ThenInclude(pu => pu.Week)
                        .Include(p => p.WeekInfos)
                            .ThenInclude(wi => wi.PackageStatsAtEnd)
                        .Where(p => !p.PackageUpdates.Any(pu => pu.Week == thisWeek))
                        .Where(p => !p.WeekInfos.FirstOrDefault(wi => wi.WeekId == thisWeek.WeekId).PackageStatsAtEnd.HasBeenModified);

                    foreach(var package in query)
                        await updateManager.CreateUpdateJobsForPackageAsync(package, thisWeek);
                }

                await Task.Delay(Constants.PACKAGE_ENSURE_UPDATES_DELAY, stoppingToken);
            }
        }
    }
}
