namespace DataProviders.YahooFinanceProviders.Configuration
{
    internal class YahooFinanceConfiguration
    {
        public string ChartUrl { get; set; }
        public string CorporateActionUrl { get; set; }
        public TimeSpan Timeout { get; set; }
        public Dictionary<string, string> IndexMap { get; set; }
    }
}
