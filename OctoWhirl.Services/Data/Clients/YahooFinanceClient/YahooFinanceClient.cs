using Microsoft.Extensions.Configuration;
using OctoWhirl.Core.Models.Common;
using OctoWhirl.Core.Models.Technicals;
using OctoWhirl.Services.Models.Requests;
using System.Configuration;

namespace OctoWhirl.Services.Data.Clients.YahooFinanceClient
{
    public class YahooFinanceClient : BaseClient, IFinanceClient
    {
        private string _chartUrl;

        public YahooFinanceClient(HttpClient httpClient, IConfiguration configuration) : base(httpClient)
        {
            InitializeClient(configuration);
        }

        #region BaseClient Methods
        protected override void InitializeClient(IConfiguration configuration)
        {
            var section = configuration.GetRequiredSection("Services").GetRequiredSection("YahooFinance");

            _chartUrl = section.GetRequiredSection("ChartUrl").Get<string>() ?? throw new ConfigurationErrorsException("Services:YahooFinance:ChartUrl");
        }
        #endregion BaseClient Methods

        #region YahooFinanceClient Methods
        public async Task<List<Candle>> GetStocks(GetStocksRequest request)
        {
            var period1 = new DateTimeOffset(request.StartDate).ToUnixTimeSeconds();
            var period2 = new DateTimeOffset(request.EndDate).ToUnixTimeSeconds();

            string resolution = YahooFinanceIntervalResolutionParser.ToString(request.Interval);

            var tasks = request.Tickers.Select(async ticker =>
            {
                var candle = await GetSingleStock(ticker, period1, period2, resolution).ConfigureAwait(false);
                return candle;
            }).ToList();

            var results = await Task.WhenAll(tasks).ConfigureAwait(false);
            var candles = results.SelectMany(result => result).ToList();
            return candles;
        }

        public Task<List<Candle>> GetOption(string reference, double strike, DateTime maturity, OptionType optionType, DateTime startDate, DateTime endDate, ResolutionInterval interval = ResolutionInterval.Day)
        {
            throw new NotImplementedException();
        }

        public Task<List<Option>> GetListedOptions(string symbol, DateTime date)
        {
            throw new NotImplementedException();
        }

        #endregion YahooFinanceClient Methods

        #region Private Methods
        private async Task<List<Candle>> GetSingleStock(string ticker, long startDate, long endDate, string interval)
        {
            var url = $"{_chartUrl}/{ticker}?period1={startDate}&period2={endDate}&interval={interval}";

            try
            {
                var result = await CallClient<YahooChartResponse>(url).ConfigureAwait(false);

                var timestamps = result?.Chart?.Result?.FirstOrDefault()?.Timestamp;
                var indicators = result?.Chart?.Result?.FirstOrDefault()?.Indicators?.Quote?.FirstOrDefault();

                var quotes = new List<Candle>();
                if (timestamps == null || indicators == null)
                    return quotes;

                for (int i = 0; i < timestamps.Count; i++)
                {
                    quotes.Add(new Candle
                    {
                        Timestamp = DateTimeOffset.FromUnixTimeSeconds(timestamps[i]).DateTime,
                        Reference = ticker,
                        Open = indicators.Open?[i] ?? 0,
                        High = indicators.High?[i] ?? 0,
                        Low = indicators.Low?[i] ?? 0,
                        Close = indicators.Close?[i] ?? 0,
                        Volume = (int)(indicators.Volume?[i] ?? 0)
                    });
                }

                return quotes;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur: {ex.Message}");
                return new List<Candle>();
            }
        }
        #endregion Private Methods

        #region Yahoo Class
        class YahooChartResponse
        {
            public ChartRoot? Chart { get; set; }
        }

        class ChartRoot
        {
            public List<ChartResult>? Result { get; set; }
        }

        class ChartResult
        {
            public List<long>? Timestamp { get; set; }
            public IndicatorGroup? Indicators { get; set; }
        }

        class IndicatorGroup
        {
            public List<QuoteIndicators>? Quote { get; set; }
        }

        class QuoteIndicators
        {
            public List<double?>? Open { get; set; }
            public List<double?>? High { get; set; }
            public List<double?>? Low { get; set; }
            public List<double?>? Close { get; set; }
            public List<long?>? Volume { get; set; }
        }

        class HistoricalQuote
        {
            public DateTime Date { get; set; }
            public double? Open { get; set; }
            public double? High { get; set; }
            public double? Low { get; set; }
            public double? Close { get; set; }
            public long? Volume { get; set; }
        }
            #endregion Yahoo Class
    }
}
