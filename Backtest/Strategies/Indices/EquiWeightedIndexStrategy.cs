using Backtester.StrategyFramework;
using MarketEvents.MarketEvents;
using MathModels.Objects.Series;

namespace Strategies.Indices
{
    public class EquiWeightedIndexStrategy : AbstractStrategy
    {
        public event EventHandler<MarketEvent>? StrategyEventOccured;

        private readonly HashSet<string> Universe;
        private readonly Dictionary<string, TimeSerie<double>> InstrumentPricesHistory;
        private readonly string Currency;

        private TimeSerie<double> History;

        public EquiWeightedIndexStrategy(HashSet<string> universe, string currency)
        {
            Universe = universe;
            InstrumentPricesHistory = universe.ToDictionary(instrument => instrument, instrument => new TimeSerie<double>());
            Currency = currency;
        }

        public override Task Init()
        {
            History = new TimeSerie<double>();
            return Task.CompletedTask;
        }

        public override Task OnMarketEvent(object? sender, MarketEvent marketEvent)
        {
            if (marketEvent is PriceUpdateEvent update)
                if (InstrumentPricesHistory.ContainsKey(update.Instrument))
                    InstrumentPricesHistory[update.Instrument].Add(update.TimeStamp, update.Close);

            return Task.CompletedTask;
        }

        public override Task ReactToMarket()
            => Task.CompletedTask;

        public override void Terminate()
        {
            base.Terminate();
            GenerateHistory();
        }

        private void GenerateHistory()
        {
            History = new TimeSerie<double>(
                InstrumentPricesHistory.SelectMany(kvp => kvp.Value.Select(v => ( Date: v.Key, Price: v.Value, Instrument: kvp.Key )))
                                       .GroupBy(x => x.Date)
                                       .ToDictionary(group => group.Key, group => group.Sum(x => x.Price))
            );
        }
    }
}
