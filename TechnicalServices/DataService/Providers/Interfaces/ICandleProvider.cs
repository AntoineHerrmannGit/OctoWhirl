using OctoWhirl.Core.Models.Models.Common;
using OctoWhirl.Core.Models.Models.Requests;

namespace OctoWhirl.TechnicalServices.DataService.Providers.Interfaces
{
    public interface ICandleProvider : IDataProvider<Candle>
    {
        IEnumerable<Candle> GetCandels(GetCandlesRequest request, CancellationToken cancellation);
    }
}
