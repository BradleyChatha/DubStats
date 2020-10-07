using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Misc
{
    public static class Constants
    {
        public static readonly TimeSpan PACKAGE_FETCHER_CHECK_DELAY = TimeSpan.FromSeconds(60.0);

        public static readonly double PACKAGE_DUMP_RATE_LIMIT_IN_DAYS = 30.0;

        public static readonly Uri STAT_FETCHER_BASE_ADDRESS = new Uri("https://code.dlang.org/", UriKind.Absolute);
        public static readonly Uri STAT_FETCHER_URL_PACKAGE_DUMP = new Uri("/api/packages/dump", UriKind.Relative);

        public static readonly string STAT_FETCHER_USER_AGENT = 
            "DubStats (Automated; Dump fetch limited to once per 30 days;" +
            "Package fetch limited to one package every 15 seconds, once per 30 days;" +
            "Contact bradley@chatha.dev if requests are misbehaving, or if you would like this to stop)";
    }
}
