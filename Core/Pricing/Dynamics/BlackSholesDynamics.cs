using OctoWhirl.Core.Models.Models.Technicals;
using OctoWhirl.Core.Tools.Maths.Generators;

namespace OctoWhirl.Core.Pricing.Dynamics
{
    public class BlackSholesDynamics : IDynamics
    {
        private readonly TimeSerieGenerator _generator;
        private readonly double _initialState;
        private readonly double _step;

        private readonly DateTime _startDate;
        private readonly DateTime _endDate;
        private readonly ResolutionInterval _interval;

        private readonly double _rate;
        private readonly double _volatility;

        public BlackSholesDynamics(DateTime startDate, DateTime endDate, double rate=0.04, double volatility=0.2, ResolutionInterval interval = ResolutionInterval.Day, double initialState = 100, double step = 0.01)
        {
            _initialState = initialState;
            _step = step;

            _startDate = startDate;
            _endDate = endDate;
            _interval = interval;
            
            _rate = rate;
            _volatility = volatility;

            _generator = new TimeSerieGenerator(
                new LogBrownianGenerator(
                    mean: _rate, sigma: _volatility, step: _step, initialState: _initialState
                ),
                _startDate, _endDate, _interval
            );
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
