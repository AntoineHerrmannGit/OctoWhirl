using Core.Http.HttpRequests;
using Core.Technicals.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Models.Models.Enums;
using Models.Models.Interfaces;
using Models.Models.Requests.Genericity;
using PolygonIOProviders.Configuration;
using PolygonIOProviders.Helpers;
using PolygonIOProviders.Interfaces;
using PolygonIOProviders.Models;

namespace PolygonIOProviders.Providers
{
    internal abstract class PolygonIOCorporateActionProvider<TMarketData>
        where TMarketData : IMarketData, new()
    {
        private readonly ILogger<PolygonIOCorporateActionProvider<TMarketData>> _logger;
        private readonly IPolygonIOClient _client;

        protected readonly PolygonIOConfiguration _configuration;
        protected abstract CorporateActionType CorporateActionType { get; }

        protected const int _pointsLimit = 1000;

        #region Ctor
        protected PolygonIOCorporateActionProvider(ILogger<PolygonIOCorporateActionProvider<TMarketData>> logger, IOptions<PolygonIOConfiguration> configuration, IPolygonIOClient client)
        {
            _logger = logger;
            _configuration = configuration.Value;
            _client = client;
        }
        #endregion Ctor

        #region Protected Methods
        protected async Task<IEnumerable<TMarketData>> Get(PolygonIOCorporateActionRequest<TMarketData> request, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Retreiving Corporate Actions from PolygonIO...");

            var startDate = request.StartDate.ToDateString();
            var endDate = request.EndDate.ToDateString();

            var tasks = request.Instruments.Select(ticker => GetSingle(ticker, startDate, endDate, cancellationToken));
            var results = await Task.WhenAll(tasks).ConfigureAwait(false);

            return results.Flatten();
        }
        #endregion Protected Methods

        #region Protected Abstract Methods
        protected abstract IEnumerable<TMarketData> Convert(PolygonIOCorporateActionResponse response);
        #endregion Protected Abstract Methods

        #region Private Methods
        private async Task<IEnumerable<TMarketData>> GetSingle(string instrument, string startDate, string endDate, CancellationToken cancellationToken = default)
        {
            var request = CreateRequest(instrument, startDate, endDate);
            var response = await _client.CallApi<PolygonIOCorporateActionResponse>(request, cancellationToken).ConfigureAwait(false);

            var result = Convert(response);
            while (response.next_url is not null)
            {
                request = CreateNextRequest(response.next_url);
                response = await _client.CallApi<PolygonIOCorporateActionResponse>(request, cancellationToken).ConfigureAwait(false);
                result = result.Concat(Convert(response));
            }
            return result;
        }

        private HttpRequest CreateRequest(string instrument, string startDate, string endDate)
            => new HttpRequest
            {
                Url = _configuration.CorporateActionsUrl,
                Method = HttpRequestMethod.Get
            }
            .AddRoute(PolygonIOCorporateActionTypeParser.Parse(CorporateActionType))
            .AddParameter("ticker", instrument)
            .AddParameter("execution_date.gte", startDate)
            .AddParameter("execution_date.lte", endDate)
            .AddParameter("limit", _pointsLimit);

        private HttpRequest CreateNextRequest(string url)
            => new HttpRequest
            {
                Url = url,
                Method = HttpRequestMethod.Get
            }
            .AddParameter("limit", _pointsLimit);
        #endregion Private Methods
    }
}
