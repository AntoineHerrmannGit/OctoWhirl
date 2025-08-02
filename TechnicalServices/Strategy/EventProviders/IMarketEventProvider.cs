using OctoWhirl.TechnicalServices.Strategy.Events;

namespace OctoWhirl.TechnicalServices.Strategy.EventProviders
{
    public interface IMarketEventProvider
    {
        Task Init();
        Task<IEnumerable<MarketEvent>> GetMarketEvents();
    }
}
