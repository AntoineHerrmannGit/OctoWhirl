using System.Text.Json.Serialization;

namespace DataProviders.YahooFinanceProviders.Models
{
    internal sealed class YahooEarningsEvent
    {
        [JsonPropertyName("date")]
        public long Date { get; set; }

        [JsonPropertyName("earningsDate")]
        public List<long> EarningsDate { get; set; }
    }
}
