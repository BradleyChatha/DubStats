using Backend.Database;
using Backend.Services;
using GraphQL;
using GraphQL.Types;
using GraphQL.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.GraphQL
{
    public class PackageGraphType : ObjectGraphType<Package>
    {
        public PackageGraphType()
        {
            Field<StringGraphType>(
                "Name",
                "The package's name",
                resolve: ctx => ctx.Source.Name
            );

            FieldAsync<ListGraphType<PackageWeekInfoGraphType>>(
                "WeekInfo",
                "The weekly info for this package",
                new QueryArguments(
                    // There's no making this look nice, it's either too vertical, too horizontal, or otherwise too cluttered.
                    new QueryArgument<NonNullGraphType<DateTimeGraphType>> { Name = "dayOfWeek", Description = "One of the days of the week to start at." },
                    new QueryArgument<NonNullGraphType<UIntGraphType>>     { Name = "prevWeeks", Description = "How many previous weeks (from the starting week) to get." },
                    new QueryArgument<NonNullGraphType<UIntGraphType>>     { Name = "nextWeeks", Description = "How many weeks after the starting week to get." }
                ),
                resolve: async ctx => 
                {
                    var db    = ctx.RequestServices.GetRequiredService<DubStatsContext>();
                    var weeks = ctx.RequestServices.GetRequiredService<IWeekManager>();

                    // GetWeekRangeAsync guarentees chronological order.
                    var query = await weeks.GetWeekRangeAsync(
                        ctx.GetArgument<DateTime>("dayOfWeek"),
                        ctx.GetArgument<uint>("prevWeeks"),
                        ctx.GetArgument<uint>("nextWeeks")
                    );

                    return query.SelectMany(w => w.WeekInfos)
                                .Where(wi => wi.PackageId == ctx.Source.PackageId)
                                .Include(wi => wi.PackageStatsAtEnd)
                                .Include(wi => wi.PackageStatsAtStart)
                                .Include(wi => wi.Week)
                                .AsEnumerable();
                }
            );
        }
    }
}
