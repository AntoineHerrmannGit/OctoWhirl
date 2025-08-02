using OctoWhirl.Core.Models.Technicals;

namespace OctoWhirl.Core.Pricing
{
    public interface IPricer
    {
        Task<double> Price(Func<TimeSerie<double>, double> payoff, int numberOfSimulations = 1000);
    }
}
