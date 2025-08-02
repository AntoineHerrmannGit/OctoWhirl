using OctoWhirl.TechnicalServices.Strategy.Events;
using OctoWhirl.TechnicalServices.Strategy.Strategies;

namespace OctoWhirl.TechnicalServices.Strategy.Simulator
{
    public interface ITimeLineSimulator : IDisposable
    {
        event EventHandler<MarketEvent> MarketEventOccured;

        Task RegisterStrategy(IStrategy strategy);
        Task Init();
        Task Run();
        Task Close();

        Task OnStrategyReact(object sender, MarketEvent marketEvent);
    }
}
