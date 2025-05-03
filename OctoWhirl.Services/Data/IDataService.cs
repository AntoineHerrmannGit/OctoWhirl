using OctoWhirl.Core.Models.Common;
using OctoWhirl.Core.Models.Enums;
using OctoWhirl.Core.Models.Technicals;

namespace OctoWhirl.Services.Data
{
    public interface IDataService : IService
    {
        Task<List<Candle>> GetStocks(string reference, DateTime startDate, DateTime endDate, ClientSource source, ResolutionInterval interval);
    }
}
