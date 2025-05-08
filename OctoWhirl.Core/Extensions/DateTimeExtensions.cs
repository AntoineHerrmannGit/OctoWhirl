namespace OctoWhirl.Core.Extensions
{
    public static class DateTimeExtensions
    {
        public static long ToUnixTimestamp(this DateTime dateTime) => ((DateTimeOffset)dateTime).ToUnixTimeSeconds();

        public static double GetYearFraction(this DateTime date, DateTime maturity)
        {
            double secondsToMaturity = (maturity - date).TotalSeconds;
            double secondsInAYear = (date - date.AddYears(-1)).TotalSeconds;
            return secondsToMaturity / secondsInAYear;
        }
    }
}