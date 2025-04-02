using System;

namespace OctoWhirl.Core.Generators
{
    public class PoissonGenerator : IGenerator<double>
    {
        private readonly Random _random;
        private double _lambda;
        public PoissonGenerator(double lambda = 1)
        {
            if (lambda <= 0)
                throw new ArgumentException("Lambda must be strictly positive");
            
            _random = new Random();
            _lambda = lambda;
        }

        public double GetNext()
        {
            double x = 0;
            double p = Match.Exp(-_lambda);
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
    }
}