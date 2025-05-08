namespace OctoWhirl.Core.Maths.Functions
{
    public static class Functions
    {
        public static int Sign(double x) => x < 0 ? -1 : (x > 0 ? 1 : 0);

        /// <summary>
        /// 1 / Sqrt(2 * Pi) * e^(-0.5 * x^2)
        /// </summary>
        public static double Normal(double x) => Constants.INVERSE_SQRT_2PI * Math.Exp(-0.5 * x * x);

        /// <summary>
        /// 1 / Sqrt(2 / Pi) * Integral from 0 to x of e^(-0.5 * t^2) dt
        /// </summary>
        public static double Erf(double x)
        {
            if (x > -5 && x < 5)
            {
                // Constants for the approximation
                double a1 = 0.254829592;
                double a2 = -0.284496736;
                double a3 = 1.421413741;
                double a4 = -1.453152027;
                double a5 = 1.061405429;
                double p = 0.3275911;

                // A&S formula 7.1.26
                double t = 1.0 / (1.0 + p * Math.Abs(x));
                double y = 1.0 - (((((a5 * t + a4) * t) + a3) * t + a2) * t + a1) * t * Math.Exp(-x * x);
                return Constants.INVERSE_SQRT_2 * Sign(x) * y;
            }
            else
                return Sign(x);
        }
    }
}
