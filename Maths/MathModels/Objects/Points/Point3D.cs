using MathModels.Interfaces;

namespace MathModels.Objects.Points
{
    public class Point3D : IPoint
    {
        public double Value => T;
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public double T { get; set; }
    }
}
