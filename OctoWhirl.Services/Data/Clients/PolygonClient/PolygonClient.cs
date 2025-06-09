using Microsoft.Extensions.Configuration;
using OctoWhirl.Core.Exceptions;
using OctoWhirl.Core.Extensions;
using OctoWhirl.Core.Models.Common;
using OctoWhirl.Services.Models.Requests;

namespace OctoWhirl.Services.Data.Clients.PolygonClient
{
    public class PolygonClient : BaseClient, IFinanceClient
    {
        private readonly string _apiKey;
        private readonly string _chartUrl;
        private readonly string _optionUrl;

        public PolygonClient(HttpClient httpClient, IConfiguration configuration) : base(httpClient)
        {
            _apiKey = configuration.GetRequiredSection("Services").GetRequiredSection("Polygon").GetRequiredSection("ApiKey").Get<string>();
            _chartUrl = configuration.GetRequiredSection("Services").GetRequiredSection("Polygon").GetRequiredSection("ChartUrl").Get<string>();
            _optionUrl = configuration.GetRequiredSection("Services").GetRequiredSection("Polygon").GetRequiredSection("OptionUrl").Get<string>();
        }

        #region IFinanceClient Methods
        public async Task<List<Candle>> GetStocks(GetStocksRequest request)
        {
            var maxNbOfCandles = 50000;

            string from = request.StartDate.ToDateString();
            string to = request.EndDate.ToDateString();

            string interval = request.Interval.GetInterval();
            int amplitude = request.Interval.GetAmplitude();

            var candles = new List<Candle>();
            foreach(var ticker in request.Tickers)
            {
                string url = CreateRequestUrl(from, to, interval, amplitude, ticker, maxNbOfCandles);
                var response = await CallClient<PolygonResponse<PolygonCandle>>(url).ConfigureAwait(false);
                if (response.results.IsNullOrEmpty())
                    throw new BadStatusException($"Polygon failed on request {url} : {response.message}");

                candles.AddRange(MapToCandles(ticker, response));
            }

            return candles;
        }

        public async Task<List<Candle>> GetOption(GetOptionRequest request)
        {
            var maxNbOfCandles = 1000;

            string from = request.StartDate.ToDateString();
            string to = request.EndDate.ToDateString();

            string interval = request.Interval.GetInterval();
            int amplitude = request.Interval.GetAmplitude();

            var candles = new List<Candle>();
            foreach (var ticker in request.Tickers)
            {
                var option = CreateOptionReference(ticker, request.Strike, request.Maturity, request.OptionType);
                var url = CreateRequestUrl(from, to, interval, amplitude, option, maxNbOfCandles);
                var response = await CallClient<PolygonResponse<PolygonCandle>>(url).ConfigureAwait(false);
                if (response.results.IsNullOrEmpty())
                    throw new BadStatusException($"Polygon failed on request {url} : {response.message}");

                candles.AddRange(MapToCandles(option, response));
            }

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
        #endregion IFinanceClient Methods



        #region Private Methods
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
        #endregion Private Class
    }
}
