using Backend.Database;
using GraphQL;
using GraphQL.Types;
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
        }
    }
}
