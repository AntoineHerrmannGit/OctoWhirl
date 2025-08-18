using OctoWhirl.Core.Models.Models.Common;
using OctoWhirl.Core.Models.Models.Requests;

namespace OctoWhirl.TechnicalServices.DataService
{
    public interface IDataService
    {
        Task<List<Candle>> GetCandles(GetCandlesRequest request);
    }
}
