using Backend.Database;
using Backend.Services;
using GraphQL.Types;
using GraphQL.Types.Relay.DataObjects;
using GraphQL.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.GraphQL
{
    public class DubStatsPackageSubQuery : ObjectGraphType
    {
        public DubStatsPackageSubQuery()
        {
            FieldAsync<PackageGraphType>(
                "single",
                "Retrieve a single package.",
                new QueryArguments(new QueryArgument<NonNullGraphType<StringGraphType>>{ Name = "name" }),
                resolve: async ctx =>
                {
                    var packages = ctx.RequestServices.GetRequiredService<IPackageManager>();

                    return await packages.GetPackageByNameOrNullAsync((string)ctx.Arguments["name"]);
                }
            );

            Connection<PackageGraphType>()
                .Name("multiple")
                .ReturnAll()
                .ResolveAsync(async ctx => 
                {
                    // No particular order, filtering, or parameters right now.
                    var packages = ctx.RequestServices.GetRequiredService<IPackageManager>();
                    
                    var query = packages
                    .QueryAll()
                    .Select(p => new Edge<Package>
                    {
                        Node = p,
                        Cursor = "TODO"
                    })
                    .Take(15); // TEMP limit

                    return new Connection<Package>
                    {
                        Edges = await query.ToListAsync(),

                        PageInfo = new PageInfo 
                        {
                            EndCursor = "TODO",
                            StartCursor = "TODO",
                            HasNextPage = false,
                            HasPreviousPage = false,
                        },

                        TotalCount = 0 // Odd how it can't figure this out manually.
                    };
                }
            );
        }
    }
}
