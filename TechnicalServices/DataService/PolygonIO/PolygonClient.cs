using Microsoft.Extensions.Configuration;
using OctoWhirl.Core.Extensions;
using OctoWhirl.Core.Models.Common;
using OctoWhirl.Core.Models.Exceptions;
using OctoWhirl.Core.Models.Requests;
using OctoWhirl.Services.Models.Requests;
using Technicals.Extensions;

namespace OctoWhirl.TechnicalServices.DataService.PolygonIO
{
    public class PolygonClient : BaseClient, IFinanceService
    {
        private readonly string _apiKey;
        private readonly string _chartUrl;
        private readonly string _optionUrl;
        private readonly string _corporateActionUrl;

        public PolygonClient(HttpClient httpClient, IConfiguration configuration) : base(httpClient)
        {
            var section = configuration.GetRequiredSection("Services").GetRequiredSection("Polygon");

            _apiKey = section.GetRequiredSection("ApiKey").Get<string>() ?? throw new InvalidOperationException("ApiKey is required");
            _chartUrl = section.GetRequiredSection("ChartUrl").Get<string>() ?? throw new InvalidOperationException("ChartUrl is required");
            _optionUrl = section.GetRequiredSection("OptionUrl").Get<string>() ?? throw new InvalidOperationException("OptionUrl is required");
            _corporateActionUrl = section.GetRequiredSection("CorporateActionUrl").Get<string>() ?? throw new InvalidOperationException("CorporateActionUrl is required");
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
            int maxNbOfResults = 1000;

            var options = new List<Option>();
            foreach (var ticker in request.Tickers)
            {
                var url = CreateOptionListURL(ticker, request.AsOfDate, maxNbOfResults);
                var response = await CallClient<PolygonResponse<PolygonOption>>(url).ConfigureAwait(false);
                if (response.results?.IsNullOrEmpty() != false)
                    throw new BadStatusException($"Polygon failed on request {url} : {response.message}");

                options.AddRange(MapToOption(response));
            }

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
            // Max limit authorized by Polygon.io
            var maxNbOfCandles = 50000;

            string url = CreateRequestUrl(startDate, endDate, interval, amplitude, ticker, maxNbOfCandles);
            var response = await CallClient<PolygonResponse<PolygonCandle>>(url).ConfigureAwait(false);
            if (response.results?.IsNullOrEmpty() != false)
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
            if (response.results?.IsNullOrEmpty() != false)
                throw new BadStatusException($"Polygon failed on request {url} : {response.message}");

            return MapToCandles(option, response);
        }

        private async Task<List<Split>> GetSingleSplit(string ticker, string startDate, string endDate)
        {
            var url = BuildCorporateActionUrl(ticker, startDate, endDate, CorporateActionType.Split);
            var response = await CallClient<PolygonCorporateActionsResponse>(url).ConfigureAwait(false);

            var splits = response.Results?.Select(result => 
            {
                var split = MapToSplit(result);
                split.Reference = ticker;
                return split;
            }).ToList() ?? [];

            return splits;
        }

        private async Task<List<Dividend>> GetSingleDividend(string ticker, string startDate, string endDate)
        {
            var url = BuildCorporateActionUrl(ticker, startDate, endDate, CorporateActionType.Dividend);
            var response = await CallClient<PolygonCorporateActionsResponse>(url).ConfigureAwait(false);

            var dividends = response.Results?.Select(result =>
            {
                var dividend = MapToDividend(result);
                dividend.Reference = ticker;
                return dividend;
            }).ToList() ?? [];

            return dividends;
        }
        #endregion Private Unitary Methods

        #region Privat Url Builders
        private string CreateRequestUrl(string from, string to, string interval, int amplitude, string ticker, int maxNbOfCandles) 
            => CreateUrlDescription(ticker, from, to, interval, amplitude) + "?" + CreateUrlParameters(maxNbOfCandles);

        private string CreateUrlParameters(int maxNbOfCandles) 
            => $"adjusted=false&sort=asc&limit={maxNbOfCandles}&apiKey={_apiKey}";

        private string CreateUrlDescription(string ticker, string from, string to, string interval, int amplitude)
            => $"{_chartUrl}/{ticker}/range/{amplitude}/{interval}/{from}/{to}";

        private string CreateOptionReference(string ticker, double strike, DateTime maturity, OptionType optionType)
        {
            var type = optionType == OptionType.Call ? "C" : optionType == OptionType.Put ? "P" : throw new NotImplementedException(nameof(optionType));
            var strikeString = (strike * 1000).ToString().PadLeft(8, '0');
            return $"O:{ticker}{maturity:yyMMdd}{type}{strikeString}";
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
            => response.results?.Select(result => new Candle
            {
                Timestamp = result.t.FromUnixTimestamp(),
                Open = result.o,
                High = result.h,
                Low = result.l,
                Close = result.c,
                Volume = (int)result.v,
                Reference = ticker,
                Currency = string.Empty
            }) ?? [];

        private static IEnumerable<Option> MapToOption(PolygonResponse<PolygonOption> response)
            => response.results?.Select(result => new Option
            {
                Underlying = result.UnderlyingTicker?.Replace("O:", "") ?? "",
                Strike = result.StrikePrice ?? 0,
                Reference = result.Ticker ?? "",
                OptionType = result.ContractType == "call" ? OptionType.Call : OptionType.Put,
                Maturity = DateTime.TryParse(result.ExpirationDate, out DateTime date) ? date : default,
            }) ?? [];

        private static Split MapToSplit(PolygonCorporateAction polygonCorporateAction)
        {
            return new Split
            {
                TimeStamp = polygonCorporateAction.ExDividendDate ?? default,
                SplitRatio = (polygonCorporateAction.SplitFrom ?? 0) / (polygonCorporateAction.SplitTo ?? 1),
            };
        }

        private static Dividend MapToDividend(PolygonCorporateAction polygonCorporateAction)
        {
            return new Dividend
            {
                TimeStamp = polygonCorporateAction.ExDividendDate ?? default,
                DividendAmount = polygonCorporateAction.CashAmount ?? 0,
            };
        }
        #endregion Mappers
        #endregion Private Methods

        #region Private Class
        class PolygonResponse<T>
        {
            public string? ticker { get; set; }
            public int queryCount { get; set; }
            public int resultCount { get; set; }
            public bool adjusted { get; set; }
            public List<T>? results { get; set; }
            public string? status { get; set; }
            public string? request_id { get; set; }
            public int count { get; set; }
            public string? message { get; set; }
            public string? next_url { get; set; }
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
            public string? Cfi { get; set; }
            public string? ContractType { get; set; }
            public string? ExerciseStyle { get; set; }
            public string? ExpirationDate { get; set; }
            public string? PrimaryExchange { get; set; }
            public string? SharesPerContract { get; set; }
            public double? StrikePrice { get; set; }
            public string? Ticker { get; set; }
            public string? UnderlyingTicker { get; set; }
        }

        class PolygonCorporateActionsResponse
        {
            public string? Status { get; set; }

            public string? RequestId { get; set; }

            public string? NextUrl { get; set; }

            public List<PolygonCorporateAction>? Results { get; set; }
        }

        class PolygonCorporateAction
        {
            public string? Id { get; set; }

            public string? Ticker { get; set; }

            public string? CaType { get; set; }

            public double? CashAmount { get; set; }

            public string? Currency { get; set; }

            public DateTime? DeclarationDate { get; set; }

            public string? DividendType { get; set; }

            public DateTime? ExDividendDate { get; set; }

            public int? Frequency { get; set; }

            public DateTime? PayDate { get; set; }

            public DateTime? RecordDate { get; set; }

            public string? Description { get; set; }

            public string? Notes { get; set; }

            public double? SplitFrom { get; set; }

            public double? SplitTo { get; set; }

            public string? OldTicker { get; set; }

            public string? NewTicker { get; set; }

            public string? TargetTicker { get; set; }
        }
        #endregion Private Class
    }
}
