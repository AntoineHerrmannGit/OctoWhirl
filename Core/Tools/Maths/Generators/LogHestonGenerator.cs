using OctoWhirl.Core.Tools.Maths.Generators.Interfaces;

namespace OctoWhirl.Core.Tools.Maths.Generators
{
    public class LogHestonGenerator : ISimpleGenerator<double>
    {
        private readonly double _drift;
        private readonly double _reversionStrength;
        private readonly double _volatilityMean;
        private readonly double _volOfVol;
        private readonly double _spotVolCorrelation;

        private double _volatilityState;
        private double _initialVolatility;

        private readonly double _step;
        private readonly double _initialState;
        private readonly IMultiGenerator<double> _generator;

        private double _state;
        private readonly double _sqrtStep;

        public LogHestonGenerator(double drift = 0.04, double volatilityMean = 0.2, double volOfVol = 0.2, double spotVolCorrelation = 0.8, 
                                  double reversionStrength = 1, double initialVolatility = 0, double step = 0.01, double initialState = 100, IMultiGenerator<double>? generator = null)
        {
            _drift = drift;
            _reversionStrength = reversionStrength;
            _volatilityMean = volatilityMean;
            _volOfVol = volOfVol;
            _spotVolCorrelation = spotVolCorrelation;

            _volatilityState = initialVolatility;
            _initialVolatility = initialVolatility;

            _step = step;
            _initialState = initialState;
            _state = initialState;

            _sqrtStep = Math.Sqrt(_step);

            double[,] matrix = new double[,] { { 1, _spotVolCorrelation }, { _spotVolCorrelation, 1 } };
            _generator = generator ?? new MultiVariableGenerator(matrix, new GaussianGenerator(mean: 0, sigma: 1));
        }

        public double GetNext()
        {
            double currentVolatility = _volatilityState;
            double[] brownianMotion = _generator.GetNext();

            double volatilityStep = _reversionStrength * (_volatilityMean - _volatilityState) * _step + _volOfVol * Math.Sqrt(_volatilityState) * _sqrtStep * brownianMotion[0];
            _volatilityState = Math.Abs(_volatilityState + volatilityStep);
            double evolution = _drift * _step + Math.Sqrt(_volatilityState) * _sqrtStep * brownianMotion[1];

            double currentState = _state;
            _state *= 1 + evolution;
            return currentState;
        }

        public void Reset()
        {
            _volatilityState = _initialVolatility;
            _state = _initialState;
            _generator.Reset();
        }
    }
}
