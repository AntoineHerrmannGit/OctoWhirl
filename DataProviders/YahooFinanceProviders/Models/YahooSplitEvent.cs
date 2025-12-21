using System.Text.Json.Serialization;

namespace DataProviders.YahooFinanceProviders.Models
{
    internal sealed class YahooSplitEvent
    {
        [JsonPropertyName("numerator")]
        public double Numerator { get; set; }

        [JsonPropertyName("denominator")]
        public double Denominator { get; set; }

        [JsonPropertyName("splitRatio")]
        public string SplitRatio { get; set; }

        [JsonPropertyName("date")]
        public long Date { get; set; }
    }
}
