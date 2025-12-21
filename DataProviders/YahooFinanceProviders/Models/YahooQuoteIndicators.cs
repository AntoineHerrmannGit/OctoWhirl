namespace DataProviders.YahooFinanceProviders.Models
{
    internal sealed class YahooQuoteIndicators
    {
        public List<double?>? Open { get; set; }
        public List<double?>? High { get; set; }
        public List<double?>? Low { get; set; }
        public List<double?>? Close { get; set; }
        public List<long?>? Volume { get; set; }
    }
}
