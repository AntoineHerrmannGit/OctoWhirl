using OctoWhirl.TechnicalServices.Strategy.Events;
using OctoWhirl.TechnicalServices.Strategy.Simulator;

namespace OctoWhirl.TechnicalServices.Strategy.Strategies
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

        public void Dispose()
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