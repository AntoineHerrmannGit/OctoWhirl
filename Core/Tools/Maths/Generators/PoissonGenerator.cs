namespace OctoWhirl.Core.Tools.Maths.Generators
{
    public class PoissonGenerator : IGenerator<double>
    {
        private Random _random;
        private readonly double _lambda;
        private readonly int? _seed;

        public PoissonGenerator(double lambda = 1, int? seed = null)
        {
            if (lambda <= 0)
                throw new ArgumentException("Lambda must be strictly positive");
            
            _random = seed == null ? new Random() : new Random(seed.Value);
            _seed = seed;

            _lambda = lambda;
        }

        public double GetNext()
        {
            double x = 0;
            double p = Math.Exp(-_lambda);
            double s = p;
            double r = _random.NextDouble();
            while(r > s)
            {
                x++;
                p *= _lambda / x;
                s += p;
            }

            return x;
        }

        public void Reset()
        {
            _random = _seed == null ? new Random() : new Random(_seed.Value);
        }
    }
}