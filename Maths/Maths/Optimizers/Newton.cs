using MathModels.Optimizers;
using Maths.Functions;

namespace Maths.Optimizers
{
    public static class Newton
    {
        public static NewtonResult GetZero(
            Func<double, double> func, 
            double lowerBound = double.NegativeInfinity, 
            double upperBound = double.PositiveInfinity, 
            double derivationStep = 1e-6,
            double threshold = 1e-10,
            int maxIterations = 10,
            double? seed = null)
        {
            if (seed is null)
                if (lowerBound == double.NegativeInfinity && upperBound == double.PositiveInfinity)
                    seed = 0;
                else if (lowerBound == double.NegativeInfinity)
                    seed = upperBound - 2 * derivationStep;
                else if (upperBound == double.PositiveInfinity)
                    seed = lowerBound + 2 * derivationStep;
                else
                    seed = (lowerBound + upperBound) / 2;

            double x = seed.Value;
            if (x < lowerBound || x > upperBound)
                return new NewtonResult
                {
                    Value = x,
                    Error = "Out of bounds",
                    Iterations = 0,
                };

            var value = func(x);
            if (value >= -threshold && value <= threshold)
                return new NewtonResult
                {
                    Value = x,
                    Residue = value,
                    Iterations = 0,
                };

            foreach (var index in Enumerable.Range(1, maxIterations))
            {
                var derivative = Derivatives.Derivative(func, x, step: derivationStep);
                if (derivative == 0)
                    return new NewtonResult
                    {
                        Value = x,
                        Residue = value,
                        Error = "Gradient diverged",
                        Iterations = index,
                    };

                x -= value / derivative;
                if (x < lowerBound || x > upperBound)
                    return new NewtonResult
                    {
                        Value = x,
                        Residue = value,
                        Error = "Out of bounds",
                        Iterations = index,
                    };

                value = func(x); 
                if (value >= -threshold && value <= threshold)
                    return new NewtonResult
                    {
                        Value = x,
                        Residue = value,
                        Iterations = index,
                    };
            }

            return new NewtonResult
            {
                Value = x,
                Residue = value,
                Error = "MaxIterations reached",
                Iterations = maxIterations,
            };
        }
    }
}
