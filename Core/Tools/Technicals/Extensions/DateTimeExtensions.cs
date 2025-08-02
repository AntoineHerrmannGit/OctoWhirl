namespace Technicals.Extensions
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
    }
}