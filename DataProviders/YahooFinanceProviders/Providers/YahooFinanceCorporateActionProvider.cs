using Core.Http.HttpRequests;
using DataProviders.YahooFinanceProviders.Configuration;
using DataProviders.YahooFinanceProviders.Core;
using DataProviders.YahooFinanceProviders.Models;
using DataProviders.YahooFincnaceProviders.Core;
using DataProviders.YahooFincnaceProviders.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Models.Models.Interfaces;
using Models.Models.Requests.Genericity;
using Core.Technicals.Extensions;

namespace DataProviders.YahooFincnaceProviders.Providers
{
    internal abstract class YahooFinanceCorporateActionProvider<TCorporateAction>
        where TCorporateAction : class, ICorporateAction
    {
        private readonly ILogger<YahooFinanceCorporateActionProvider<TCorporateAction>> _logger;
        private readonly IYahooClient _client;
        private readonly YahooFinanceConfiguration _configuration;

        public YahooFinanceCorporateActionProvider(ILogger<YahooFinanceCorporateActionProvider<TCorporateAction>> logger, IOptions<YahooFinanceConfiguration> configuration, IYahooClient yahooClient)
        {
            _logger = logger;
            _client = yahooClient;
            _configuration = configuration.Value;
        }

        #region Protected Methods
        protected async Task<IEnumerable<TCorporateAction>> GetFromYahooFinance(YahooFinanceCorporateActionsRequest<TCorporateAction> request, CancellationToken cancellationToken = default)
        {
            var startDate = new DateTimeOffset(request.StartDate).ToUnixTimeSeconds();
            var endDate = new DateTimeOffset(request.EndDate).ToUnixTimeSeconds();

            var interval = request.Resolution.ToYahooString();

            var corporateActionType = request.CorporateActionType.ToYahooString();

            var tasks = request.Instruments.Select(instrument =>
                GetSingleFromYahooFinance(instrument, startDate, endDate, interval, corporateActionType, cancellationToken)
            );

            var results = await Task.WhenAll(tasks).ConfigureAwait(false);
            return results.Flatten();
        }
        #endregion Protected Methods

        #region Abstract Protected Methods
        protected abstract IEnumerable<TCorporateAction> Convert(YahooChartResponse response, string instrument);
        #endregion Abstract Protected Methods

        #region Private Methods
        private HttpRequest CreateRequest(string instrument, long startDate, long endDate, string interval, string corporateActionType)
            => new HttpRequest
            {
                Url = _configuration.CorporateActionUrl,
                Method = HttpRequestMethod.Get,
            }
            .AddRoute(instrument)
            .AddParameter("interval", interval)
            .AddParameter("period1", startDate.ToString())
            .AddParameter("period2", endDate.ToString())
            .AddParameter("events", corporateActionType);

        private Task<IEnumerable<TCorporateAction>> GetSingleFromYahooFinance(string instrument, long startDate, long endDate, string interval, string corporateActionType, CancellationToken cancellationToken = default)
        {
            var httpRequest = CreateRequest(instrument, startDate, endDate, interval, corporateActionType);
            return _client.CallApi<YahooChartResponse, TCorporateAction>(httpRequest, div => Convert(div, instrument), cancellationToken);
        }
        #endregion Private Methods
    }
}
