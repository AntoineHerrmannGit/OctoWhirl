using Core.Technicals.Extensions;
using DataProviders.GenericProvider.DataProvider;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Models.Models.Enums;
using Models.Models.Objects.CorporateActions;
using Models.Models.Requests;
using Models.Models.Requests.CorporateActions;
using PolygonIOProviders.Configuration;
using PolygonIOProviders.Interfaces;
using PolygonIOProviders.Models;

namespace PolygonIOProviders.Providers
{
    internal class PolygonIODividendProvider : PolygonIOCorporateActionProvider<PolygonIODividend>, IDataProvider<PolygonIODividend>
    {
        private readonly ILogger<PolygonIODividendProvider> _logger;
        protected override CorporateActionType CorporateActionType => CorporateActionType.Dividend;

        #region Ctor
        public PolygonIODividendProvider(ILogger<PolygonIODividendProvider> logger, IOptions<PolygonIOConfiguration> configuration, IPolygonIOClient client)
            : base(logger, configuration, client)
        {
            _logger = logger;
        }
        #endregion Ctor

        #region IDataProvider Methods
        public Task<IEnumerable<PolygonIODividend>> Get(IMarketDataRequest<PolygonIODividend> request, CancellationToken cancellationToken = default)
        {
            var dividendRequest = request as PolygonIODividendRequest;
            if (dividendRequest is null)
            {
                _logger.LogError("Wrong request type");
                throw new Exception();
            }
            return base.Get(dividendRequest, cancellationToken);
        }
        #endregion IDataProvider Methods

        #region Protected Override Methods
        protected override IEnumerable<PolygonIODividend> Convert(PolygonIOCorporateActionResponse response)
            => response.results.Select(result => new PolygonIODividend
            {
                Instrument = result.ticker,
                ExecutionDate = result.ex_dividend_date.ToDate(),
                Value = result.cash_amount,
                Timestamp = DateTime.Now,
                Currency = result.currency,
            });
        #endregion Protected Override Methods
    }
}
