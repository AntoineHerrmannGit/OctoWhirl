using OctoWhirl.Core.Models.Common;
using OctoWhirl.Services.Models.Requests;

namespace OctoWhirl.TechnicalServices.DataService
{
    public interface IFinanceService : IDataService
    {
        Task<List<Candle>> GetOption(GetOptionRequest request);
        Task<List<Option>> GetListedOptions(GetListedOptionRequest request);
        Task<List<Split>> GetSplits(GetCorporateActionsRequest request);
        Task<List<Dividend>> GetDividends(GetCorporateActionsRequest request);
    }
}
