using Backtester.StrategyFramework;
using MarketEvents.MarketEvents;

namespace Backtester.TimeLineSimulator
{
    public interface ITimeLineSimulator
    {
        event EventHandler<MarketEvent> MarketEventOccured;

        Task RegisterStrategy(IStrategy strategy);
        Task Init();
        Task Run();
        Task Close();

        Task OnStrategyReact(object sender, MarketEvent marketEvent);
    }
}
