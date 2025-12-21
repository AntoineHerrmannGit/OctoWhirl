using DataProviders.YahooFincnaceProviders.Models;

namespace DataProviders.YahooFinanceProviders.Models
{
    internal sealed class YahooIndicatorGroup
    {
        public List<YahooQuoteIndicators>? Quote { get; set; }
        public List<YahooAdjCloseIndicators>? AdjClose { get; set; }
    }
}
