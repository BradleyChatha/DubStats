using Backend.Services;
using GraphQL.Types;
using GraphQL.Utilities;
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
        }
    }
}
