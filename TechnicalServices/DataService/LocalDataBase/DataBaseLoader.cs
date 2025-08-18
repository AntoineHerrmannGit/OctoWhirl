using Microsoft.Extensions.Configuration;
using OctoWhirl.Core.Models.Exceptions;
using OctoWhirl.Core.Models.Models.Common;
using OctoWhirl.Core.Models.Models.Enums;
using OctoWhirl.Core.Models.Models.Requests;
using OctoWhirl.Core.Models.Models.Technicals;
using OctoWhirl.Core.Tools.Serializer;
using OctoWhirl.Core.Tools.Technicals.FileManagement;

namespace OctoWhirl.TechnicalServices.DataService.LocalDataBase
{
    public class DataBaseLoader : IFinanceService
    {
        private readonly string _dataBasePath;

        public DataBaseLoader(IConfiguration configuration)
        {
            var dbPath = configuration.GetRequiredSection("App").GetRequiredSection("DataBaseName").Get<string>() ?? throw new MissingSectionException("DataBaseName");
            _dataBasePath = FileManager.FindDirPath(dbPath);
        }

        #region IFinanceService Methods
        public async Task<List<Candle>> GetCandles(GetCandlesRequest request)
        {
            var historizedMarketDataType = request.Interval == ResolutionInterval.Minute1 ? HistorizedMarketDataType.StockIntraday : HistorizedMarketDataType.StockDaily;
            var tasks = request.References.Distinct().Select(async ticker =>
            {
                var filename = GetFileName(ticker, historizedMarketDataType, request.Source);
                var candles = await File.ReadAllTextAsync(filename).ConfigureAwait(false);
                return candles.DeserializeFromJson<List<Candle>>();
            }).ToList();

            var stocks = await Task.WhenAll(tasks).ConfigureAwait(false);
            return stocks.SelectMany(stock =>  stock).ToList();
        }

        public Task<List<Candle>> GetOption(GetOptionRequest request) => throw new NotImplementedException();
        public Task<List<Option>> GetListedOptions(GetListedOptionRequest request) => throw new NotImplementedException();
        public Task<List<Split>> GetSplits(GetCorporateActionsRequest request) => throw new NotImplementedException();
        public Task<List<Dividend>> GetDividends(GetCorporateActionsRequest request) => throw new NotImplementedException();
        #endregion IFinanceService Methods

        #region Private File Methods
        private string GetFileName(string reference, HistorizedMarketDataType historizedMarketDataType, DataSource source)
        {
            return Path.Combine(_dataBasePath, source.ToString(), historizedMarketDataType.ToString(), $"{reference}.json");
        }
        #endregion Private File Methods
    }
}
