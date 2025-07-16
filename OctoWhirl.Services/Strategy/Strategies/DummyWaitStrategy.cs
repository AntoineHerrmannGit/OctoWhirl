using OctoWhirl.Services.Strategy.Events;
using System.Reactive.Linq;

namespace OctoWhirl.Services.Strategy.Strategies
{
    public class DummyWaitStrategy : IStrategy
    {
        #region IStrategy Properties
        public IDisposable _orderExecSub { get; set; }
        public StrategyRunner _strategyRunner { get; set; }
        public IEnumerable<string> Universe { get; set; }
        #endregion IStrategy Properties

        #region Strategy Properties
        public int SeenEvents { get; set; }
        #endregion Strategy Properties

        #region Ctor
        public DummyWaitStrategy()
        {
        }
        #endregion Ctor

        #region IStrategy Methods
        public void Attach(StrategyRunner runner)
        {
            _strategyRunner = runner;
            runner.ReceivedEvents.OfType<MarketEvent>()
                                 .Where(_event => Universe.Contains(_event.Reference))
                                 .Subscribe(OnMarketEvent);
        }

        public Task Init()
        {
            SeenEvents = 0;
            return Task.CompletedTask;
        }

        public void OnMarketEvent(MarketEvent marketEvent)
        {
            SeenEvents++;
        }

        #region IDisposable Methods
        public void Dispose()
        {
            _orderExecSub.Dispose();
        }
        #endregion IDisposable Methods
        
        #endregion IStrategy Methods
    }
}