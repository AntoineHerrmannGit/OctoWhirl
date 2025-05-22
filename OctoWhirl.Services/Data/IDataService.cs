using OctoWhirl.Core.Models.Common;
using OctoWhirl.Core.Models.Enums;
using OctoWhirl.Core.Models.Technicals;
using OctoWhirl.Services.Models.Requests;

namespace OctoWhirl.Services.Data
{
    public interface IDataService : IService
    {
        Task<List<Candle>> GetStocks(GetStocksRequest request);
    }
}
