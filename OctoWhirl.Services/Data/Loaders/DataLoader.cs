using OctoWhirl.Core.Models.Common;
using OctoWhirl.Services.Models.Requests;

namespace OctoWhirl.Services.Data.Loaders
{
    public class DataLoader : IDataService
    {
        private readonly DataLoaderFactory _factory;

        public DataLoader(DataLoaderFactory factory)
        {
            _factory = factory;
        }

        public Task<List<Candle>> GetStocks(GetStocksRequest request)
        {
            var loader = _factory.GetService(request.Source);
            return loader.GetStocks(request);
        }
    }
}