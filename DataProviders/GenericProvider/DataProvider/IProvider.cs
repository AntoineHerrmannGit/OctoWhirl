using Models.Models.Interfaces;
using Models.Models.Requests;

namespace DataProviders.GenericProvider.DataProvider
{
    public interface IProvider<TMarketData>
        where TMarketData : IMarketData
    {
        Task<IEnumerable<TMarketData>> Get(IMarketDataRequest<TMarketData> request, CancellationToken cancellationToken);
    }
}
