using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OctoWhirl.Core.Generators;
using OctoWhirl.Core.Models.Technicals;

namespace OctoWhirl.Services.Pricing.MonteCarlo
{
    public class MonteCarloPricer : IPricer
    {
        private readonly IGenerator<TimeSerie<double>> _generator;

        public MonteCarloPricer(IGenerator<TimeSerie<double>> generator)
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
