using OctoWhirl.Core.Models.Technicals;
using OctoWhirl.Services.Strategy.Events;
using System.Reactive.Linq;

namespace OctoWhirl.Services.Strategy.Strategies
{
    public class MeanRebalancingStrategy : IStrategy
    {
        #region IStrategy Properties
        public IDisposable _orderExecSub { get; set; }
        public StrategyRunner _strategyRunner { get; set; }
        public IEnumerable<string> Universe { get; set; }
        #endregion IStrategy Properties

        #region Strategy Properties
        public Dictionary<string, double> Positions;
        public TimeSerie<double> Valorization;
        public TimeSpan RebalancingFrequency;
        #endregion Strategy Properties

        #region Ctor
        public MeanRebalancingStrategy(IEnumerable<string> universe, TimeSpan rebalancingFrequency)
        {
            Universe = universe;
            Positions = universe.ToDictionary(x => x, x => 0d);
            Valorization = new TimeSerie<double>();
            RebalancingFrequency = rebalancingFrequency;
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
            return Task.CompletedTask;
        }

        public void OnMarketEvent(MarketEvent marketEvent)
        {

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