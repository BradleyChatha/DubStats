using Backend.Database;
using Backend.Misc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Services
{
    public interface IWeekManager
    {
        Task<Week> GetWeekByDayAsync(DateTime day);
        Task<Week> GetCurrentWeekAsync();
        Task<IQueryable<Week>> GetWeekRangeAsync(DateTime startWeek, uint weeksBefore, uint weeksAfter);
    }

    public sealed class WeekManager : IWeekManager
    {
        readonly DubStatsContext _db;

        public WeekManager(DubStatsContext db)
        {
            this._db = db;
        }

        public Task<Week> GetCurrentWeekAsync()
        {
            return this.GetWeekByDayAsync(DateTime.UtcNow);
        }

        public async Task<Week> GetWeekByDayAsync(DateTime day)
        {
                day   = day.Date; // Ignore hh:mm:ss
            var start = day.StartOfWeek();
            var end   = day.EndOfWeek();

            var result = await this._db.Weeks.FirstOrDefaultAsync(w => w.WeekStart == start && w.WeekEnd == end);
            if (result == null)
            {
                result = new Week
                {
                    WeekStart = start,
                    WeekEnd = end
                };
                this._db.Add(result);
                await this._db.SaveChangesAsync();
            }

            return result;
        }

        public async Task<IQueryable<Week>> GetWeekRangeAsync(DateTime startWeek, uint weeksBefore, uint weeksAfter)
        {
            var week = await this.GetWeekByDayAsync(startWeek);

            var weekRangeStart = week.WeekStart.AddDays(-(7 * weeksBefore));
            var weekRangeEnd   = week.WeekEnd.AddDays(7 * weeksAfter);

            return this._db.Weeks
                           .OrderBy(w => w.WeekStart)
                           .Where(w => w.WeekStart >= weekRangeStart && w.WeekEnd <= weekRangeEnd);
        }
    }
}
