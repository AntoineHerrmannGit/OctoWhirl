using OctoWhirl.Core.Models.Models.Technicals;

namespace OctoWhirl.Core.Tools.Maths.Optimizers
{
    public static class Newton
    {
        public static double Derive(Func<double, double> function, double point, double step = 1e-6, int order = 1)
        {
            if (order == 0)
                return function(point);

            return (Derive(function, point + step, step, order - 1) - Derive(function, point - step, step, order - 1)) / (2 * step);
        }

        public static NewtonZeroResult GetZero(Func<double, double> function, double seed, double step = 1e-6,
            double lowerBound = double.NegativeInfinity, double upperbound = double.PositiveInfinity,
            int maxIterations = 10, double tolerance = 1e-8)
        {
            int iteration = 0;
            double point = seed;

            double value = function(point);
            if (value < tolerance)
                return new NewtonZeroResult
                {
                    Value = point,
                    Iterations = iteration,
                    HasConverged = true,
                    Distance = Math.Abs(value)
                };

            while (iteration < maxIterations)
            {
                value = function(point);
                double derivative = Derive(function, point, step, 1);
                if (Math.Abs(derivative) < tolerance)
                    return new NewtonZeroResult
                    {
                        Value = point,
                        Iterations = iteration,
                        HasConverged = false,
                        Distance = Math.Abs(value),
                        Error = "Derivative is too small"
                    };
                double nextPoint = point - value / derivative;
                if (nextPoint < lowerBound || nextPoint > upperbound)
                    return new NewtonZeroResult
                    {
                        Value = nextPoint,
                        Iterations = iteration,
                        HasConverged = false,
                        Distance = Math.Abs(value),
                        Error = "Out of bounds"
                    };
                if (Math.Abs(nextPoint - point) < tolerance)
                    return new NewtonZeroResult
                    {
                        Value = nextPoint,
                        Iterations = iteration,
                        HasConverged = true,
                        Distance = Math.Abs(value)
                    };
                point = nextPoint;
                iteration++;
            }

            return new NewtonZeroResult
            {
                Value = point,
                Iterations = iteration,
                HasConverged = false,
                Distance = Math.Abs(value),
                Error = "Reached max number of Iterations"
            };
        }
    }
}
