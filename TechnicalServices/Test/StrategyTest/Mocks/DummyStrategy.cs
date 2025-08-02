using OctoWhirl.TechnicalServices.Strategy.Events;
using OctoWhirl.TechnicalServices.Strategy.Strategies;

namespace Strategy.Test.Mocks
{
    internal class DummyStrategy : AbstractStrategy
    {
        public int EventsOccured;

        public override Task Init()
        {
            EventsOccured = 0;
            return Task.CompletedTask;
        }

        public override Task OnMarketEvent(object? sender, MarketEvent marketEvent)
        {
            EventsOccured++;
            return Task.CompletedTask;
        }

        public override Task ReactToMarket()
        {
            return Task.CompletedTask;
        }
    }
}
