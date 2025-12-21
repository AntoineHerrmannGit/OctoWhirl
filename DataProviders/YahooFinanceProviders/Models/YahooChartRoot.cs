using System.Text.Json.Serialization;

namespace DataProviders.YahooFinanceProviders.Models
{
    internal sealed class YahooChartRoot
    {
        [JsonPropertyName("result")]
        public List<YahooChartResult>? Result { get; set; }

        [JsonPropertyName("error")]
        public string? Error { get; set; }
    }
}
