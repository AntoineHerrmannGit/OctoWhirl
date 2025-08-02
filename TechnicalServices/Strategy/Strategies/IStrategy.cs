using OctoWhirl.TechnicalServices.Strategy.Events;
using OctoWhirl.TechnicalServices.Strategy.Simulator;

namespace OctoWhirl.TechnicalServices.Strategy.Strategies
{
    public interface IStrategy : IDisposable
    {
        event EventHandler<MarketEvent>? StrategyEventOccured;

        Task Init();
        Task Attach(ITimeLineSimulator simulator);
        Task OnMarketEvent(object? sender, MarketEvent marketEvent);
        Task ReactToMarket();
    }
}
