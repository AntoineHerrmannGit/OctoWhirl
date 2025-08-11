using OctoWhirl.TechnicalServices.Strategy.EventProviders;
using OctoWhirl.TechnicalServices.Strategy.Events;
using OctoWhirl.TechnicalServices.Strategy.Strategies;

namespace OctoWhirl.TechnicalServices.Strategy.Simulator
{
    public class TimeLineSimulator : ITimeLineSimulator
    {
        private bool _isDisposed;
        private readonly IMarketEventProvider _marketEventProvider;

        private readonly List<IStrategy> _strategies = new();
        private readonly List<MarketEvent> _marketEvents = new();

        // Événement que les stratégies écoutent
        public event EventHandler<MarketEvent>? MarketEventOccured;

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
            Dispose();
            return Task.CompletedTask;
        }

        #region Private Methods
        private void EmitEvent(MarketEvent marketEvent)
        {
            MarketEventOccured?.Invoke(this, marketEvent);
        }
        #endregion Private Methods

        #region IDisposable
        public void Dispose()
        {
            if (!_isDisposed)
            {
                foreach (var strategy in _strategies)
                {
                    strategy.StrategyEventOccured -= MarketEventOccured;
                    strategy.Dispose();
                }

                _strategies.Clear();
                _marketEvents.Clear();
                GC.SuppressFinalize(this);
            }
            _isDisposed = true;
        }
        #endregion IDisposable
    }
}
