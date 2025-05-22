using Microsoft.Extensions.Configuration;
using OctoWhirl.Core.Models.Common;
using OctoWhirl.Core.Models.Enums;
using OctoWhirl.Core.Models.Technicals;
using OctoWhirl.Core.Tools;
using OctoWhirl.Services.Data.DataBaseModels;
using OctoWhirl.Services.Models.Requests;

namespace OctoWhirl.Services.Data.Loaders
{
    public class DataBaseLoader : IDataService
    {
        public DataBaseLoader(IConfiguration configuration)
        {
        }

        public async Task<List<Candle>> GetStocks(GetStocksRequest request)
        {
            var historizedMarketDataType = request.Interval == ResolutionInterval.Minute1 ? HistorizedMarketDataType.StockIntraday : HistorizedMarketDataType.StockDaily;
            var tasks = request.Tickers.Distinct().Select(async ticker =>
            {
                var filename = GetFileName(ticker, historizedMarketDataType, request.Source);
                var dbSpots = (await File.ReadAllTextAsync(filename).ConfigureAwait(false)).Deserialize<List<DBSpot>>();
                var candles = MapDBSpotToCandle(dbSpots);
                return candles;
            }).ToList();

            var stocks = await Task.WhenAll(tasks).ConfigureAwait(false);
            return stocks.SelectMany(stock =>  stock).ToList();
        }

        #region Private File Methods
        private static string GetFileName(string reference, HistorizedMarketDataType historizedMarketDataType, ClientSource source)
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var filename = Path.Combine(currentDirectory, source.ToString(), historizedMarketDataType.ToString(), reference, ".json");
            return filename;
        }

        private static List<Candle> MapDBSpotToCandle(List<DBSpot> dbSpots)
        {
            return dbSpots.Select(spot => new Candle
            {
                Reference = spot.ticker,
                Currency = spot.currency,
                Timestamp = spot.timestamp,
                Open = spot.open,
                High = spot.high,
                Low = spot.low,
                Close = spot.close,
                Volume = spot.volume,
            }).OrderBy(spot => spot.Timestamp).ToList();
        }
        #endregion Private File Methods
    }
}
