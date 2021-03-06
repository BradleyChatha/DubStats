using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Backend.Database;
using Backend.Services;
using Microsoft.EntityFrameworkCore;
using GraphQL.Server;
using Microsoft.Extensions.Logging;
using Backend.GraphQL;
using GraphQL.Server.Ui.Playground;

namespace Backend
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DubStatsContext>();
            services.AddScoped<IPackageManager, DubRegistryStatFetcher>();
            services.AddScoped<IWeekManager, WeekManager>();
            services.AddScoped<IUpdateManager, UpdateManager>();
            services.AddSingleton<DubStatsSchema>();
            services.AddGraphQL(o => 
            {
#if DEBUG
                o.UnhandledExceptionDelegate = context => 
                { 
                    context.ErrorMessage = context.OriginalException.ToString();
                };
#endif
            })
            .AddSystemTextJson()
            .AddGraphTypes();
            services.AddMvc()
                    .AddRazorRuntimeCompilation();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DubStatsContext db, IPackageManager p)
        {
            db.Database.Migrate();

            var package = p.GetPackageByNameOrNullAsync("jcli").Result;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseStatusCodePages();
            }
            else
            {
                app.UseHsts();
            }
            
            app.UseStaticFiles();
            app.UseGraphQL<DubStatsSchema>("/graphql");
            app.UseGraphQLPlayground(new GraphQLPlaygroundOptions
            {
                PlaygroundSettings = new Dictionary<string, object> {
                    { "request.credentials", "same-origin" }
                }
            });
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
