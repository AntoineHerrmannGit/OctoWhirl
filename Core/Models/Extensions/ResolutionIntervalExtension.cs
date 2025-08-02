using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OctoWhirl.Core.Models.Technicals;

namespace OctoWhirl.Core.Models.Extensions
{
    public static class ResolutionIntervalExtension
    {
        public static double AsYearFraction(this ResolutionInterval interval)
        {
            var minutesInAYear = (DateTime.Now.AddYears(1) - DateTime.Now).TotalMinutes;
            var minuteAsYearFraction = 1.0 / minutesInAYear;
            return interval switch
            {
                ResolutionInterval.Minute1 => minuteAsYearFraction,
                ResolutionInterval.Minute5 => 5 * minuteAsYearFraction,
                ResolutionInterval.Minute15 => 15 * minuteAsYearFraction,
                ResolutionInterval.Minute30 => 30 * minuteAsYearFraction,
                ResolutionInterval.Hour1 => 60 * minuteAsYearFraction,
                ResolutionInterval.Hour2 => 2 * 60 * minuteAsYearFraction,
                ResolutionInterval.Hour6 => 2 * 60 * minuteAsYearFraction,
                ResolutionInterval.Hour12 => 2 * 60 * minuteAsYearFraction,
                ResolutionInterval.Day => 60 * 24 * minuteAsYearFraction,
                ResolutionInterval.Week => 7 * 60 * 24 * minuteAsYearFraction,
                ResolutionInterval.Month => 30.5 * 60 * 24 * minuteAsYearFraction,
                _ => throw new ArgumentOutOfRangeException(nameof(interval), interval, null)
            };
        }

        public static TimeSpan AsTimeSpan(this ResolutionInterval interval)
        {
            return interval switch
            {
                ResolutionInterval.Minute1 => TimeSpan.FromMinutes(1),
                ResolutionInterval.Minute5 => TimeSpan.FromMinutes(5),
                ResolutionInterval.Minute15 => TimeSpan.FromMinutes(15),
                ResolutionInterval.Minute30 => TimeSpan.FromMinutes(30),
                ResolutionInterval.Hour1 => TimeSpan.FromHours(1),
                ResolutionInterval.Hour2 => TimeSpan.FromHours(2),
                ResolutionInterval.Hour6 => TimeSpan.FromHours(6),
                ResolutionInterval.Hour12 => TimeSpan.FromHours(12),
                ResolutionInterval.Day => TimeSpan.FromDays(1),
                ResolutionInterval.Week => TimeSpan.FromDays(7),
                ResolutionInterval.Month => DateTime.Now.AddMonths(1) - DateTime.Now.AddMonths(1),
                _ => throw new ArgumentOutOfRangeException(nameof(interval), interval, null)
            };
        }
    }
}
