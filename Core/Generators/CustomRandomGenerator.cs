namespace OctoWhirl.Core.Generators
{
    public class CustomRandomGenerator : IGenerator<double>
    {
        private Random _random;
        private Func<double, double> _distribution;

        public CustomRandomGenerator<T>(Func<double, double> distribution, int? seed = null)
        {
            _random = seed = null ? new Random() : new Random(seed);
            _distribution = distribution;
        }

        public double GetNext()
        {
            double x, y, f;
            do
            {
                x = _random.NextDouble();
                y = _random.NextDouble();
                f = _distribution(x)
            }
            while (y > f)
            return x;
        }
    }
}