using OctoWhirl.Core.Models.Models.Technicals;
using OctoWhirl.Core.Tools.Maths.Generators;

namespace OctoWhirl.Core.Pricing.Dynamics
{
    public class HestonDynamics : IDynamics
    {
        private readonly TimeSerieGenerator _generator;
        private readonly DateTime _startDate;
        private readonly DateTime _endDate;
        private readonly ResolutionInterval _interval;
        private readonly double _rate;
        private readonly double _volOfVol;
        private readonly double _averageVolatility;
        private readonly double _spotVolCorrelation;
        private readonly double _volatilityReversionStrength;

        private readonly double _initialVolatility;

        public HestonDynamics(DateTime startDate, DateTime endDate, ResolutionInterval interval, 
            double rate, double averageVolatility, double volOfVol, double spotVolCorrelation, double volatilityReversionStrength = 1, double initialVolatility = 0.2)
        {
            _startDate = startDate;
            _endDate = endDate;
            _interval = interval;

            _rate = rate;
            _volOfVol = volOfVol;
            _averageVolatility = averageVolatility;
            _volatilityReversionStrength = volatilityReversionStrength;
            _spotVolCorrelation = spotVolCorrelation;

            _initialVolatility = initialVolatility;

            _generator = new TimeSerieGenerator(
                new LogHestonGenerator(
                    drift: _rate, volatilityMean: _averageVolatility, volOfVol: _volOfVol, 
                    spotVolCorrelation: _spotVolCorrelation, reversionStrength: _volatilityReversionStrength,
                    initialVolatility: initialVolatility
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
