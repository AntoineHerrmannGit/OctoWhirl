using System.Text.Json.Serialization;

namespace DataProviders.YahooFinanceProviders.Models
{
    internal sealed class YahooDividendEvent
    {
        [JsonPropertyName("amount")]
        public double Amount { get; set; }

        [JsonPropertyName("date")]
        public long Date { get; set; }
    }
}
