using DataProviders.GenericProvider.DataProvider;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Models.Models.Interfaces;
using Models.Models.Requests;

namespace DataProviders.GenericProvider.ProviderSelectors
{
    public class ProviderSelector : IProviderSelector
    {
        private readonly ILogger<ProviderSelector> _logger;
        private readonly IServiceProvider _serviceProvider;

        public ProviderSelector(ILogger<ProviderSelector> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }


        public IDataProvider<TMarketData> SelectProvider<TMarketData>(IMarketDataRequest<TMarketData> request)
            where TMarketData : IMarketData
        {
            _logger.LogInformation($"Selecting DataProvider for source : {request.Source}, market data : {request.MarketDataType}");
            //if (request.SkipCache)
            //    return _serviceProvider.GetRequiredService<IDataProvider<TMarketData>>();
            return _serviceProvider.GetRequiredService<IDataProvider<TMarketData>>();
        }
    }
}
