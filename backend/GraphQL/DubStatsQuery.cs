using GraphQL;
using GraphQL.Types;
using GraphQL.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.GraphQL
{
    public class DubStatsQuery : ObjectGraphType
    {
        public DubStatsQuery()
        {
            Field<DubStatsPackageSubQuery>(
                "packages",
                resolve: ctx => ctx.RequestServices.GetRequiredService<DubStatsPackageSubQuery>()
            );
        }
    }
}
