using OctoWhirl.Core.Models.Common;
using OctoWhirl.Core.Models.Technicals;
using OctoWhirl.Services.Models.Requests;

namespace OctoWhirl.Services.Data.Clients
{
    public interface IFinanceClient : IDataService
    {
        Task<List<Candle>> GetOption(string reference, double strike, DateTime maturity, OptionType optionType, DateTime startDate, DateTime endDate, ResolutionInterval resolution = ResolutionInterval.Day);
        Task<List<Option>> GetListedOptions(string symbol, DateTime date);
    }
}
