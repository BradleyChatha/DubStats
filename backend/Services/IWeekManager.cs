using Backend.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Services
{
    public interface IWeekManager
    {
        Task<Week> GetCurrentWeekAsync();
    }

    public sealed class WeekManager : IWeekManager
    {
        readonly DubStatsContext _db;

        public WeekManager(DubStatsContext db)
        {
            this._db = db;
        }

        public async Task<Week> GetCurrentWeekAsync()
        {
            var today = DateTime.UtcNow.Date;
            var start = today.AddDays(-(int)today.DayOfWeek);
            var end   = today.AddDays((int)DayOfWeek.Saturday - (int)today.DayOfWeek);

            var result = await this._db.Weeks.FirstOrDefaultAsync(w => w.WeekStart == start && w.WeekEnd == end);
            if(result == null)
            {
                result = new Week 
                {
                    WeekStart = start,
                    WeekEnd   = end
                };
                this._db.Add(result);
                await this._db.SaveChangesAsync();
            }

            return result;
        }
    }
}
