using OctoWhirl.Core.Models.Models.Technicals;
using OctoWhirl.Core.Tools.Maths.Generators;

namespace OctoWhirl.Core.Pricing.Dynamics
{
    public class BlackSholesDynamics : IDynamics
    {
        private readonly TimeSerieGenerator _generator;
        private readonly DateTime _startDate;
        private readonly DateTime _endDate;
        private readonly ResolutionInterval _resolutionInterval;
        private readonly double _rate;
        private readonly double _volatility;

        public BlackSholesDynamics(DateTime startDate, DateTime endDate, double rate=0.04, double volatility=0.2, ResolutionInterval interval = ResolutionInterval.Day)
        {
            _generator = new TimeSerieGenerator(new LogBrownianGenerator(rate, volatility), startDate, endDate, interval);
            _startDate = startDate;
            _endDate = endDate;
            _resolutionInterval = interval;
            _rate = rate;
            _volatility = volatility;
        }

        public Task<TimeSerie<double>> GeneratePath()
        {
            return Task.FromResult(_generator.GetNext());
        }

        public void Reset()
        {
            _generator.Reset();
        }
    }
}
