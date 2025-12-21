using DataProviders.GenericProvider.DataProvider;
using Models.Models.Interfaces;
using Models.Models.Requests;

namespace DataProviders.GenericProvider.ProviderSelectors
{
    public interface IProviderSelector
    {
        IDataProvider<TMarketData> SelectProvider<TMarketData>(IMarketDataRequest<TMarketData> request)
            where TMarketData : IMarketData;
    }
}
