using DataProviders.GenericProvider.DataProvider;
using DataProviders.YahooFinanceProviders.Configuration;
using DataProviders.YahooFinanceProviders.Models;
using DataProviders.YahooFincnaceProviders.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Models.Models.Objects.CorporateActions;
using Models.Models.Requests;
using Models.Models.Requests.CorporateActions;
using Models.TechnicalModels.Exceptions;

namespace DataProviders.YahooFincnaceProviders.Providers
{
    internal sealed class YahooFinanceSplitProvider : YahooFinanceCorporateActionProvider<YahooFinanceSplit>, IDataProvider<YahooFinanceSplit>
    {
        private readonly ILogger<YahooFinanceSplitProvider> _logger;
        private readonly IYahooClient _client;
        private readonly YahooFinanceConfiguration _configuration;

        public YahooFinanceSplitProvider(ILogger<YahooFinanceSplitProvider> logger, IOptions<YahooFinanceConfiguration> configuration, IYahooClient yahooClient)
            : base(logger, configuration, yahooClient)
        {
            _logger = logger;
            _client = yahooClient;
            _configuration = configuration.Value;
        }

        #region IDataProvider Methods
        public Task<IEnumerable<YahooFinanceSplit>> Get(IMarketDataRequest<YahooFinanceSplit> request, CancellationToken cancellationToken)
        {
            var splitRequest = request as YahooFinanceSplitRequest;
            if (splitRequest == null)
                throw new WrongArgumentException(nameof(request));

            return GetFromYahooFinance(splitRequest, cancellationToken);
        }
        #endregion IDataProvider Methods

        #region Protected Override YahooFinanceCorporateActionProvider Methods
        protected override IEnumerable<YahooFinanceSplit> Convert(YahooChartResponse response, string instrument)
            => response?.Chart?.Result?.FirstOrDefault()?
                        .Events?.Splits?.Values.Select(split => new YahooFinanceSplit
                        {
                            Instrument = instrument,
                            Timestamp = DateTimeOffset.FromUnixTimeSeconds(split.Date).DateTime,
                            SplitRatio = split.Numerator / split.Denominator,
                        }) ?? new List<YahooFinanceSplit>();
        #endregion Protected Override YahooFinanceCorporateActionProvider Methods
    }
}
