using OctoWhirl.Services.Strategy.Events;

namespace OctoWhirl.Services.Strategy
{
    public interface IStrategy : IDisposable
    {
        IDisposable _orderExecSub { get; set; }
        StrategyRunner _strategyRunner { get; set; }

        IEnumerable<string> Universe { get; set; }
        void Attach(StrategyRunner runner);
        void OnMarketEvent(MarketEvent marketEvent);
        Task Init();
        void Dispose();
    }

}
