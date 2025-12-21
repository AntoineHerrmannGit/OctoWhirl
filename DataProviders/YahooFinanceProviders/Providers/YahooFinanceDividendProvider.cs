using DataProviders.GenericProvider.DataProvider;
using DataProviders.YahooFinanceProviders.Configuration;
using DataProviders.YahooFinanceProviders.Models;
using DataProviders.YahooFincnaceProviders.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Models.Models.Objects.CorporateActions;
using Models.Models.Requests;
using Models.Models.Requests.Dividends;
using Models.TechnicalModels.Exceptions;

namespace DataProviders.YahooFincnaceProviders.Providers
{
    internal sealed class YahooFinanceDividendProvider : YahooFinanceCorporateActionProvider<YahooFinanceDividend>, IDataProvider<YahooFinanceDividend>
    {
        private readonly ILogger<YahooFinanceDividendProvider> _logger;
        private readonly IYahooClient _client;
        private readonly YahooFinanceConfiguration _configuration;

        public YahooFinanceDividendProvider(ILogger<YahooFinanceDividendProvider> logger, IOptions<YahooFinanceConfiguration> configuration, IYahooClient yahooClient)
            : base(logger, configuration, yahooClient)
        {
            _logger = logger;
            _client = yahooClient;
            _configuration = configuration.Value;
        }

        #region IDataProvider Methods
        public Task<IEnumerable<YahooFinanceDividend>> Get(IMarketDataRequest<YahooFinanceDividend> request, CancellationToken cancellationToken = default)
        {
            var dividendRequest = request as YahooFinanceDividendRequest;
            if (dividendRequest == null)
                throw new WrongArgumentException(nameof(request));

            return GetFromYahooFinance(dividendRequest, cancellationToken);
        }
        #endregion IDataProvider Methods

        #region Protected Override YahooFinanceCorporateActionProvider Methods
        protected override IEnumerable<YahooFinanceDividend> Convert(YahooChartResponse response, string instrument)
            => response?.Chart?.Result?.FirstOrDefault()?
                        .Events?.Dividends?.Values.Select(div => new YahooFinanceDividend()
                        {
                            Instrument = instrument,
                            Timestamp = DateTimeOffset.FromUnixTimeSeconds(div.Date).DateTime,
                            Value = div.Amount,
                            ExecutionDate = DateTimeOffset.FromUnixTimeSeconds(div.Date).DateTime,
                        }) ?? new List<YahooFinanceDividend>();
        #endregion Protected Override YahooFinanceCorporateActionProvider Methods
    }
}
