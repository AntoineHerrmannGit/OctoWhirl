using System.Net.Http;
using Microsoft.Extensions.Configuration;
using OctoWhirl.Core.Extensions;
using OctoWhirl.Core.Models.Common;
using OctoWhirl.Core.Models.Technicals;

namespace OctoWhirl.Services.Data.Clients.YahooFinanceClient
{
    public class YahooFinanceClient : BaseClient, IFinanceClient
    {
        public YahooFinanceClient(HttpClient httpClient, IConfiguration configuration) : base(httpClient)
        {
            InitializeClient(configuration);
        }

        #region BaseClient Methods
        protected override void InitializeClient(IConfiguration configuration)
        {
            var baseUrl = configuration.GetRequiredSection("YahooFinance").GetRequiredSection("BaseUrl").Get<string>();
            if (baseUrl.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(baseUrl), "Base URL cannot be null or empty.");

            _httpClient.BaseAddress = new Uri(baseUrl);
        }
        #endregion BaseClient Methods

        #region YahooFinanceClient Methods
        public async Task<List<Candle>> GetStock(string reference, DateTime startDate, DateTime endDate, ResolutionInterval interval = ResolutionInterval.Day)
        {
            var period1 = new DateTimeOffset(startDate).ToUnixTimeSeconds();
            var period2 = new DateTimeOffset(endDate).ToUnixTimeSeconds();

            string resolution = YahooFinanceIntervalResolutionParser.ToString(interval);

            var url = $"v8/finance/chart/{reference}?period1={period1}&period2={period2}&interval={resolution}";
            var candles = await CallClient<List<Candle>>(url).ConfigureAwait(false);

            candles.ForEach(candle => candle.Reference = reference);

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
    }
}
