namespace Core.Technicals.Extensions
{
    public static class DateTimeExtensions
    {
        public static bool IsWeekend(this DateTime date)
            => date.DayOfWeek == DayOfWeek.Sunday || date.DayOfWeek == DayOfWeek.Saturday;

        public static string ToDateString(this DateTime dateTime)
            => dateTime.ToString("yyyy-MM-dd");

        public static IEnumerable<DateTime> ForEachDateUntil(this DateTime startDate, DateTime endDate, bool includeWeekends = false, TimeSpan? increment = null)
        {
            var step = increment ?? TimeSpan.FromDays(1);
            var date = startDate;
            while (date <= endDate)
            {
                if (date.IsWeekend() && !includeWeekends)
                {
                    date += step;
                    continue;
                }
                yield return date;
                date += step;
            }
        }
    }
}
