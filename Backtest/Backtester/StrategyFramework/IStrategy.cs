using Backtester.TimeLineSimulator;
using MarketEvents.MarketEvents;

namespace Backtester.StrategyFramework
{
    public interface IStrategy
    {
        event EventHandler<MarketEvent>? StrategyEventOccured;

        Task Init();
        Task Attach(ITimeLineSimulator simulator);
        Task OnMarketEvent(object? sender, MarketEvent marketEvent);
        Task ReactToMarket();
        void Terminate();
    }
}
