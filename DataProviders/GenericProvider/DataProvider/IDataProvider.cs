using Models.Models.Interfaces;

namespace DataProviders.GenericProvider.DataProvider
{
    public interface IDataProvider<TMarketData> : IProvider<TMarketData>
        where TMarketData : IMarketData
    {
    }
}
