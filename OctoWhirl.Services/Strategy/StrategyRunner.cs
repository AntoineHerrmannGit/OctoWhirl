using OctoWhirl.Core.Models.Common;
using OctoWhirl.Core.Models.Enums;
using OctoWhirl.Core.Models.Technicals;
using OctoWhirl.Services.Data;
using OctoWhirl.Services.Models.Requests;
using OctoWhirl.Services.Strategy.Events;
using OctoWhirl.Services.Strategy.KPI;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace OctoWhirl.Services.Strategy
{
    public class StrategyRunner : IDisposable
    {
        private readonly IDataService _dataService;

        private readonly Subject<MarketEvent> _eventHandler;
        private readonly IDisposable _orderSubscription;

        private readonly IDisposable _orderExecSub;
        private readonly IDisposable _recordSub;
        private List<MarketEvent> _backtestEvents;

        // KPI run for report
        private StrategyRunKPIReport _strategyRunKPI;

        private List<string> _universe;
        private DateTime _startTime;
        private DateTime _endTime;

        // Incoming events sent by the strategies
        public IObservable<MarketEvent> ReceivedEvents => _eventHandler.AsObservable();

        public StrategyRunner(IDataService dataService)
        {
            _dataService = dataService;

            // Setupping new handler to avoid memory side effects of events
            _eventHandler = new Subject<MarketEvent>();
            _backtestEvents = new List<MarketEvent>();

            // Responding Flux to incoming market events
            _orderSubscription = _eventHandler.OfType<MarketEvent>()
                                              .Subscribe(OnMarketOrder);
        }

        #region Backtest Methods
        public async Task RunBacketest(IEnumerable<IStrategy> strategies, DateTime staratDate, DateTime endDate)
        {
            var watch = Stopwatch.StartNew();
            _startTime = staratDate;
            _endTime = endDate;
            _universe = strategies.SelectMany(strategy => strategy.Universe).Distinct().ToList();

            await Init().ConfigureAwait(false);
            var initializationTime = watch.Elapsed;

            watch = Stopwatch.StartNew();
            await Run().ConfigureAwait(false);
            var runTime = watch.Elapsed;

            watch = Stopwatch.StartNew();
            await Close().ConfigureAwait(false);
            var closeTime = watch.Elapsed;

            _strategyRunKPI.InitializationTime = initializationTime;
            _strategyRunKPI.RumTime = runTime;
            _strategyRunKPI.CloseTime = closeTime;
        }
        #endregion Backtest Methods

        #region Generic Init Methods
        public async Task Init()
        {
            await GenerateEvents().ConfigureAwait(false);
        }

        // TODO: Modify this methods so that it matches the comment
        // Initializes all possible events from the universe of the Strategy
        // between a startdate and an enddate (limits of the run).
        private async Task GenerateEvents()
        {
            _backtestEvents = new List<MarketEvent>();

            var stocks = await LoadStocks().ConfigureAwait(false);

            _backtestEvents.AddRange(MapCandleToStocks(stocks));
        }

        #region Private Candles Methods
        private Task<List<Candle>> LoadStocks()
        {
            var stocksRequest = new GetStocksRequest
            {
                Tickers = _universe,
                Source = DataSource.YahooFinance,
                StartDate = _startTime,
                EndDate = _endTime,
                Interval = ResolutionInterval.Day,
            };
            return _dataService.GetStocks(stocksRequest);
        }

        private IEnumerable<PriceUpdateEvent> MapCandleToStocks(IEnumerable<Candle> candles)
        {
            return candles.Select(candle => new PriceUpdateEvent
            {
                Reference = candle.Reference,
                TimeStamp = candle.Timestamp,
                Open = candle.Open,
                Close = candle.Close,
                Currency = candle.Currency,
                High = candle.High,
                Low = candle.Low,
            });
        }
        #endregion Private Candles Methods

        #endregion Generic Init Methods

        #region Generic Run Methods
        // Runs the time, consumes all generated events
        public Task Run()
        {
            foreach (var evt in _backtestEvents.OrderBy(_event => _event.TimeStamp))
               Emit(evt);

            return Task.CompletedTask;
        }
        #endregion Generic Run Methods

        #region Generic Close Methods
        // Terminates the strategy and analyses the results
        public Task Close()
        {
            _eventHandler.OnCompleted();
            AnalyzeKpi();
            Dispose();

            return Task.CompletedTask;
        }

        #region KPI Methods
        // TODO: Implement this method if required
        // Computes and stores KPIs
        private void AnalyzeKpi()
        {
            _strategyRunKPI.NbOfEvents = _backtestEvents.Count;
        }
        #endregion KPI Methods

        #endregion Generic Close Methods

        #region Event Methods
        // Emits the next event
        public void Emit(MarketEvent evt)
            => _eventHandler.OnNext(evt);

        // Manages incoming events
        private void OnMarketOrder(MarketEvent order)
        {
            var execution = new MarketEvent
            {
                TimeStamp = DateTime.Now,
                Reference = string.Empty,
            };
            _eventHandler.OnNext(execution);
        }
        #endregion Event Methods

        #region IDisposable Methods
        public void Dispose()
        {
            _orderExecSub.Dispose();
            _recordSub.Dispose();
            _eventHandler.OnCompleted();
            _eventHandler.Dispose();
        }
        #endregion IDisposable Methods
    }
}
