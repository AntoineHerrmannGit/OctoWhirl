namespace OctoWhirl.Core.Generators
{
    public class BrownianGenerator : IGenerator<double>
    {
        protected readonly IGenerator<double> _random;
        
        protected double _mean;
        protected double _sigma;

        protected double _step;

        public BrownianGenerator(double mean = 0, double sigma = 1, double step = 0.01, IGenerator<double>? generator = null, int? seed = null)
        {
            _random = generator ?? new GaussianGenerator(mean, sigma * Math.Sqrt(step), seed);

            _mean = mean;
            _sigma = sigma;

            _step = step;
        }

        public virtual double GetNext()
        {
            return _mean * _step + _sigma * _random.GetNext();
        }

        public virtual void Reset()
        {
            _random.Reset();
        }
    }
}