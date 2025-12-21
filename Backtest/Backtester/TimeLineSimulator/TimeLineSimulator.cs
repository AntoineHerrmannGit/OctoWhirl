using Backtester.MarketEventProvider;
using Backtester.StrategyFramework;
using MarketEvents.MarketEvents;

namespace Backtester.TimeLineSimulator
{
    public class TimeLineSimulator : ITimeLineSimulator
    {
        private readonly IMarketEventProvider _marketEventProvider;

        private readonly List<IStrategy> _strategies = new();
        private readonly List<MarketEvent> _marketEvents = new();

        // Événement que les stratégies écoutent
        public event EventHandler<MarketEvent> MarketEventOccured;

        public TimeLineSimulator(IMarketEventProvider marketEventProvider)
        {
            _marketEventProvider = marketEventProvider;
        }

        public Task RegisterStrategy(IStrategy strategy)
        {
            if (strategy == null)
                throw new ArgumentNullException(nameof(strategy));

            strategy.StrategyEventOccured += async (sender, marketEvent) =>
                await OnStrategyReact(sender, marketEvent).ConfigureAwait(false);

            _strategies.Add(strategy);
            return Task.CompletedTask;
        }


        public async Task Init()
        {
            await _marketEventProvider.Init().ConfigureAwait(false);

            _marketEvents.Clear();
            _marketEvents.AddRange(await _marketEventProvider.GetMarketEvents().ConfigureAwait(false));
        }

        public Task Run()
        {
            foreach (var marketEvent in _marketEvents.OrderBy(e => e.TimeStamp))
                EmitEvent(marketEvent);

            return Task.CompletedTask;
        }

        public Task OnStrategyReact(object? sender, MarketEvent marketEvent)
        {
            return Task.CompletedTask;
        }

        public Task Close()
        {
            foreach (var strategy in _strategies)
            {
                strategy.StrategyEventOccured -= MarketEventOccured;
                strategy.Terminate();
            }

            _strategies.Clear();
            _marketEvents.Clear();
            return Task.CompletedTask;
        }

        #region Private Methods
        private void EmitEvent(MarketEvent marketEvent)
        {
            MarketEventOccured?.Invoke(this, marketEvent);
        }
        #endregion Private Methods

    }
}
