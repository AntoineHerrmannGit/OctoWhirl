using OctoWhirl.TechnicalServices.Strategy.EventProviders;
using OctoWhirl.TechnicalServices.Strategy.Events;

namespace OctoWhirl.Test.Strategy
{
    internal class MarketEventProviderMock : IMarketEventProvider
    {
        private List<MarketEvent> _events;

        public Task Init()
        {
            _events = new List<MarketEvent>();
            for(int i = 0; i < 10; i++)
            {
                _events.Add(new MarketEvent
                {
                    Reference = $"Event_{i}",
                    TimeStamp = DateTime.Now,
                });
            }
            return Task.CompletedTask;
        }

        public Task<IEnumerable<MarketEvent>> GetMarketEvents()
        {
            return Task.FromResult(_events.AsEnumerable());
        }
    }
}
