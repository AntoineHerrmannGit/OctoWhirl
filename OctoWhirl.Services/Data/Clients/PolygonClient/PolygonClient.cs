using Microsoft.Extensions.Configuration;
using OctoWhirl.Core.Exceptions;
using OctoWhirl.Core.Extensions;
using OctoWhirl.Core.Models.Common;
using OctoWhirl.Services.Models.Requests;
using System.Reflection.Metadata;
using System.Text.Json.Serialization;

namespace OctoWhirl.Services.Data.Clients.PolygonClient
{
    public class PolygonClient : BaseClient, IFinanceClient
    {
        private readonly string _apiKey;
        private readonly string _chartUrl;
        private readonly string _optionUrl;
        private readonly string _corporateActionUrl;

        public PolygonClient(HttpClient httpClient, IConfiguration configuration) : base(httpClient)
        {
            var section = configuration.GetRequiredSection("Services").GetRequiredSection("Polygon");

            _apiKey = section.GetRequiredSection("ApiKey").Get<string>();
            _chartUrl = section.GetRequiredSection("ChartUrl").Get<string>();
            _optionUrl = section.GetRequiredSection("OptionUrl").Get<string>();
            _corporateActionUrl = section.GetRequiredSection("CorporateActionUrl").Get<string>();
        }

        #region IFinanceClient Methods
        public async Task<List<Candle>> GetStocks(GetStocksRequest request)
        {
            string startDate = request.StartDate.ToDateString();
            string endDate = request.EndDate.ToDateString();

            string interval = request.Interval.GetInterval();
            int amplitude = request.Interval.GetAmplitude();

            var tasks = request.Tickers.Select(ticker => GetSingleStock(ticker, startDate, endDate, interval, amplitude));

            var results = await Task.WhenAll(tasks).ConfigureAwait(false);
            var candles = results.SelectMany(result => result).ToList();

            return candles;
        }

        public async Task<List<Candle>> GetOption(GetOptionRequest request)
        {
            string from = request.StartDate.ToDateString();
            string to = request.EndDate.ToDateString();

            string interval = request.Interval.GetInterval();
            int amplitude = request.Interval.GetAmplitude();

            var tasks = request.Tickers.Select(ticker => GetSingleOption(
                ticker, from, to, interval, amplitude, request.Strike, request.Maturity, request.OptionType
            ));

            var results = await Task.WhenAll(tasks).ConfigureAwait(false);
            var candles = results.SelectMany(result => result).ToList();

            return candles;
        }

        public async Task<List<Option>> GetListedOptions(GetListedOptionRequest request)
        {
            int maxNbOfResults = 1000;

            var options = new List<Option>();
            foreach (var ticker in request.Tickers)
            {
                var url = CreateOptionListURL(ticker, request.AsOfDate, maxNbOfResults);
                var response = await CallClient<PolygonResponse<PolygonOption>>(url).ConfigureAwait(false);
                if (response.results.IsNullOrEmpty())
                    throw new BadStatusException($"Polygon failed on request {url} : {response.message}");

                options.AddRange(MapToOption(response));
            }

            return options;
        }

        public async Task<List<CorporateAction>> GetCorporateActions(GetCorporateActionsRequest request)
        {
            var startDate = request.StartDate.ToDateString();
            var endDate = request.EndDate.ToDateString();

            var tasks = request.Tickers.Select(ticker => GetSingleCorporateAction(ticker, startDate, endDate)).ToList();
            var results = await Task.WhenAll(tasks).ConfigureAwait(false);

            var corporateActions = results.SelectMany(result => result).ToList();

            return corporateActions;
        }
        #endregion IFinanceClient Methods


        #region Private Methods
        #region Private Unitary Methods
        private async Task<IEnumerable<Candle>> GetSingleStock(string ticker, string startDate, string endDate, string interval, int amplitude)
        {
            // Max limit authorized by Polygon.io
            var maxNbOfCandles = 50000;

            string url = CreateRequestUrl(startDate, endDate, interval, amplitude, ticker, maxNbOfCandles);
            var response = await CallClient<PolygonResponse<PolygonCandle>>(url).ConfigureAwait(false);
            if (response.results.IsNullOrEmpty())
                throw new BadStatusException($"Polygon failed on request {url} : {response.message}");

            var candles = MapToCandles(ticker, response);
            return candles;
        }

        private async Task<IEnumerable<Candle>> GetSingleOption(string ticker, string startDate, string endDate, string interval, int amplitude, double strike, DateTime maturity, OptionType optionType)
        {
            // Max limit authorized by Polygon.io
            var maxNbOfCandles = 1000;

            var option = CreateOptionReference(ticker, strike, maturity, optionType);
            var url = CreateRequestUrl(startDate, endDate, interval, amplitude, option, maxNbOfCandles);

            var response = await CallClient<PolygonResponse<PolygonCandle>>(url).ConfigureAwait(false);
            if (response.results.IsNullOrEmpty())
                throw new BadStatusException($"Polygon failed on request {url} : {response.message}");

            return MapToCandles(option, response);
        }

        private async Task<List<CorporateAction>> GetSingleCorporateAction(string ticker, string startDate, string endDate)
        {
            var url = $"{_corporateActionUrl}?ticker={ticker}&execution_date.gte={startDate}&execution_date.lte={endDate}&apiKey={_apiKey}";
            var response = await CallClient<PolygonCorporateActionsResponse>(url).ConfigureAwait(false);

            // Other types of corporate actions are not supported yet as protected in the switch
            var corporateActions = response.Results.Where(result => 
                   result.CorporateActionType == PolygonCorporateActionType.dividend 
                || result.CorporateActionType == PolygonCorporateActionType.split
            ).Select(result =>
            {
                return result.CorporateActionType switch
                {
                    PolygonCorporateActionType.dividend => MapToDividend(result),
                    PolygonCorporateActionType.split => MapToSplit(result),
                    _ => throw new NotSupportedException()
                };
            }).ToList();

            corporateActions.ForEach(c => c.Reference = ticker);

            return corporateActions;
        }
        #endregion Private Unitary Methods

        #region Private Stock Url Builders
        private string CreateRequestUrl(string from, string to, string interval, int amplitude, string ticker, int maxNbOfCandles) 
            => CreateUrlDescription(ticker, from, to, interval, amplitude) + "?" + CreateUrlParameters(maxNbOfCandles);

        private string CreateUrlParameters(int maxNbOfCandles) 
            => $"adjusted=false&sort=asc&limit={maxNbOfCandles}&apiKey={_apiKey}";

        private string CreateUrlDescription(string ticker, string from, string to, string interval, int amplitude)
            => $"{_chartUrl}/{ticker}/range/{amplitude.ToString()}/{interval}/{from}/{to}";

        private string CreateOptionReference(string ticker, double strike, DateTime maturity, OptionType optionType)
        {
            var type = optionType == OptionType.Call ? "C" : optionType == OptionType.Put ? "P" : throw new NotImplementedException(nameof(optionType));
            var strikeString = (strike * 1000).ToString().PadLeft(8, '0');
            return $"O:{ticker}{maturity.ToString("yyMMdd")}{type}{strikeString}";
        }

        private string CreateOptionListURL(string ticker, DateTime asOfDate, int limit)
            => $"{_optionUrl}?underlying_ticker={ticker}&as_of={asOfDate.ToDateString()}&expired=false&order=asc&limit={limit}&sort=ticker&apiKey={_apiKey}";
        #endregion Private Stock Url Builders

        #region Mappers
        private static IEnumerable<Candle> MapToCandles(string ticker, PolygonResponse<PolygonCandle> response)
            => response.results.Select(result => new Candle
            {
                Timestamp = result.t.FromUnixTimestamp(),
                Open = result.o,
                High = result.h,
                Low = result.l,
                Close = result.c,
                Volume = (int)result.v,
                Reference = ticker,
                Currency = null
            });

        private static IEnumerable<Option> MapToOption(PolygonResponse<PolygonOption> response)
            => response.results.Select(result => new Option
            {
                Underlying = result.underlying_ticker.Replace("O:", String.Empty),
                Strike = result.strike_price,
                Reference = result.ticker,
                OptionType = result.contract_type == "call" ? OptionType.Call : OptionType.Put,
                Maturity = DateTime.TryParse(result.expiration_date, out DateTime date) ? date : default,
            });

        private static CorporateAction MapToSplit(PolygonCorporateAction polygonCorporateAction)
        {
            return new Split
            {
                TimeStamp = DateTime.Parse(polygonCorporateAction.ExecutionDate),
                SplitRatio = polygonCorporateAction.SplitFrom.Value / polygonCorporateAction.SplitTo.Value,
            };
        }

        private static CorporateAction MapToDividend(PolygonCorporateAction polygonCorporateAction)
        {
            return new Dividend
            {
                TimeStamp = DateTime.Parse(polygonCorporateAction.ExecutionDate),
                DividendAmount = polygonCorporateAction.CashAmount.Value,
            };
        }
        #endregion Mappers
        #endregion Private Methods

        #region Private Class
        class PolygonResponse<T>
        {
            public string ticker { get; set; }
            public int queryCount { get; set; }
            public int resultCount { get; set; }
            public bool adjusted { get; set; }
            public List<T> results { get; set; }
            public string status { get; set; }
            public string request_id { get; set; }
            public int count { get; set; }
            public string message { get; set; }
            public string next_url { get; set; }
        }

        class PolygonCandle
        {
            public long t { get; set; }
            public double o { get; set; }
            public double h { get; set; }
            public double l { get; set; }
            public double c { get; set; }
            public int n { get; set; }
            public double v { get; set; }
            public double vw { get; set; }
            public bool otc { get; set; }
        }

        class PolygonOption
        {
            public string cfi { get; set; }
            public string contract_type { get; set; }
            public string exercise_style { get; set; }
            public string expiration_date { get; set; }
            public string primary_exchange { get; set; }
            public string shares_per_contract { get; set; }
            public double strike_price { get; set; }
            public string ticker { get; set; }
            public string underlying_ticker { get; set; }
        }

        class PolygonCorporateActionsResponse
        {
            [JsonPropertyName("status")]
            public string Status { get; set; }

            [JsonPropertyName("request_id")]
            public string RequestId { get; set; }

            [JsonPropertyName("next_url")]
            public string NextUrl { get; set; }

            [JsonPropertyName("results")]
            public List<PolygonCorporateAction> Results { get; set; }
        }

        class PolygonCorporateAction
        {
            [JsonPropertyName("id")]
            public string Id { get; set; }

            [JsonPropertyName("ca_type")]
            public PolygonCorporateActionType CorporateActionType { get; set; }

            [JsonPropertyName("execution_date")]
            public string ExecutionDate { get; set; }

            [JsonPropertyName("record_date")]
            public string RecordDate { get; set; }

            [JsonPropertyName("declared_date")]
            public string DeclaredDate { get; set; }

            [JsonPropertyName("payment_date")]
            public string PaymentDate { get; set; }

            [JsonPropertyName("cash_amount")]
            public double? CashAmount { get; set; }

            [JsonPropertyName("ticker")]
            public string Ticker { get; set; }

            [JsonPropertyName("description")]
            public string Description { get; set; }

            [JsonPropertyName("notes")]
            public string Notes { get; set; }

            [JsonPropertyName("split_from")]
            public double? SplitFrom { get; set; }

            [JsonPropertyName("split_to")]
            public double? SplitTo { get; set; }

            [JsonPropertyName("old_ticker")]
            public string OldTicker { get; set; }

            [JsonPropertyName("new_ticker")]
            public string NewTicker { get; set; }

            [JsonPropertyName("target_ticker")]
            public string TargetTicker { get; set; }
        }

        enum PolygonCorporateActionType
        {
            dividend,
            split,
            merger,
            spinoff,
            acquisition,
            name_change,
            symbol_change,
            bankruptcy,
            delisted
        }

        #endregion Private Class
    }
}
