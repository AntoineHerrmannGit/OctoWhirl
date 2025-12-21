using System.Text.Json.Serialization;

namespace DataProviders.YahooFinanceProviders.Models
{
    internal sealed class YahooChartResponse
    {
        [JsonPropertyName("chart")]
        public YahooChartRoot? Chart { get; set; }
    }
}
