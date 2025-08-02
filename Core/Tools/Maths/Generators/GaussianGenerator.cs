namespace OctoWhirl.Core.Tools.Generators
{
    public class GaussianGenerator : IGenerator<double>
    {
        private Random _random;

        private double _mean = 0;
        private double _sigma = 1;
        private readonly int? _seed;

        public GaussianGenerator(double mean = 0, double sigma = 1, int? seed = null)
        {
            _random = seed == null ? new Random() : new Random(seed.Value);
            _seed = seed;

            _mean = mean;
            _sigma = sigma;
        }

        public double GetNext()
        {
            double x, y, r;
            do
            {
                x = _random.NextDouble();
                y = _random.NextDouble();
                r = x * x + y * y;
            }
            while(r*r > 1);

            return _mean + _sigma * x * Math.Sqrt(-2 * Math.Log(r) / r);
        }

        public void Reset()
        {
            _random = _seed == null ? new Random() : new Random(_seed.Value);
        }
    }
}