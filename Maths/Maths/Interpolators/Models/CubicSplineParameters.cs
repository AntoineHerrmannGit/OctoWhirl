namespace Maths.Interpolators.Models
{
    internal class CubicSplineParameters
    {
        // Describes the range of validity of the cubic polynomial
        public double XMin { get; set; }
        public double XMax { get; set; }

        // Parameters of the polynômial
        // P(x) = ax^3 + bx^2 + cx + d,    x C [XMin, XMax]
        public double A { get; set; }
        public double B { get; set; }
        public double C { get; set; }
        public double D { get; set; }
    }
}
