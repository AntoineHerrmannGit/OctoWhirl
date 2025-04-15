using OctoWhirl.Core.Models.Common;
using OctoWhirl.Core.Models.Technicals;

namespace OctoWhirl.Services.Data.Clients
{
    public interface IFinanceClient
    {
        Task<List<Candle>> GetStock(string reference, DateTime startDate, DateTime endDate, ResolutionInterval resolution = ResolutionInterval.Day);
        Task<List<Candle>> GetOption(string reference, double strike, DateTime maturity, OptionType optionType, DateTime startDate, DateTime endDate, ResolutionInterval resolution = ResolutionInterval.Day);
        Task<List<Option>> GetListedOptions(string symbol, DateTime date);
    }
}
