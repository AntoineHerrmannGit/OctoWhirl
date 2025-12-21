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
    internal class PolygonIOSplitProvider : PolygonIOCorporateActionProvider<PolygonIOSplit>, IDataProvider<PolygonIOSplit>
    {
        private readonly ILogger<PolygonIOSplitProvider> _logger;
        protected override CorporateActionType CorporateActionType => CorporateActionType.Split;

        #region Ctor
        public PolygonIOSplitProvider(ILogger<PolygonIOSplitProvider> logger, IOptions<PolygonIOConfiguration> configuration, IPolygonIOClient client)
            : base(logger, configuration, client)
        {
            _logger = logger;
        }
        #endregion Ctor

        #region IDataProvider Methods
        public Task<IEnumerable<PolygonIOSplit>> Get(IMarketDataRequest<PolygonIOSplit> request, CancellationToken cancellationToken)
        {
            var splitRequest = request as PolygonIOSplitRequest;
            if (splitRequest is null)
            {
                _logger.LogError("Wrong request type");
                throw new Exception();
            }
            return base.Get(splitRequest, cancellationToken);
        }
        #endregion IDataProvider Methods

        #region Protected Override Methods
        protected override IEnumerable<PolygonIOSplit> Convert(PolygonIOCorporateActionResponse response)
            => response.results.Select(result => new PolygonIOSplit
            {
                Instrument = result.ticker,
                Timestamp = DateTime.TryParse(result.ex_dividend_date, out var timestamp) ? timestamp : null,
                SplitRatio = result.split_from / result.split_to,
                NewInstrument = result.new_ticker,
                OldInstrument = result.old_ticker,
                TargetInstrument = result.target_ticker,
            });
        #endregion Protected Override Methods
    }
}
