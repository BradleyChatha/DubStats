using Backend.Database;
using Backend.Misc;
using Backend.Services;
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
    public class PackageUpdaterService : BackgroundService
    {
        readonly IServiceProvider _services;
        readonly ILogger          _logger;

        public PackageUpdaterService(IServiceProvider services, ILogger<PackageUpdaterService> logger)
        {
            this._services = services;
            this._logger   = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
            {
                this._logger.LogInformation("Performing updates.");

                using(var scope = this._services.CreateScope())
                {
                    var updateManager  = scope.ServiceProvider.GetRequiredService<IUpdateManager>();
                    var packageManager = scope.ServiceProvider.GetRequiredService<IPackageManager>();

                    ScheduledPackageUpdate update = await updateManager.GetNextUpdateAsync(null);
                    int? lastId = null;
                    while(update != null)
                    {
                        var updated = await this.HandleUpdate(update, packageManager);
                        if(updated)
                            await updateManager.FinaliseUpdateAsync(update);

                        lastId = update.ScheduledPackageUpdateId;
                        update = await updateManager.GetNextUpdateAsync(lastId);
                        await Task.Delay(Constants.PACKAGE_UPDATE_BETWEEN_DELAY, stoppingToken);
                    }
                }

                await Task.Delay(Constants.PACKAGE_UPDATE_CHECK_DELAY, stoppingToken);
            }
        }

        private async Task<bool> HandleUpdate(
            ScheduledPackageUpdate update,
            IPackageManager packages
        )
        {
            bool shouldUpdate = false;
            switch(update.Milestone)
            {
                case PackageUpdateMilestone.StartOfWeek:
                    shouldUpdate = DateTime.UtcNow.Date >= update.Week.WeekStart;
                    break;

                case PackageUpdateMilestone.EndOfWeek:
                    shouldUpdate = DateTime.UtcNow.Date >= update.Week.WeekEnd;
                    break;

                default: break;
            }

            if(!shouldUpdate)
                return false;

            await packages.UpdatePackageAsync(update.Package, update.Week, update.Milestone);
            return true;
        }
    }
}
