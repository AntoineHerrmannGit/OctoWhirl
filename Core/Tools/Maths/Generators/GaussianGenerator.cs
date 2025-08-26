using OctoWhirl.Core.Tools.Maths.Generators.Interfaces;

namespace OctoWhirl.Core.Tools.Maths.Generators
{
    public class GaussianGenerator : ISimpleGenerator<double>
    {
        private Random _random;

        private double _mean = 0;
        private double _sigma = 1;
        private readonly int? _seed;

        private double? _cached;

        public GaussianGenerator(double mean = 0, double sigma = 1, int? seed = null)
        {
            _random = seed == null ? new Random() : new Random(seed.Value);
            _seed = seed;

            _mean = mean;
            _sigma = sigma;

            _cached = null;
        }

        public double GetNext()
        {
            double result;
            if (_cached.HasValue)
            {
                result = _cached.Value;
                _cached = null;
                return result;
            }

            double x, y, r;
            do
            {
                x = 2 * _random.NextDouble() - 1;
                y = 2 * _random.NextDouble() - 1;
                r = x * x + y * y;
            }
            while (r >= 1 || r == 0);

            double factor = Math.Sqrt(-2 * Math.Log(r) / r);
            result = _mean + _sigma * x * factor;
            _cached = _mean + _sigma * y * factor;

            return result;
        }

        public void Reset()
        {
            _random = _seed == null ? new Random() : new Random(_seed.Value);
        }
    }
}