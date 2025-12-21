using MathModels.Interfaces;

namespace MathModels.Objects.Points
{
    public class Point1D : IPoint
    {
        public double Value => Y;
        public double X { get; set; }
        public double Y { get; set; }
    }
}
