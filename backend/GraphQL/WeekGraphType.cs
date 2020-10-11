using Backend.Database;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.GraphQL
{
    public class WeekGraphType : ObjectGraphType<Week>
    {
        public WeekGraphType()
        {
            Field<DateTimeGraphType>(
                "start",
                resolve: ctx => ctx.Source.WeekStart
            );
            Field<DateTimeGraphType>(
                "end",
                resolve: ctx => ctx.Source.WeekEnd
            );
        }
    }
}
