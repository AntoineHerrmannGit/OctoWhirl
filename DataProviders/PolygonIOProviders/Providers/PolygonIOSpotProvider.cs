using Core.Http.HttpRequests;
using Core.Technicals.Extensions;
using DataProviders.GenericProvider.DataProvider;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Models.Models.Objects.Spots;
using Models.Models.Requests;
using Models.Models.Requests.Spots;
using Models.TechnicalModels.Exceptions;
using PolygonIOProviders.Configuration;
using PolygonIOProviders.Helpers;
using PolygonIOProviders.Interfaces;
using PolygonIOProviders.Models;

namespace PolygonIOProviders.Providers
{
    internal class PolygonIOSpotProvider : IDataProvider<PolygonIOSpot>
    {
        private readonly ILogger<PolygonIOSpotProvider> _logger;
        private readonly PolygonIOConfiguration _configuration;
        private readonly IPolygonIOClient _client;

        private const int _maxNbOfCandles = 50000;

        #region Ctor
        public PolygonIOSpotProvider(ILogger<PolygonIOSpotProvider> logger, IOptions<PolygonIOConfiguration> configuration, IPolygonIOClient client)
        {
            _logger = logger;
            _configuration = configuration.Value;
            _client = client;
        }
        #endregion Ctor

        #region Public Methods
        public async Task<IEnumerable<PolygonIOSpot>> Get(IMarketDataRequest<PolygonIOSpot> request, CancellationToken cancellationToken = default)
        {
            var polygonIORequest = request as PolygonIOSpotRequest;
            if (polygonIORequest == null)
                throw new WrongArgumentException(request.GetType().Name);

            _logger.LogInformation("Retreiving spots from PolygonIO...");
            var tasks = polygonIORequest.Instruments.Select(async instrument =>
            {
                var request = CreateRequest(polygonIORequest, instrument);
                var response = await _client.CallApi<PolygonIOResponse<PolygonIOOHLC>>(request, cancellationToken).ConfigureAwait(false);

                if (response is null)
                    return Enumerable.Empty<PolygonIOSpot>();

                var result = Convert(response);
                while (response.next_url is not null)
                {
                    request = CreateNextRequest(response.next_url);
                    response = await _client.CallApi<PolygonIOResponse<PolygonIOOHLC>>(request, cancellationToken).ConfigureAwait(false);

                    if (response is null)
                        continue;
                    result = result.Concat(Convert(response));
                }
                return result;
            });

            var results = await Task.WhenAll(tasks).ConfigureAwait(false);
            return results.Flatten();
        }
        #endregion Public Methods

        #region Private Methods
        private HttpRequest CreateRequest(PolygonIOSpotRequest request, string instrument)
            => new HttpRequest()
            {
                Url = _configuration.ChartUrl,
                Method = HttpRequestMethod.Get
            }
            .AddRoutes(new string[] { instrument, "range", request.Resolution.GetAmplitude().ToLowerString(), request.Resolution.GetInterval(), request.StartDate.ToDateString(), request.EndDate.ToDateString() })
            .AddParameter("adjusted", request.AdjustSpots.ToString().ToLower())
            .AddParameter("sort", "asc")
            .AddParameter("limit", _maxNbOfCandles);

        private HttpRequest CreateNextRequest(string url)
            => new HttpRequest()
            {
                Url = url,
                Method = HttpRequestMethod.Get
            }
            .AddParameter("limit", _maxNbOfCandles);

        private IEnumerable<PolygonIOSpot> Convert(PolygonIOResponse<PolygonIOOHLC> response)
            => response.results.Select(result => new PolygonIOSpot
            {
                Instrument = response.ticker,
                Timestamp = DateTime.FromFileTime(result.t),
                Open = result.o,
                High = result.h,
                Low = result.l,
                Close = result.c,
                Volume = result.v,
                OTC = result.otc,
                Value = result.c,
                WeightedVolume = result.vw,
            });
        #endregion Private Methods
    }
}
