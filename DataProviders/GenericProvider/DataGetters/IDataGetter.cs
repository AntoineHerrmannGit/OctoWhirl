using Models.Models.Interfaces;
using Models.Models.Requests;

namespace DataProviders.GenericProvider.DataGetters
{
    public interface IDataGetter
    {
        Task<IEnumerable<TMarketData>> Get<TMarketData>(IMarketDataRequest<TMarketData> request, CancellationToken cancellationToken = default)
            where TMarketData : IMarketData;
    }
}
