using OctoWhirl.Core.Models.Common;
using OctoWhirl.Core.Models.Enums;
using OctoWhirl.Core.Models.Technicals;
using OctoWhirl.Services.Data.Clients.FinnHubClient;
using OctoWhirl.Services.Data.Clients.YahooFinanceClient;

namespace OctoWhirl.Services.Data
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

        public async Task<List<Candle>> GetStocks(string reference, DateTime startDate, DateTime endDate, ClientSource source, ResolutionInterval interval)
        {
            switch (source)
            {
                case ClientSource.FinnHub:
                    return await _finnHubClient.GetStock(reference, startDate, endDate, interval).ConfigureAwait(false);
                case ClientSource.YahooFinance:
                    return await _yahooFinanceClient.GetStock(reference, startDate, endDate, interval).ConfigureAwait(false);
                default:
                    throw new NotSupportedException($"The source '{source}' is not supported.");
            }
        }
    }
}