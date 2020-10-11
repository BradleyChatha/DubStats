using Backend.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Misc
{
    public static class Constants
    {
        public static readonly TimeSpan PACKAGE_UPDATE_CHECK_DELAY   = TimeSpan.FromMinutes(5.0);
        public static readonly TimeSpan PACKAGE_UPDATE_BETWEEN_DELAY = TimeSpan.FromSeconds(15.0);
        public static readonly TimeSpan PACKAGE_ENSURE_UPDATES_DELAY = TimeSpan.FromMinutes(15.0);

        public static readonly Uri STAT_FETCHER_BASE_ADDRESS = new Uri("https://code.dlang.org/", UriKind.Absolute);
        public static readonly Uri STAT_FETCHER_URL_PACKAGE_DUMP = new Uri("/api/packages/dump", UriKind.Relative);

        public static readonly string STAT_FETCHER_USER_AGENT = 
            "DubStats (Automated;" +
            "Contact bradley@chatha.dev if requests are misbehaving, or if you would like this to stop)";

        public static Uri GetPackageStatsUri(Package package)
        {
            return new Uri($"/api/packages/{package.Name}/stats", UriKind.Relative);
        }
    }
}
