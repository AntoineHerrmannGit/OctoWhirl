using Core.Http.HttpClients;
using Core.Http.HttpRequests;
using DataProviders.YahooFinanceProviders.Configuration;
using DataProviders.YahooFincnaceProviders.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Models.Models.Interfaces;

namespace DataProviders.YahooFinanceProviders.Client
{
    internal sealed class YahooClient : BaseHttpClient, IYahooClient
    {
        private readonly ILogger<YahooClient> _logger;

        public YahooClient(ILogger<YahooClient> logger, IOptions<YahooFinanceConfiguration> configuration)
            : base(url: null, timeout: configuration.Value.Timeout)
        {
            _logger = logger;
        }


        #region Public Methods
        public async Task<IEnumerable<TOctoWhirlModel>> CallApi<TYahooModel, TOctoWhirlModel>(HttpRequest request, Func<TYahooModel, IEnumerable<TOctoWhirlModel>> mapper, CancellationToken cancellationToken = default)
            where TYahooModel : class
            where TOctoWhirlModel : IMarketData
        {
            // This header is required otherwise YahooFinance throws a 429 errror
            // It simulates a call from a browser
            request.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/113.0.0.0 Safari/537.36");
            
            _logger.LogInformation($"Calling YahooFinance Api...");
            var yahooResponse = await Execute<TYahooModel>(request, cancellationToken).ConfigureAwait(false);
            return mapper(yahooResponse);
        }
        #endregion Public Methods
    }
}
