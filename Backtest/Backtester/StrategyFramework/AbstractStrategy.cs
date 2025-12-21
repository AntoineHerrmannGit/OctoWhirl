using Backtester.TimeLineSimulator;
using MarketEvents.MarketEvents;

namespace Backtester.StrategyFramework
{
    public abstract class AbstractStrategy : IStrategy
    {
        protected ITimeLineSimulator? Simulator { get; private set; }

        public event EventHandler<MarketEvent>? StrategyEventOccured;

        public Task Attach(ITimeLineSimulator simulator)
        {
            StrategyEventOccured = async (sender, evt) => await OnMarketEvent(sender, evt).ConfigureAwait(false);
            simulator.MarketEventOccured += StrategyEventOccured;

            Simulator = simulator;
            return Task.CompletedTask;
        }

        protected Task EmitMarketEvent(MarketEvent marketEvent)
        {
            StrategyEventOccured?.Invoke(this, marketEvent);
            return Task.CompletedTask;
        }

        public abstract Task Init();
        public abstract Task OnMarketEvent(object? sender, MarketEvent marketEvent);
        public abstract Task ReactToMarket();

        public virtual void Terminate()
        {
            if (Simulator != null && StrategyEventOccured != null)
            {
                Simulator.MarketEventOccured -= StrategyEventOccured;
                StrategyEventOccured = null;
                Simulator = null;
            }
        }
    }
}