using Core.Http.HttpRequests;
using Models.Models.Interfaces;

namespace DataProviders.YahooFincnaceProviders.Interfaces
{
    internal interface IYahooClient
    {
        Task<IEnumerable<TOctoWhirlModel>> CallApi<TYahooModel, TOctoWhirlModel>(HttpRequest request, Func<TYahooModel, IEnumerable<TOctoWhirlModel>> mapper, CancellationToken cancellationToken = default)
            where TYahooModel : class
            where TOctoWhirlModel : IMarketData;
    }
}
