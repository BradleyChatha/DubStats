using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Misc
{
    public static class MiscExtentions
    {
        public static DateTime StartOfWeek(this DateTime day)
        {
            return day.AddDays(-(int)day.DayOfWeek);
        }

        public static DateTime EndOfWeek(this DateTime day)
        {
            return day.AddDays((int)DayOfWeek.Saturday - (int)day.DayOfWeek);
        }
    }
}
