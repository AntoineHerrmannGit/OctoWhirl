using MathModels.Optimizers;

namespace Maths.Optimizers
{
    public static class Gradient
    {
        public static GradientResult FindMinimum(
            Func<double, double> func,
            double lowerBound = double.NegativeInfinity,
            double upperBound = double.PositiveInfinity,
            double derivationStep = 1e-6,
            double threshold = 1e-10,
            int maxIterations = 25,
            double speed = 0.1,
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

            var x = seed.Value;
            if (x < lowerBound || x > upperBound)
                return new GradientResult
                {
                    Value = x,
                    Error = "Out of bounds",
                    Iterations = 0,
                };

            foreach (var index in Enumerable.Range(0, maxIterations))
            {
                if (x < lowerBound || x > upperBound)
                    return new GradientResult
                    {
                        Value = x,
                        Error = "Out of bounds",
                        Iterations = index,
                    };

                var fl = func(x - derivationStep);
                var f = func(x);
                var fu = func(x + derivationStep);

                var derivative = (fu - fl) / (2 * derivationStep);
                var hessian = (fu - 2 * f + fl) / (4 * derivationStep * derivationStep);

                if (derivative >= -threshold && derivative <= threshold && hessian > 0)
                    return new GradientResult
                    {
                        Value = x,
                        DerivativeResidue = derivative,
                        Iterations = index
                    };

                if (hessian > 0)
                    x -= derivative / hessian * derivationStep * speed;
                else
                    x -= derivative * derivationStep * speed;
            }

            return new GradientResult
            {
                Iterations = maxIterations,
                Value = x,
                Error = "MaxIterations reached",
            };
        }
    }
}
