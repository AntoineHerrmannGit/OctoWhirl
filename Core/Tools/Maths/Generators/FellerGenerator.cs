using OctoWhirl.Core.Tools.Maths.Generators.Interfaces;

namespace OctoWhirl.Core.Tools.Maths.Generators
{
    public class FellerGenerator : ISimpleGenerator<double>
    {
        private double _state;
        private double _initialState;

        private double _reversionStrength;
        private double _mean;
        private double _volatility;

        private double _step;
        private ISimpleGenerator<double> _generator;

        public FellerGenerator(double reversionStrength, double mean, double volatility, double initialState = 0, double step = 0.01, ISimpleGenerator<double>? generator = null)
        {
            _state = initialState;
            _initialState = initialState;

            _reversionStrength = reversionStrength;
            _mean = mean;
            _volatility = volatility;

            _step = step;
            _generator = generator ?? new BrownianGenerator(step: step);
        }

        public double GetNext()
        {
            double lastState = _state;
            _state += _reversionStrength * (_mean - _state) * _step + _volatility * Math.Sqrt(_state) * _generator.GetNext();
            return lastState;
        }

        public void Reset()
        {
            _state = _initialState;
            _generator.Reset();
        }
    }
}
