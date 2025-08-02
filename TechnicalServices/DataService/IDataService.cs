using OctoWhirl.Core.Models.Common;
using OctoWhirl.Core.Models.Requests;

namespace OctoWhirl.TechnicalServices.DataService
{
    public interface IDataService
    {
        Task<List<Candle>> GetCandles(GetCandlesRequest request);
    }
}
