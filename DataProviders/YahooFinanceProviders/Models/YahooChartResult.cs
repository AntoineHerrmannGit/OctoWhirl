using System.Text.Json.Serialization;

namespace DataProviders.YahooFinanceProviders.Models
{
    internal sealed class YahooChartResult
    {
        [JsonPropertyName("timestamp")]
        public List<long>? Timestamp { get; set; }
        [JsonPropertyName("indicators")]
        public YahooIndicatorGroup? Indicators { get; set; }

        [JsonPropertyName("events")]
        public YahooCorporateActionEvents? Events { get; set; }
    }
}
