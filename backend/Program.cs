using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.HostedServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureServices(builder => 
                {
                    builder.AddHostedService<PackageUpdaterService>();
                    builder.AddHostedService<EnsurePackageUpdatesService>();
                });
    }
}
