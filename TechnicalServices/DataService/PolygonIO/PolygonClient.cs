using Microsoft.Extensions.Configuration;
using OctoWhirl.Core.Models.Exceptions;
using OctoWhirl.Core.Models.Models.Common;
using OctoWhirl.Core.Models.Models.Requests;
using OctoWhirl.Core.Tools.AsyncTools;
using OctoWhirl.Core.Tools.Technicals.Extensions;

namespace OctoWhirl.TechnicalServices.DataService.PolygonIO
{
    public class PolygonClient : BaseClient, IFinanceService
    {
        private readonly string _apiKey;
        private readonly string _chartUrl;
        private readonly string _optionUrl;
        private readonly string _corporateActionUrl;

        private const int _listedOptionLimit = 1000;
        private const int _maxNbOfCandles = 50000;
        private const int _retryDelay = 60000;
        private const int _retryAttempts = 3;

        public PolygonClient(HttpClient httpClient, IConfiguration configuration) : base(httpClient)
        {
            var section = configuration.GetRequiredSection("Services").GetRequiredSection("Polygon");

            _apiKey = section.GetRequiredSection("ApiKey").Get<string>();
            _chartUrl = section.GetRequiredSection("ChartUrl").Get<string>();
            _optionUrl = section.GetRequiredSection("OptionUrl").Get<string>();
            _corporateActionUrl = section.GetRequiredSection("CorporateActionUrl").Get<string>();
        }

        #region IFinanceClient Methods
        public async Task<List<Candle>> GetCandles(GetCandlesRequest request)
        {
            string startDate = request.StartDate.ToDateString();
            string endDate = request.EndDate.ToDateString();

            string interval = request.Interval.GetInterval();
            int amplitude = request.Interval.GetAmplitude();

            var tasks = request.References.Select(ticker => GetSingleStock(ticker, startDate, endDate, interval, amplitude));

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

            var tasks = request.References.Select(ticker => GetSingleOption(
                ticker, from, to, interval, amplitude, request.Strike, request.Maturity, request.OptionType
            ));

            var results = await Task.WhenAll(tasks).ConfigureAwait(false);
            var candles = results.SelectMany(result => result).ToList();

            return candles;
        }

        public async Task<List<Option>> GetListedOptions(GetListedOptionRequest request)
        {
            var tasks = request.References.Select(reference => GetListedOptionForSingleReference(reference, request.AsOfDate));
            var results = await Task.WhenAll(tasks).ConfigureAwait(false);

            var options = results.SelectMany(result => result).ToList();
            return options;
        }

        public async Task<List<Split>> GetSplits(GetCorporateActionsRequest request)
        {
            var startDate = request.StartDate.ToDateString();
            var endDate = request.EndDate.ToDateString();

            var tasks = request.Tickers.Select(ticker => GetSingleSplit(ticker, startDate, endDate)).ToList();
            var results = await Task.WhenAll(tasks).ConfigureAwait(false);

            var splits = results.SelectMany(result => result).ToList();

            return splits;
        }

        public async Task<List<Dividend>> GetDividends(GetCorporateActionsRequest request)
        {
            var startDate = request.StartDate.ToDateString();
            var endDate = request.EndDate.ToDateString();

            var tasks = request.Tickers.Select(ticker => GetSingleDividend(ticker, startDate, endDate)).ToList();
            var results = await Task.WhenAll(tasks).ConfigureAwait(false);

            var dividends = results.SelectMany(result => result).ToList();

            return dividends;
        }
        #endregion IFinanceClient Methods


        #region Private Methods
        #region Private Unitary Methods
        private async Task<IEnumerable<Candle>> GetSingleStock(string ticker, string startDate, string endDate, string interval, int amplitude)
        {
            string url = CreateRequestUrl(startDate, endDate, interval, amplitude, ticker, _maxNbOfCandles);
            var response = await ExecutePolygonRequest<PolygonResponse<PolygonCandle>>(url).ConfigureAwait(false);

            if (response.results.IsNullOrEmpty())
                throw new BadStatusException($"Polygon failed on request {url} : {response.message}");

            var candles = MapToCandles(ticker, response);
            return candles;
        }

        private async Task<IEnumerable<Candle>> GetSingleOption(string ticker, string startDate, string endDate, string interval, int amplitude, double strike, DateTime maturity, OptionType optionType)
        {
            var option = CreateOptionReference(ticker, strike, maturity, optionType);
            var url = CreateRequestUrl(startDate, endDate, interval, amplitude, option, _listedOptionLimit);

            var response = await ExecutePolygonRequest<PolygonResponse<PolygonCandle>>(url).ConfigureAwait(false);
            if (response.results.IsNullOrEmpty())
                throw new BadStatusException($"Polygon failed on request {url} : {response.message}");

            return MapToCandles(option, response);
        }

        private async Task<List<Option>> GetListedOptionForSingleReference(string reference, DateTime asOfDate)
        {
            var url = CreateOptionListURL(reference, asOfDate, _listedOptionLimit);
            var response = await CallClient<PolygonResponse<PolygonOption>>(url).ConfigureAwait(false);
            if (response.results.IsNullOrEmpty())
                throw new BadStatusException($"Polygon failed on request {url} : {response.message}");

            var options = MapToOption(response);
            while (!response.next_url.IsNullOrEmpty())
            {
                url = $"{response.next_url}&apiKey={_apiKey}&limit={_listedOptionLimit}";
                response = await ExecutePolygonRequest<PolygonResponse<PolygonOption>>(url).ConfigureAwait(false);

                if (response.results.IsNullOrEmpty())
                    throw new BadStatusException($"Polygon failed on request {url} : {response.message}");

                options = options.Concat(MapToOption(response));
            }

            return options.ToList();
        }

        private async Task<List<Split>> GetSingleSplit(string ticker, string startDate, string endDate)
        {
            var url = BuildCorporateActionUrl(ticker, startDate, endDate, CorporateActionType.Split);
            var response = await ExecutePolygonRequest<PolygonCorporateActionsResponse>(url).ConfigureAwait(false);

            var splits = response.results.Select(result =>
            {
                var split = MapToSplit(result);
                split.Reference = ticker;
                return split;
            }).ToList();

            return splits;
        }

        private async Task<List<Dividend>> GetSingleDividend(string ticker, string startDate, string endDate)
        {
            var url = BuildCorporateActionUrl(ticker, startDate, endDate, CorporateActionType.Dividend);
            var response = await ExecutePolygonRequest<PolygonCorporateActionsResponse>(url).ConfigureAwait(false);

            var dividends = response.results.Select(result =>
            {
                var dividend = MapToDividend(result);
                dividend.Reference = ticker;
                return dividend;
            }).ToList();

            return dividends;
        }

        // Polygon.io has a maximum number of 5 calls per minute so in case of too many calls, we give ourselves an other couple of attempts
        private async Task<T> ExecutePolygonRequest<T>(string url)
        {
            return await TaskTools.Retry(() => CallClient<T>(url), attempts: _retryAttempts, delay: _retryDelay).ConfigureAwait(false);
        }
        #endregion Private Unitary Methods

        #region Privat Url Builders
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

        private string BuildCorporateActionUrl(string ticker, string startDate, string endDate, CorporateActionType corporateActionType)
        {
            int maxPoints = 1000;
            var type = PolygonCorporateActionTypeParser.Parse(corporateActionType);
            return $"{_corporateActionUrl}/{type}?ticker={ticker}&execution_date.gte={startDate}&execution_date.lte={endDate}&limit={maxPoints}&apiKey={_apiKey}";
        }
        #endregion Private Url Builders

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

        private static Split MapToSplit(PolygonCorporateAction polygonCorporateAction)
        {
            return new Split
            {
                TimeStamp = polygonCorporateAction.ex_dividend_date.Value,
                SplitRatio = polygonCorporateAction.split_from.Value / polygonCorporateAction.split_to.Value,
            };
        }

        private static Dividend MapToDividend(PolygonCorporateAction polygonCorporateAction)
        {
            return new Dividend
            {
                TimeStamp = polygonCorporateAction.ex_dividend_date.Value,
                DividendAmount = polygonCorporateAction.cash_amount.Value,
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
            public string status { get; set; }

            public string request_id { get; set; }

            public string next_url { get; set; }

            public List<PolygonCorporateAction> results { get; set; }
        }

        class PolygonCorporateAction
        {
            public string id { get; set; }

            public string ticker { get; set; }

            public string ca_type { get; set; }

            public double? cash_amount { get; set; }

            public string currency { get; set; }

            public DateTime? declaration_date { get; set; }

            public string dividend_type { get; set; }

            public DateTime? ex_dividend_date { get; set; }

            public int? frequency { get; set; }

            public DateTime? pay_date { get; set; }

            public DateTime? record_date { get; set; }

            public string description { get; set; }

            public string notes { get; set; }

            public double? split_from { get; set; }

            public double? split_to { get; set; }

            public string old_ticker { get; set; }

            public string new_ticker { get; set; }

            public string target_ticker { get; set; }
        }
        #endregion Private Class
    }
}
