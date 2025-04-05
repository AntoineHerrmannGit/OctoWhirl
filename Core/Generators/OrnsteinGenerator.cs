using System;

namespace OctoWhirl.Core.Generators
{
    public class OrnsteinGenerator : IGenerator<double>
    {
        private readonly GaussianGenerator _random;

        private double _mean;
        private double _reversion;
        private double _sigma;

        private double _step;
        private double _state;

        public OrnsteinGenerator(double mean = 0, double sigma = 1, double reversion = 1, double initialState = 0, double step = 0.01, int? seed = null)
        {
            _random = new GaussianGenerator(seed: seed);

            _mean = mean;
            _reversion = reversion;
            _sigma = sigma * Math.Sqrt(step);

            _state = initialState;
            _step = step;
        }

        public double GetNext()
        {
            return _reversion * (_mean - _state) * _step + _sigma * _random.GetNext();
        }
    }
}