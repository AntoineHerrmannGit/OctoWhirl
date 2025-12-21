using MathModels.Interfaces;

namespace MathModels.Objects.Points
{
    public class Point2D : IPoint
    {
        public double Value => Z;
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
    }
}
