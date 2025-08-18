using OctoWhirl.Core.Models.Exceptions;
using OctoWhirl.Core.Tools.Maths.Models;

namespace OctoWhirl.Core.Tools.Maths.Fitting
{
    public class SplineInterpolator : ISplineInterpolator
    {
        private readonly List<Point> _points;
        private readonly List<Area> _areas;

        public SplineInterpolator(List<Point> points)
        {
            if (points == null || points.Count < 2)
                throw new ArgumentException("At least two points are required for spline interpolation.", nameof(points));

            _points = points;
            _areas = new List<Area>();

            for (int i = 0; i < points.Count - 1; i++)
                _areas.Add(new Area
                {
                    Bounds = new[] { points[i], points[i + 1] }
                });

            ComputeSplineCoefficients();
        }

        public double Interpolate(double x)
        {
            var area = FindSegment(x);
            double dx = x - area.Bounds[0].Axis;

            return area.A * dx * dx * dx
                    + area.B * dx * dx
                    + area.C * dx
                    + area.D;
        }

        private Area FindSegment(double x)
        {
            var area = _areas.FirstOrDefault(a =>
                x >= a.Bounds[0].Axis && x <= a.Bounds[1].Axis);

            if (area == null)
                throw new OutOfBoundsException("Interpolation point is outside the spline domain.");

            return area;
        }

        private void ComputeSplineCoefficients()
        {
            if (_points == null || _areas == null)
                throw new InvalidOperationException("Points and areas must be initialized before computing spline coefficients.");

            int pointCount = _points.Count;
            int segmentCount = _areas.Count;

            // Step 1: Compute segment widths and alpha values
            double[] h = new double[segmentCount];
            double[] alpha = new double[segmentCount];

            for (int i = 0; i < segmentCount; i++)
            {
                h[i] = _points[i + 1].Axis - _points[i].Axis;
            }

            for (int i = 1; i < segmentCount; i++)
            {
                double slopeNext = (_points[i + 1].Value - _points[i].Value) / h[i];
                double slopePrev = (_points[i].Value - _points[i - 1].Value) / h[i - 1];
                alpha[i] = 3.0 * (slopeNext - slopePrev);
            }

            // Step 2: Solve tridiagonal system for c coefficients
            double[] l = new double[pointCount];
            double[] mu = new double[pointCount];
            double[] z = new double[pointCount];
            double[] c = new double[pointCount];
            double[] b = new double[segmentCount];
            double[] d = new double[segmentCount];

            // Natural spline boundary conditions
            l[0] = 1.0;
            mu[0] = 0.0;
            z[0] = 0.0;

            for (int i = 1; i < segmentCount; i++)
            {
                l[i] = 2.0 * (h[i] + h[i - 1]) - h[i - 1] * mu[i - 1];
                mu[i] = h[i] / l[i];
                z[i] = (alpha[i] - h[i - 1] * z[i - 1]) / l[i];
            }

            l[pointCount - 1] = 1.0;
            z[pointCount - 1] = 0.0;
            c[pointCount - 1] = 0.0;

            // Step 3: Back-substitution to compute b, c, d
            for (int j = segmentCount - 1; j >= 0; j--)
            {
                c[j] = z[j] - mu[j] * c[j + 1];
                double deltaY = _points[j + 1].Value - _points[j].Value;
                b[j] = deltaY / h[j] - h[j] * (c[j + 1] + 2.0 * c[j]) / 3.0;
                d[j] = (c[j + 1] - c[j]) / (3.0 * h[j]);
            }

            // Step 4: Store coefficients in Area objects
            for (int i = 0; i < segmentCount; i++)
            {
                _areas[i].A = d[i];                    // Cubic term
                _areas[i].B = c[i];                    // Quadratic term
                _areas[i].C = b[i];                    // Linear term
                _areas[i].D = _points[i].Value;        // Constant term
            }
        }

        #region Local Private Class
        private class Area
        {
            public Point[] Bounds { get; set; } = Array.Empty<Point>();
            public double A { get; set; }
            public double B { get; set; }
            public double C { get; set; }
            public double D { get; set; }
        }
        #endregion
    }
}
