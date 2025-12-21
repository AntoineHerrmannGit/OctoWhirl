using DataProviders.GenericProvider.ProviderSelectors;
using Microsoft.Extensions.Logging;
using Models.Models.Interfaces;
using Models.Models.Requests;

namespace DataProviders.GenericProvider.DataGetters
{
    public class DataGetter : IDataGetter
    {
        private readonly ILogger<DataGetter> _logger;
        private readonly IProviderSelector _selector;

        public DataGetter(ILogger<DataGetter> logger, IProviderSelector selector)
        {
            _logger = logger;
            _selector = selector;
        }


        public Task<IEnumerable<TMarketData>> Get<TMarketData>(IMarketDataRequest<TMarketData> request, CancellationToken cancellationToken = default) 
            where TMarketData : IMarketData
            => _selector.SelectProvider(request).Get(request, cancellationToken);
    }
}
