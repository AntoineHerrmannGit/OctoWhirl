namespace Maths.Functions
{
    public static class Derivatives
    {
        public static double Derivative(Func<double, double> func, double x, double step = 1e-6)
            => (func(x + step) - func(x - step)) / (2*step);

        public static double SecondDerivative(Func<double, double> func, double x, double step = 1e-6)
            => (func(x + step) - 2 * func(x) + func(x - step)) / (4 * step * step);
    }
}
