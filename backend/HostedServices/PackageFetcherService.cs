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
    public class PackageFetcherService : BackgroundService
    {
        readonly IServiceProvider _services;
        readonly ILogger          _logger;

        public PackageFetcherService(IServiceProvider services, ILogger<PackageFetcherService> logger)
        {
            this._services = services;
            this._logger   = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using(var scope = this._services.CreateScope())
                    {
                        var fetcher = scope.ServiceProvider.GetRequiredService<IStatFetcher>();
                        await fetcher.FetchPackagesAsync();
                    }
                }
                catch(Exception ex)
                {
                    this._logger.LogError("EXCEPTION THROWN:\n"+ex.Message+"\n"+ex.StackTrace);
                }

                await Task.Delay(Constants.PACKAGE_FETCHER_CHECK_DELAY, stoppingToken);
            }
        }
    }
}
