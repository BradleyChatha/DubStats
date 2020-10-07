using Backend.Database;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.GraphQL
{
    public class PackageStatsGraphType : ObjectGraphType<PackageStats>
    {
        public PackageStatsGraphType()
        {
            Field<IntGraphType>(
                "Downloads",
                resolve: ctx => ctx.Source.Downloads
            );
            Field<IntGraphType>(
                "Stars",
                resolve: ctx => ctx.Source.Stars
            );
            Field<IntGraphType>(
                "Watchers",
                resolve: ctx => ctx.Source.Watchers
            );
            Field<IntGraphType>(
                "Forks",
                resolve: ctx => ctx.Source.Forks
            );
            Field<IntGraphType>(
                "Issues",
                resolve: ctx => ctx.Source.Issues
            );
        }
    }
}
