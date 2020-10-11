using Backend.Database;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.GraphQL
{
    public class PackageWeekInfoGraphType : ObjectGraphType<PackageWeekInfo>
    {
        public PackageWeekInfoGraphType()
        {
            Field<PackageStatsGraphType>(
                "statsStartOfWeek",
                resolve: ctx => ctx.Source.PackageStatsAtStart
            );
            Field<PackageStatsGraphType>(
                "statsEndOfWeek",
                resolve: ctx => ctx.Source.PackageStatsAtEnd
            );
            Field<WeekGraphType>(
                "week",
                resolve: ctx => ctx.Source.Week
            );
        }
    }
}
