using Microsoft.Extensions.Configuration;
using OctoWhirl.Core.Models.Common;
using OctoWhirl.Core.Models.Technicals;
using OctoWhirl.Services.Models.Requests;
using System.Configuration;
using System.Reactive;
using System.Text.Json.Serialization;

namespace OctoWhirl.Services.Data.Clients.YahooFinanceClient
{
    public class YahooFinanceClient : BaseClient, IFinanceClient
    {
        private string _chartUrl;
        private string _corporateActionUrl;

        public YahooFinanceClient(HttpClient httpClient, IConfiguration configuration) : base(httpClient)
        {
            InitializeClient(configuration);
        }

        #region BaseClient Methods
        protected override void InitializeClient(IConfiguration configuration)
        {
            var section = configuration.GetRequiredSection("Services").GetRequiredSection("YahooFinance");

            _chartUrl = section.GetRequiredSection("ChartUrl").Get<string>() ?? throw new ConfigurationErrorsException("Services:YahooFinance:ChartUrl");
            _corporateActionUrl = section.GetRequiredSection("CorporataeActionUrl").Get<string>() ?? throw new ConfigurationErrorsException("Services:YahooFinance:CorporataeActionUrl");
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

        public Task<List<Candle>> GetOption(GetOptionRequest request)
        {
            var type = request.OptionType == OptionType.Call ? "C" : request.OptionType == OptionType.Put ? "P" : throw new NotSupportedException(request.OptionType.ToString());
            var options = request.Tickers.Select(reference => $"{reference}{request.Maturity.ToString("YYmmdd")}{type}{(request.Strike * 1000).ToString().PadLeft(14, '0')}").ToList();
            var stocksRequest = new GetStocksRequest
            {
                Tickers = options,
                EndDate = request.EndDate,
                Interval = request.Interval,
                Source = request.Source,
                StartDate = request.StartDate
            };
            return GetStocks(stocksRequest);
        }

        public Task<List<Option>> GetListedOptions(GetListedOptionRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<List<CorporateAction>> GetCorporateActions(GetCorporateActionsRequest request)
        {
            var startDate = new DateTimeOffset(request.StartDate).ToUnixTimeSeconds();
            var endDate = new DateTimeOffset(request.EndDate).ToUnixTimeSeconds();
            var interval = YahooFinanceIntervalResolutionParser.ToString(ResolutionInterval.Day);

            var tasks = request.Tickers.Select(async ticker =>
            { 
                var corpAction = await GetSingleCorporateAction(ticker, startDate, endDate, interval).ConfigureAwait(false);
                if (request.CorporateActionType != null)
                    return corpAction.Where(coac => coac.ActionType == request.CorporateActionType).ToList();
                else
                    return corpAction;
            }).ToList();

            var results = await Task.WhenAll(tasks).ConfigureAwait(false);
            var corporateActions = results.SelectMany(result => result).ToList();

            return corporateActions;
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

        private async Task<List<CorporateAction>> GetSingleCorporateAction(string ticker, long startDate, long endDate, string interval)
        {
            var url = $"{_corporateActionUrl}/{ticker}?interval={interval}&period1={startDate}&period2={endDate}&events=div|split";

            try
            {
                var result = await CallClient<YahooChartResponse>(url).ConfigureAwait(false);

                var yahooSplits = result?.Chart?.Result?.FirstOrDefault()?.Events?.Splits;
                var yahooDividends = result?.Chart?.Result?.FirstOrDefault()?.Events?.Dividends;

                var corporateActions = new List<CorporateAction>();
                if (yahooSplits != null)
                {
                    var splits = yahooSplits.Values.Select(split => new Split
                    {
                        Reference = ticker,
                        TimeStamp = DateTimeOffset.FromUnixTimeSeconds(split.Date).DateTime,
                        SplitRatio = split.Numerator / split.Denominator
                    });
                    corporateActions.AddRange(splits);
                }

                if (yahooDividends != null)
                {
                    var dividends = yahooDividends.Values.Select(div => new Dividend
                    {
                        Reference = ticker,
                        TimeStamp = DateTimeOffset.FromUnixTimeSeconds(div.Date).DateTime,
                        DividendAmount = div.Amount,
                    });
                    corporateActions.AddRange(dividends);
                }

                return corporateActions;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur: {ex.Message}");
                return new List<CorporateAction>();
            }
        }
        #endregion Private Methods

        #region Yahoo Class
        class YahooChartResponse
        {
            [JsonPropertyName("chart")]
            public ChartRoot? Chart { get; set; }
        }

        class ChartRoot
        {
            [JsonPropertyName("result")]
            public List<ChartResult>? Result { get; set; }

            [JsonPropertyName("error")]
            public string? Error { get; set; }
        }

        class ChartResult
        {
            [JsonPropertyName("timestamp")]
            public List<long>? Timestamp { get; set; }
            [JsonPropertyName("indicators")]
            public IndicatorGroup? Indicators { get; set; }

            [JsonPropertyName("events")]
            public CorporateActionEvents? Events { get; set; }
        }

        class CorporateActionEvents
        {
            [JsonPropertyName("dividends")]
            public Dictionary<string, DividendEvent>? Dividends { get; set; }

            [JsonPropertyName("splits")]
            public Dictionary<string, SplitEvent>? Splits { get; set; }

            [JsonPropertyName("earnings")]
            public Dictionary<string, EarningsEvent>? Earnings { get; set; }
        }

        class DividendEvent
        {
            [JsonPropertyName("amount")]
            public double Amount { get; set; }

            [JsonPropertyName("date")]
            public long Date { get; set; }
        }

        class SplitEvent
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

        class EarningsEvent
        {
            [JsonPropertyName("date")]
            public long Date { get; set; }

            [JsonPropertyName("earningsDate")]
            public List<long> EarningsDate { get; set; }
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
