using OctoWhirl.Core.Models.Common;
using OctoWhirl.Core.Models.Enums;
using OctoWhirl.Services.Data.Clients.FinnHubClient;
using OctoWhirl.Services.Data.Clients.YahooFinanceClient;
using OctoWhirl.Services.Models.Requests;

namespace OctoWhirl.Services.Data.Loaders
{
    public class DataLoader : IDataService
    {
        private readonly FinnHubClient _finnHubClient;
        private readonly YahooFinanceClient _yahooFinanceClient;

        public DataLoader(FinnHubClient finnHubClient, YahooFinanceClient yahooFinanceClient)
        {
            _finnHubClient = finnHubClient;
            _yahooFinanceClient = yahooFinanceClient;
        }

        public async Task<List<Candle>> GetStocks(GetStocksRequest request)
        {
            switch (request.Source)
            {
                case ClientSource.FinnHub:
                    return await _finnHubClient.GetStocks(request).ConfigureAwait(false);
                case ClientSource.YahooFinance:
                    return await _yahooFinanceClient.GetStocks(request).ConfigureAwait(false);
                default:
                    throw new NotSupportedException($"The source '{request.Source}' is not supported.");
            }
        }
    }
}