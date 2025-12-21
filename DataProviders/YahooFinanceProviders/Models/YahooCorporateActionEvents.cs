using System.Text.Json.Serialization;

namespace DataProviders.YahooFinanceProviders.Models
{
    internal sealed class YahooCorporateActionEvents
    {
        [JsonPropertyName("dividends")]
        public Dictionary<string, YahooDividendEvent>? Dividends { get; set; }

        [JsonPropertyName("splits")]
        public Dictionary<string, YahooSplitEvent>? Splits { get; set; }

        [JsonPropertyName("earnings")]
        public Dictionary<string, YahooEarningsEvent>? Earnings { get; set; }
    }
}
