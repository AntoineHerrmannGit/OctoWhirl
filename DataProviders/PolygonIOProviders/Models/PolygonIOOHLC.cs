namespace PolygonIOProviders.Models
{
    internal class PolygonIOOHLC
    {
        public long t { get; set; }
        public double o { get; set; }
        public double h { get; set; }
        public double l { get; set; }
        public double c { get; set; }
        public int n { get; set; }
        public double v { get; set; }
        public double vw { get; set; }
        public bool otc { get; set; }
    }
}
