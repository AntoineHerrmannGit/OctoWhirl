using OctoWhirl.Core.Models.Requests;
using OctoWhirl.Models.Technicals;

namespace OctoWhirl.BusinessServices.Data.Core
{
    public interface IBusinessDataService
    {
        Task<List<CandleSerie>> GetCandles(GetCandlesRequest request);
    }
}
