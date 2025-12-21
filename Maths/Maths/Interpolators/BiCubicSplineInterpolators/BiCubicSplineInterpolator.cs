using MathModels.Enums;
using MathModels.Exceptions;
using MathModels.Objects.Points;
using Maths.Interpolators.SplineInterpolators;

namespace Maths.Interpolators.BiCubicSplineInterpolators
{
    public class BiCubicSplineInterpolator : ISplineInterpolator<Point2D>
    {
        private bool IsInitialized;
        private List<SplineInterpolator> XInterpolators;
        private List<SplineInterpolator> YInterpolators;
        private readonly SortedSet<double> XGrid;
        private readonly SortedSet<double> YGrid;

        private readonly List<Point2D> Points;
        private readonly SplineBoundaryConditions BoundaryConditions;

        #region Ctor
        public BiCubicSplineInterpolator(SplineBoundaryConditions boundaryConditions = SplineBoundaryConditions.Natural)
        {
            BoundaryConditions = boundaryConditions;
            IsInitialized = false;
        }

        public BiCubicSplineInterpolator(List<Point2D> points, SplineBoundaryConditions boundaryConditions = SplineBoundaryConditions.Natural)
            : this(boundaryConditions)
        {
            if (points is null)
                throw new ArgumentNullException(nameof(points));
            if (points.Count < 4)
                throw new NotEnoughPointsExceptions(4);

            Points = points;

            XGrid = new SortedSet<double>(Points.GroupBy(x => x.X)
                          .Select(x => x.Key)
                          .OrderBy(x => x));

            if (XGrid.Count < 2)
                throw new NotEnoughPointsExceptions(2, nameof(XGrid));

            YGrid = new SortedSet<double>(Points.GroupBy(x => x.Y)
                          .Select(x => x.Key)
                          .OrderBy(x => x));

            if (YGrid.Count < 2)
                throw new NotEnoughPointsExceptions(2, nameof(YGrid));
        }
        #endregion Ctor

        #region Public Accessors
        public Point2D this[double x, double y]
            => Evaluate(x, y);
        #endregion Public Accessors

        #region ISplineInterpolator Method
        public void Interpolate()
        {
            if (IsInitialized)
                return;

            XInterpolators = Points.GroupBy(x => x.X)
                                   .OrderBy(x => x.Key)
                                   .Select(group => new SplineInterpolator(group.Select(point => new Point1D
                                   {
                                       X = point.Y,
                                       Y = point.Z
                                   }).ToList()))
                                   .ToList();

            YInterpolators = Points.GroupBy(x => x.Y)
                                   .OrderBy(x => x.Key)
                                   .Select(group => new SplineInterpolator(group.Select(point => new Point1D
                                   {
                                       X = point.X,
                                       Y = point.Z
                                   }).ToList()))
                                   .ToList();

            IsInitialized = true;
        }
        #endregion ISplineInterpolator Method

        #region Private Methods
        private Point2D Evaluate(double x, double y)
        {
            if (!IsInitialized)
                Interpolate();

            var XSpline = new SplineInterpolator(XInterpolators.Select(interpolator => interpolator[y]).ToList(), BoundaryConditions);
            var YSpline = new SplineInterpolator(YInterpolators.Select(interpolator => interpolator[x]).ToList(), BoundaryConditions);

            var value = 0.5 * (XSpline[x].Value + YSpline[y].Value);
            return new Point2D
            {
                X = x,
                Y = y,
                Z = value
            };
        }
        #endregion Private Methods
    }
}
