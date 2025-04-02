using System;

namespace OctoWhirl.Core.Generators
{
    private readonly Random _random;

    private double _mean = 0;
    private double _sigma = 1;

    public class GaussianGenerator : IGenerator<double>
    {
        public GaussianGenerator(int? seed = null, double mean = 0, double sigma = 1)
        {
            _random = seed == null ? new Random() : new Random(seed);

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

            double normedRandom = x * Math.Sqrt(-2 * Math.Log(r) / r);
            _state = (normedRandom + _mean) * _sigma;
        }
    }
}