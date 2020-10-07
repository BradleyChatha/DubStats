using Backend.Database;
using Backend.Misc;
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
    public interface IStatFetcher
    {
        Task FetchPackagesAsync();
    }

    public sealed class DubRegistryStatFetcher : IStatFetcher
    {
        readonly HttpClient      _client;
        readonly DubStatsContext _db;
        readonly ILogger         _logger;

        public DubRegistryStatFetcher(DubStatsContext db, ILogger<DubRegistryStatFetcher> logger)
        {
            this._db = db;
            this._logger = logger;

            this._client = new HttpClient();
            this._client.BaseAddress = Constants.STAT_FETCHER_BASE_ADDRESS;
            this._client.DefaultRequestHeaders.Add("user-agent", Constants.STAT_FETCHER_USER_AGENT);
        }

        public async Task FetchPackagesAsync()
        {
            return; // I've used enough of their bandwith fucking about here, I'm just gonna go a different route for now.

            var cache = await this._db.GetPackageCacheAsync();

            this._logger.LogInformation("Fetching packages.");

            if(cache.NextFetch > DateTimeOffset.UtcNow && cache.Data != "N/A")
            {
                this._logger.LogTrace("Using cached JSON data.");
                await this.HandlePackageDump(JsonDocument.Parse(cache.Data));
                return;
            }

            this._logger.LogTrace("Performing fetch over HTTP.");
            using(var response = await this._client.GetAsync(Constants.STAT_FETCHER_URL_PACKAGE_DUMP))
            {
                if(!response.IsSuccessStatusCode)
                {
                    this._logger.LogError("Request returned status code {Status}: {Reason}", response.StatusCode, response.ReasonPhrase);
                    return;
                }

                cache.NextFetch = DateTimeOffset.UtcNow.AddDays(Constants.PACKAGE_DUMP_RATE_LIMIT_IN_DAYS);
                cache.Data      = await response.Content.ReadAsStringAsync();
                this._db.Update(cache);
                await this._db.SaveChangesAsync();

                var json = JsonDocument.Parse(cache.Data);
                await this.HandlePackageDump(json);
            }
        }

        private async Task HandlePackageDump(JsonDocument document)
        {
            this._logger.LogTrace("Handling package dump JSON.");
        }
    }
}
