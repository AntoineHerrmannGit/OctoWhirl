using System.Configuration;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using OctoWhirl.Core.Extensions;
using OctoWhirl.Core.Models.Common;
using OctoWhirl.Core.Models.Technicals;

namespace OctoWhirl.Services.Data.Clients.FinnHubClient
{
    public class FinnHubClient : BaseClient, IFinanceClient
    {
        private readonly string _apiKey;
        private readonly string _acknowledgeSecret;    // For real time updates events

        public FinnHubClient(HttpClient httpClient, IConfiguration configuration) : base(httpClient)
        {
            _apiKey = configuration.GetRequiredSection("Services").GetRequiredSection("FinnHub").GetRequiredSection("ApiKey").Get<string>() 
                    ?? throw new ConfigurationErrorsException("Services:FinnHub:ApiKey");
            _acknowledgeSecret = configuration.GetRequiredSection("Services").GetRequiredSection("FinnHub").GetRequiredSection("RTApiCheckKey").Get<string>() 
                    ?? throw new ConfigurationErrorsException("Services:FinnHub:RTApiCheckKey");

            InitializeClient(configuration);
        }

        #region BaseClient Methods
        protected override void InitializeClient(IConfiguration configuration)
        {
            var baseUrl = configuration.GetRequiredSection("Services").GetRequiredSection("FinnHub").GetRequiredSection("BaseUrl").Get<string>();
            if (baseUrl.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(baseUrl), "Base URL cannot be null or empty.");

            _httpClient.BaseAddress = new Uri(baseUrl);
        }
        #endregion BaseClient Methods

        #region FinnHubClient Methods
        /// <summary>
        /// Retrieves stocks spots from FinnHub
        /// </summary>
        public async Task<List<Candle>> GetStock(string reference, DateTime startDate, DateTime endDate, ResolutionInterval interval = ResolutionInterval.Day)
        {
            long from = startDate.ToUnixTimestamp();
            long to = endDate.ToUnixTimestamp();

            string resolution = FinnHubResolutionIntervalParser.ToString(interval);

            string url = $"stock/candle?symbol={reference}&resolution={resolution}&from={from}&to={to}&token={_apiKey}";

            var response = await CallClient<CandleResponse>(url);

            if (response?.s != "ok" || response.t == null)
                return new List<Candle>();

            var candles = MapCandles(response, reference);

            return candles;
        }

        /// <summary>
        /// Retrieves options spots from FinnHub
        /// </summary>
        public async Task<List<Candle>> GetOption(string reference, double strike, DateTime maturity, OptionType optionType, DateTime startDate, DateTime endDate, ResolutionInterval interval = ResolutionInterval.Day)
        {
            long from = startDate.ToUnixTimestamp();
            long to = endDate.ToUnixTimestamp();

            string resolution = FinnHubResolutionIntervalParser.ToString(interval);

            var symbol = BuildOptionSymbol(reference, strike, maturity, optionType);

            string url = $"options/candle?symbol={symbol}&resolution={resolution}&from={from}&to={to}&token={_apiKey}";

            var response = await CallClient<CandleResponse>(url);

            if (response?.s != "ok" || response.t == null)
                return new List<Candle>();

            var candles = MapCandles(response, reference);

            return candles;
        }

        /// <summary>
        /// Retrieve the list of available options on market
        /// </summary>
        public async Task<List<Option>> GetListedOptions(string symbol, DateTime date)
        {
            string formattedDate = date.ToString("yyyy-MM-dd");
            string url = $"stock/option-chain?symbol={symbol}&date={formattedDate}&token={_apiKey}";

            var response = await CallClient<OptionChainResponse>(url);

            if (response?.optionChain == null)
                return new List<Option>();

            var options = MapOptions(response, symbol);

            return options;
        }
        #endregion FinnHubClient Methods

        #region Private Methods
        /// <summary>
        /// Follows OOC naming
        /// </summary>
        private string BuildOptionSymbol(string reference, double strike, DateTime maturity, OptionType optionType)
        {
            // Format maturity (MMDDYY)
            var endDate = $"{maturity.Month:D2}{maturity.Day:D2}{maturity.Year.ToString().Substring(2, 2)}"; // MMDDYY

            // Option type
            var type = optionType == OptionType.Call ? "C" : optionType == OptionType.Put ? "P" : throw new ArgumentException(nameof(optionType));

            // Strike format
            var formattedStrike = ((int)(strike * 100)).ToString();

            // Truncate underlying to 5 characters
            var truncatedReference = reference.Length > 5 ? reference.Substring(0, 5) : reference;

            // Construction du symbole de l'option
            var symbol = $"{truncatedReference}{endDate}{type}{formattedStrike}";
            return symbol;
        }

        private List<Candle> MapCandles(CandleResponse candleResponse, string symbol)
        {
            var candles = new List<Candle>();
            for (int i = 0; i < candleResponse.t.Count; i++)
            {
                candles.Add(new Candle
                {
                    Reference = symbol,
                    Timestamp = DateTimeOffset.FromUnixTimeSeconds(candleResponse.t[i]).DateTime,
                    Open = candleResponse.o[i],
                    High = candleResponse.h[i],
                    Low = candleResponse.l[i],
                    Close = candleResponse.c[i],
                    Volume = (int)candleResponse.v[i]
                });
            }

            return candles;
        }

        private List<Option> MapOptions(OptionChainResponse optionChainResponse, string symbol)
        {
            var options = new List<Option>();
            foreach (var expiration in optionChainResponse.optionChain)
            {
                DateTime maturity = DateTime.Parse(expiration.expirationDate);

                if (expiration.calls != null)
                {
                    foreach (var call in expiration.calls)
                    {
                        options.Add(new Option
                        {
                            OptionType = OptionType.Call,
                            Maturity = maturity,
                            Strike = call.strike,
                            Reference = call.symbol,
                            Underlying = symbol
                        });
                    }
                }

                if (expiration.puts != null)
                {
                    foreach (var put in expiration.puts)
                    {
                        options.Add(new Option
                        {
                            OptionType = OptionType.Put,
                            Maturity = maturity,
                            Strike = put.strike,
                            Reference = put.symbol,
                            Underlying = symbol
                        });
                    }
                }
            }

            return options;
        }
        #endregion Private Methods
    }
}