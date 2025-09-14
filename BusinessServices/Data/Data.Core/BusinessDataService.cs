using DataService.Factory;
using OctoWhirl.Core.Models.Models.Common;
using OctoWhirl.Core.Models.Models.Requests;
using OctoWhirl.Core.Models.Models.Technicals;

namespace OctoWhirl.BusinessServices.Data.Core
{
    public class BusinessDataService : IBusinessDataService
    {
        private readonly FinancialServiceFactory _dataServiceFactory;

        public BusinessDataService(FinancialServiceFactory dataServiceFactory)
        {
            _dataServiceFactory = dataServiceFactory;
        }

        public async Task<List<CandleSerie>> GetCandles(GetCandlesRequest request)
        {
            var dataService = _dataServiceFactory.GetService(request.Source);
            var candles = await dataService.GetCandles(request).ConfigureAwait(false);

            return candles.GroupBy(c => c.Instrument)
                                .Select(g => new CandleSerie(g.Key, g.Select(c => new KeyValuePair<DateTime, Candle>(c.Timestamp, c))))
                                .ToList();
        }
    }
}
