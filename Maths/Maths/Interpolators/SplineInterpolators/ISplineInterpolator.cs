using MathModels.Interfaces;

namespace Maths.Interpolators.SplineInterpolators
{
    public interface ISplineInterpolator<TPoint>
        where TPoint : IPoint
    {
        void Interpolate();
    }
}
