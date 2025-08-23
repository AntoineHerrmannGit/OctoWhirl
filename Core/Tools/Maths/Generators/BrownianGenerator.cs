using OctoWhirl.Core.Tools.Maths.Generators.Interfaces;

namespace OctoWhirl.Core.Tools.Maths.Generators
{
    public class BrownianGenerator : ISimpleGenerator<double>
    {
        protected readonly ISimpleGenerator<double> _random;
        
        protected readonly double _mean;
        protected readonly double _sigma;

        protected readonly double _step;

        private readonly double _sqrtStep;

        public BrownianGenerator(double mean = 0, double sigma = 1, double step = 0.01, ISimpleGenerator<double>? generator = null, int? seed = null)
        {
            _random = generator ?? new GaussianGenerator(mean: 0, sigma: 1, seed: seed);

            _mean = mean;
            _sigma = sigma;

            _step = step;
            _sqrtStep = Math.Sqrt(_step);
        }

        public virtual double GetNext()
        {
            return _mean * _step + _sigma * _sqrtStep * _random.GetNext();
        }

        public virtual void Reset()
        {
            _random.Reset();
        }
    }
}