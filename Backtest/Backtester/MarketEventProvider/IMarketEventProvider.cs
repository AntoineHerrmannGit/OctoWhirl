using MarketEvents.MarketEvents;

namespace Backtester.MarketEventProvider
{
    public interface IMarketEventProvider
    {
        Task Init();
        Task<IEnumerable<MarketEvent>> GetMarketEvents();
    }
}
