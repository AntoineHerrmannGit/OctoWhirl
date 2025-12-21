using Models.Models.Enums;

namespace DataProviders.YahooFinanceProviders.Core
{
    internal static class ResolutionIntervalHelper
    {
        public static string ToYahooString(this ResolutionInterval interval)
            => interval switch
            {
                ResolutionInterval.Minute1 => "1m",
                ResolutionInterval.Minute5 => "5m",
                ResolutionInterval.Minute15 => "15m",
                ResolutionInterval.Minute30 => "30m",
                ResolutionInterval.Hour1 => "1h",
                ResolutionInterval.Hour2 => "2h",
                ResolutionInterval.Hour6 => "6h",
                ResolutionInterval.Hour12 => "12h",
                ResolutionInterval.Day => "1d",
                ResolutionInterval.Week => "1w",
                ResolutionInterval.Month => "1mo",
                _ => throw new ArgumentException($"Invalid value: {interval.ToString()}")
            };

        public static ResolutionInterval FromYahooString(this string interval)
            => interval switch
            {
                "1m" => ResolutionInterval.Minute1,
                "5m" => ResolutionInterval.Minute5,
                "15m" => ResolutionInterval.Minute15,
                "30m" => ResolutionInterval.Minute30,
                "1h" => ResolutionInterval.Hour1,
                "2h" => ResolutionInterval.Hour2,
                "6h" => ResolutionInterval.Hour6,
                "12h" => ResolutionInterval.Hour12,
                "1d" => ResolutionInterval.Day,
                "1w" => ResolutionInterval.Week,
                "1mo" => ResolutionInterval.Month,
                _ => throw new ArgumentException($"Invalid value: {interval}")
            };
    }
}
