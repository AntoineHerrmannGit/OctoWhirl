using Core.Http.HttpRequests;
using Core.Technicals.Extensions;
using DataProviders.GenericProvider.DataProvider;
using DataProviders.YahooFinanceProviders.Configuration;
using DataProviders.YahooFinanceProviders.Core;
using DataProviders.YahooFinanceProviders.Models;
using DataProviders.YahooFincnaceProviders.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Models.Models.Objects.Spots;
using Models.Models.Requests;
using Models.Models.Requests.Spots;
using Models.TechnicalModels.Exceptions;

namespace DataProviders.YahooFinanceProviders.Providers
{
    internal sealed class YahooFinanceSpotProvider : IDataProvider<YahooFinanceSpot>
    {
        private readonly ILogger<YahooFinanceSpotProvider> _logger;
        private readonly YahooFinanceConfiguration _configuration;
        private readonly IYahooClient _client;

        #region Ctor
        public YahooFinanceSpotProvider(ILogger<YahooFinanceSpotProvider> logger, IOptions<YahooFinanceConfiguration> configuration, IYahooClient client)
        {
            _logger = logger;
            _configuration = configuration.Value;
            _client = client;
        }
        #endregion Ctor


        #region IDataProvider Methods
        public async Task<IEnumerable<YahooFinanceSpot>> Get(IMarketDataRequest<YahooFinanceSpot> request, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Getting Spots from YahooFinance...");

            var yahooRequest = request as YahooFinanceSpotRequest;
            if (yahooRequest is null)
                throw new WrongArgumentException(nameof(request));

            var startDate = new DateTimeOffset(yahooRequest.StartDate).ToUnixTimeSeconds();
            var endDate = new DateTimeOffset(yahooRequest.EndDate).ToUnixTimeSeconds();
            var interval = yahooRequest.Resolution.ToYahooString();

            var tasks = yahooRequest.Instruments.Select(instrument => 
                            GetSingleStock(instrument, startDate, endDate, interval, yahooRequest.AdjustClose, cancellationToken)
                        );

            var results = await Task.WhenAll(tasks).ConfigureAwait(false);
            return results.Flatten();
        }
        #endregion IDataProvider Methods

        #region Private Methods
        private Task<IEnumerable<YahooFinanceSpot>> GetSingleStock(string instrument, long startDate, long endDate, string interval, bool adjustSpots, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation($"Retreiving Spots for {instrument}...");
                var url = $"{_configuration.ChartUrl}/{instrument}?period1={startDate}&period2={endDate}&interval={interval}";

                var request = CreateRequest(instrument, startDate, endDate, interval);
                return _client.CallApi<YahooChartResponse, YahooFinanceSpot>(request, result => Convert(result, instrument, adjustSpots), cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur: {ex.Message}");
                return Task.FromResult(new List<YahooFinanceSpot>().AsEnumerable());
            }
        }

        private HttpRequest CreateRequest(string instrument, long startDate, long endDate, string interval)
            => new HttpRequest
            {
                Url = _configuration.ChartUrl,
                Method = HttpRequestMethod.Get,
            }
            .AddRoute(instrument)
            .AddParameter("period1", startDate)
            .AddParameter("period2", endDate)
            .AddParameter("interval", interval);

        private IEnumerable<YahooFinanceSpot> Convert(YahooChartResponse response, string instrument, bool adjustSpots)
        {
            var timestamps = response?.Chart?.Result?.FirstOrDefault()?.Timestamp;
            var quotes = response?.Chart?.Result?.FirstOrDefault()?.Indicators?.Quote?.FirstOrDefault();
            var adjustedQuotes = response?.Chart?.Result?.FirstOrDefault()?.Indicators?.AdjClose?.FirstOrDefault()?.AdjClose;

            if (timestamps == null || quotes == null)
                yield return new YahooFinanceSpot
                {
                    Instrument = instrument
                };

            var totalSpots = timestamps.Count;
            for (int i = 0; i < timestamps.Count; i++)
                yield return new YahooFinanceSpot
                {
                    Timestamp = DateTimeOffset.FromUnixTimeSeconds(timestamps[i]).DateTime,
                    Instrument = instrument,
                    Value = quotes?.Close?[i] ?? -1,
                    Open = quotes?.Open?[i] ?? -1,
                    High = quotes?.High?[i] ?? -1,
                    Low = quotes?.Low?[i] ?? -1,
                    Close = adjustSpots ? (quotes?.Close?[i] ?? -1) : (adjustedQuotes?[i] ?? -1),
                    Volume = (int)(quotes?.Volume?[i] ?? -1)
                };
        }
        #endregion Private Methods
    }
}
