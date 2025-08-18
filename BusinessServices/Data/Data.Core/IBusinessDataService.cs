using OctoWhirl.Core.Models.Models.Requests;
using OctoWhirl.Core.Models.Models.Technicals;

namespace OctoWhirl.BusinessServices.Data.Core
{
    public interface IBusinessDataService
    {
        Task<List<CandleSerie>> GetCandles(GetCandlesRequest request);
    }
}
