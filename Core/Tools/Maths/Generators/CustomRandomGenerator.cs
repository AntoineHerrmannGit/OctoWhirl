namespace OctoWhirl.Core.Tools.Maths.Generators
{
    public class CustomRandomGenerator : IGenerator<double>
    {
        private Random _random;
        private Func<double, double> _distribution;
        private readonly int? _seed;

        public CustomRandomGenerator(Func<double, double> distribution, int? seed = null)
        {
            _random = seed == null ? new Random() : new Random(seed.Value);
            _distribution = distribution;
            _seed = seed;
        }

        public double GetNext()
        {
            double x, y, f;
            do
            {
                x = _random.NextDouble();
                y = _random.NextDouble();
                f = _distribution(x);
            }
            while (y > f);
            return x;
        }

        public void Reset()
        {
            _random = _seed == null ? new Random() : new Random(_seed.Value);
        }
    }
}