using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OctoWhirl.Core.Models.Technicals;

namespace OctoWhirl.Services.Pricing
{
    public interface IPricer
    {
        Task<double> Price(Func<TimeSerie<double>, double> payoff, int numberOfSimulations = 1000);
    }
}
