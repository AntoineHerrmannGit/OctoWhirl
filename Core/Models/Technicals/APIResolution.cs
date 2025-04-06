namespace OctoWhirl.Core.Models.Technicals
{
    public enum APIResolution
    {
        Minute1,
        Minute5,
        Minute15,
        Minute30,
        Hour1,
        Hour2,
        Hour6,
        Hour12,
        Day,
        Week,
        Month,
    }

    public static class APIResolutionParser
    {
        public static string ToString(this APIResolution resolution)
        { 
            return resolution switch
            {
                APIResolution.Minute1 => "1",
                APIResolution.Minute5 => "5",
                APIResolution.Minute15 => "15",
                APIResolution.Minute30 => "30",
                APIResolution.Hour1 => "60",
                APIResolution.Hour2 => "120",
                APIResolution.Hour6 => "360",
                APIResolution.Hour12 => "720",
                APIResolution.Day => "D",
                APIResolution.Week => "W",
                APIResolution.Month => "M",
                _ => throw new ArgumentException($"Invalid value: {resolution}")
            };
        }

        public static APIResolution FromString(string resolution)
        {
            return resolution switch
            {
                "1" => APIResolution.Minute1,
                "5" => APIResolution.Minute5,
                "15" => APIResolution.Minute15,
                "30" => APIResolution.Minute30,
                "60" => APIResolution.Hour1,
                "120" => APIResolution.Hour2,
                "360" => APIResolution.Hour6,
                "720" => APIResolution.Hour12,
                "D" => APIResolution.Day,
                "W" => APIResolution.Week,
                "M" => APIResolution.Month,
                _ => throw new ArgumentException($"Invalid value: {resolution}")
            };
        }
    }
}
