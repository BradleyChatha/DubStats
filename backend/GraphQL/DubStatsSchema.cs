using GraphQL.Types;
using GraphQL.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.GraphQL
{
    public class DubStatsSchema : Schema
    {
        public DubStatsSchema(IServiceProvider provider) : base(provider)
        {
            base.Query = provider.GetRequiredService<DubStatsQuery>();
        }
    }
}
