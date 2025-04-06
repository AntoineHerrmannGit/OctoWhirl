namespace OctoWhirl.Core.Generators
{
    public class BrownianGenerator : IGenerator<double>
    {
        private readonly IGenerator<double> _random;
        
        private double _mean;
        private double _sigma;

        private double _step;

        public BrownianGenerator(double mean = 0, double sigma = 1, IGenerator<double>? generator = null, double step = 0.01, int? seed = null)
        {
            _random = generator == null ? new GaussianGenerator(0, Math.Sqrt(step), seed) : generator;

            _mean = mean;
            _sigma = sigma;

            _step = step;
        }

        public double GetNext()
        {
            return _mean * _step + _sigma * _random.GetNext();
        }
    }
}