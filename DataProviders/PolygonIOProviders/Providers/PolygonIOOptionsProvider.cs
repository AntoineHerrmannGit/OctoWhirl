using Core.Http.HttpRequests;
using Core.Technicals.Extensions;
using DataProviders.GenericProvider.DataProvider;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Models.Models.Enums;
using Models.Models.Objects.Options;
using Models.Models.Requests;
using Models.Models.Requests.Options;
using Models.TechnicalModels.Exceptions;
using PolygonIOProviders.Configuration;
using PolygonIOProviders.Interfaces;
using PolygonIOProviders.Models;

namespace PolygonIOProviders.Providers
{
    internal class PolygonIOOptionsProvider : IDataProvider<PolygonIOOption>
    {
        private readonly ILogger<PolygonIOOptionsProvider> _logger;
        private readonly PolygonIOConfiguration _configuration;
        private readonly IPolygonIOClient _client;

        private const int _listedOptionLimit = 1000;

        public PolygonIOOptionsProvider(ILogger<PolygonIOOptionsProvider> logger, IOptions<PolygonIOConfiguration> configuration, IPolygonIOClient client)
        {
            _logger = logger;
            _configuration = configuration.Value;
            _client = client;
        }

        #region IDataProvider Methods
        public async Task<IEnumerable<PolygonIOOption>> Get(IMarketDataRequest<PolygonIOOption> request, CancellationToken cancellationToken)
        {
            var polygonRequest = request as PolygonIOOptionsRequest;
            if (polygonRequest is null)
                throw new WrongArgumentException(nameof(request));

            _logger.LogInformation("Retreiving options from PolygonIO...");
            var tasks = polygonRequest.StartDate.ForEachDateUntil(polygonRequest.EndDate)
                                                .SelectMany(date => polygonRequest.Instruments.Select(instrument
                                                    => GetSingleOption(instrument, date, polygonRequest.IncludeExpired, cancellationToken)
                                                ));

            var result = await Task.WhenAll(tasks).ConfigureAwait(false);
            return result.Flatten();
        }
        #endregion IDataProvider Methods

        #region Private Methods
        private async Task<IEnumerable<PolygonIOOption>> GetSingleOption(string instrument, DateTime date, bool includeExpired, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation($"Retreiving options for {instrument} on {date}...");

            var request = CreateRequest(instrument, date, includeExpired);
            var response = await _client.CallApi<PolygonIOResponse<PolygonIOOptionResponse>>(request, cancellationToken).ConfigureAwait(false);

            var result = Convert(response);
            while (response.next_url is not null)
            {
                request = CreateNextRequest(response.next_url);
                response = await _client.CallApi<PolygonIOResponse<PolygonIOOptionResponse>>(request, cancellationToken).ConfigureAwait(false);
                if (response.results.IsNullOrEmpty())
                    break;

                result = result.Concat(Convert(response));
            }
            return result;
        }

        private HttpRequest CreateRequest(string instrument, DateTime date, bool includeExpired)
            => new HttpRequest()
            {
                Url = _configuration.OptionsUrl,
                Method = HttpRequestMethod.Get,
            }
            .AddParameter("underlying_ticker", instrument)
            .AddParameter("as_of", date.ToDateString())
            .AddParameter("expired", includeExpired.ToString().ToLower())
            .AddParameter("order", "asc")
            .AddParameter("limit", _listedOptionLimit)
            .AddParameter("sort", "ticker");

        private HttpRequest CreateNextRequest(string url)
            => new HttpRequest
            {
                Url = url,
                Method = HttpRequestMethod.Get,
            }
            .AddParameter("limit", _listedOptionLimit);

        private IEnumerable<PolygonIOOption> Convert(PolygonIOResponse<PolygonIOOptionResponse> response)
            => response.results.Select(result => new PolygonIOOption
            {
                AbsoluteStrike = result.strike_price,
                Expiration = DateTime.Parse(result.expiration_date),
                Exchange = result.primary_exchange,
                OptionType = result.contract_type == "call" ? OptionTypeEnum.Call : OptionTypeEnum.Put,
                Instrument = response.ticker,
                Ticker = result.ticker,
                Underlying = result.underlying_ticker,
            });
        #endregion Private Methods
    }
}
