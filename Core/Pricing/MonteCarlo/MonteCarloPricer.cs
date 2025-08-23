using OctoWhirl.Core.Models.Models.Technicals;
using OctoWhirl.Core.Tools.Maths.Generators.Interfaces;

namespace OctoWhirl.Core.Pricing.MonteCarlo
{
    public class MonteCarloPricer : IPricer
    {
        private readonly ISimpleGenerator<TimeSerie<double>> _generator;

        public MonteCarloPricer(ISimpleGenerator<TimeSerie<double>> generator)
        {
            _generator = generator;
        }

        public Task<double> Price(Func<TimeSerie<double>, double> payoff, int numberOfSimulations = 1000)
        {
            var prices = new List<double>();
            for (int i = 0; i < numberOfSimulations; i++)
            {
                var timeSerie = _generator.GetNext();
                var payoffValue = payoff(timeSerie);
                prices.Add(payoffValue);
            }

            return Task.FromResult(prices.Average());
        }
    }
}
