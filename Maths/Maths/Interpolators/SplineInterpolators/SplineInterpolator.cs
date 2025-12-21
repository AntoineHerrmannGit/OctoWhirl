using MathModels.Enums;
using MathModels.Exceptions;
using MathModels.Objects.Points;
using Maths.Interpolators.Models;

namespace Maths.Interpolators.SplineInterpolators
{
    public class SplineInterpolator : ISplineInterpolator<Point1D>
    {
        private bool IsInitialized;
        private List<CubicSplineParameters> LocalParameters;

        private List<Point1D> Points;
        private SplineBoundaryConditions BoundaryConditions;

        #region Ctor
        public SplineInterpolator(SplineBoundaryConditions boundaryConditions = SplineBoundaryConditions.Natural)
        {
            BoundaryConditions = boundaryConditions;
            IsInitialized = false;
        }

        public SplineInterpolator(List<Point1D> points, SplineBoundaryConditions boundaryConditions = SplineBoundaryConditions.Natural)
            : this(boundaryConditions)
        {
            if (points.Count < 2)
                throw new NotEnoughPointsExceptions(2);
            Points = points.OrderBy(x => x.X).ToList();
        }
        #endregion Ctor

        #region Public Accessors
        public double Value;
        public Point1D this[double x]
            => Evaluate(x);
        #endregion Public Accessors

        #region Public Methods
        public void Interpolate()
        {
            if (IsInitialized)
                return;

            int n = Points.Count;

            double[] xs = Points.Select(p => p.X).ToArray();
            double[] ys = Points.Select(p => p.Y).ToArray();

            double[] a = new double[n];
            double[] b = new double[n];
            double[] c = new double[n];
            double[] d = ys;

            double[] h = new double[n - 1];
            for (int i = 0; i < n - 1; i++)
                h[i] = xs[i + 1] - xs[i];

            double[] alpha = new double[n];
            for (int i = 1; i < n - 1; i++)
                alpha[i] = 3.0 / h[i] * (d[i + 1] - d[i]) - 3.0 / h[i - 1] * (d[i] - d[i - 1]);

            double[] l = new double[n];
            double[] mu = new double[n];
            double[] z = new double[n];

            if (BoundaryConditions == SplineBoundaryConditions.Natural)
            {
                l[0] = 1.0;
                mu[0] = 0.0;
                z[0] = 0.0;

                l[n - 1] = 1.0;
                z[n - 1] = 0.0;
                b[n - 1] = 0.0;
            }
            else
                throw new NotSupportedException(BoundaryConditions.ToString());

            for (int i = 1; i < n - 1; i++)
            {
                l[i] = 2.0 * (xs[i + 1] - xs[i - 1]) - h[i - 1] * mu[i - 1];
                mu[i] = h[i] / l[i];
                z[i] = (alpha[i] - h[i - 1] * z[i - 1]) / l[i];
            }

            for (int j = n - 2; j >= 0; j--)
            {
                b[j] = z[j] - mu[j] * b[j + 1];
                c[j] = (d[j + 1] - d[j]) / h[j] - h[j] * (b[j + 1] + 2.0 * b[j]) / 3.0;
                a[j] = (b[j + 1] - b[j]) / (3.0 * h[j]);
            }

            LocalParameters = new List<CubicSplineParameters>();
            for (int i = 0; i < n - 1; i++)
            {
                LocalParameters.Add(new CubicSplineParameters
                {
                    XMin = xs[i],
                    XMax = xs[i + 1],
                    A = a[i],
                    B = b[i],
                    C = c[i],
                    D = d[i]
                });
            }

            IsInitialized = true;
        }
        #endregion Public Methods

        #region Private Methods
        private Point1D Evaluate(double x)
        {
            if (!IsInitialized)
                Interpolate();

            var parameters = GetParameters(x);
            if (parameters is null)
                throw new IndexOutOfRangeException($"{x} is not interpolable.");

            var dx = x - parameters.XMin;
            var y = parameters.A * dx * dx * dx
                    + parameters.B * dx * dx
                    + parameters.C * dx
                    + parameters.D;

            return new Point1D
            {
                X = x,
                Y = y,
            };
        }

        private CubicSplineParameters? GetParameters(double x)
            => LocalParameters.FirstOrDefault(p => p.XMin <= x && x <= p.XMax);
        #endregion Private Methods
    }
}
