using OctoWhirl.Core.Models.Models.Common.Interfaces;
using OctoWhirl.Core.Models.Models.Requests;

namespace OctoWhirl.TechnicalServices.DataService.Providers.Interfaces
{
    public interface IDataProvider<TMarketData> where TMarketData : IMarketData
    {
        Task<IEnumerable<TMarketData>> Get(IMarketDataRequest request, CancellationToken cancellation);
    }
}
