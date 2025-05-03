using OctoWhirl.Core.Models.Technicals;

namespace OctoWhirl.Services.Data.Clients.FinnHubClient
{
    public static class FinnHubResolutionIntervalParser
    {
        public static string ToString(this ResolutionInterval resolution)
        {
            return resolution switch
            {
                ResolutionInterval.Minute1 => "1",
                ResolutionInterval.Minute5 => "5",
                ResolutionInterval.Minute15 => "15",
                ResolutionInterval.Minute30 => "30",
                ResolutionInterval.Hour1 => "60",
                ResolutionInterval.Hour2 => "120",
                ResolutionInterval.Hour6 => "360",
                ResolutionInterval.Hour12 => "720",
                ResolutionInterval.Day => "D",
                ResolutionInterval.Week => "W",
                ResolutionInterval.Month => "M",
                _ => throw new ArgumentException($"Invalid value: {resolution}")
            };
        }

        public static ResolutionInterval FromString(string resolution)
        {
            return resolution switch
            {
                "1" => ResolutionInterval.Minute1,
                "5" => ResolutionInterval.Minute5,
                "15" => ResolutionInterval.Minute15,
                "30" => ResolutionInterval.Minute30,
                "60" => ResolutionInterval.Hour1,
                "120" => ResolutionInterval.Hour2,
                "360" => ResolutionInterval.Hour6,
                "720" => ResolutionInterval.Hour12,
                "D" => ResolutionInterval.Day,
                "W" => ResolutionInterval.Week,
                "M" => ResolutionInterval.Month,
                _ => throw new ArgumentException($"Invalid value: {resolution}")
            };
        }
    }
}
