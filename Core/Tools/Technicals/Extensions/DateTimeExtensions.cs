namespace OctoWhirl.Core.Tools.Technicals.Extensions
{
    public static class DateTimeExtensions
    {
        public static long ToUnixTimestamp(this DateTime dateTime) => ((DateTimeOffset)dateTime).ToUnixTimeSeconds();

        public static DateTime FromUnixTimestamp(this long unixTimeStamp)
        {
            if (unixTimeStamp > 253402300799)
                return DateTimeOffset.FromUnixTimeMilliseconds(unixTimeStamp).DateTime;
            return DateTimeOffset.FromUnixTimeSeconds(unixTimeStamp).DateTime;
        }

        public static string ToDateString(this DateTime dateTime) => dateTime.ToString("yyyy-MM-dd");

        public static double GetYearFraction(this DateTime date, DateTime maturity)
        {
            double secondsToMaturity = (maturity - date).TotalSeconds;
            double secondsInAYear = (date - date.AddYears(-1)).TotalSeconds;
            return secondsToMaturity / secondsInAYear;
        }

        public static bool IsWeekend(this DateTime date) => date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;

        public static int BusinessDays(this TimeSpan timeSpan, DateTime startDate)
        {
            int businessDays = 0;
            for(DateTime date = startDate; date <= startDate + timeSpan; date = date.AddDays(1))
                if(!date.IsWeekend())
                    businessDays++;
            return businessDays;
        }
    }
}