namespace OctoWhirl.Core.Tools.Maths.Functions
{
    public static class Distributions
    {
        public static double GaussianDistribution(double x, double mean, double stdDev) => 1 / stdDev * Functions.Normal((x - mean) / stdDev);

        public static double CumulativeGaussianDistribution(double x, double mean, double stdDev) => 0.5 * (1 + Functions.Erf((x - mean) / stdDev));
    }
}
