using Models.Models.Interfaces;

namespace Models.Models.Objects.Spots
{
    public class PolygonIOSpot : MarketData, ISpot
    {
        public double? Value { get; set; }

        public double? Open { get; set; }
        public double? High { get; set; }
        public double? Low { get; set; }
        public double? Close { get; set; }
        public double? Volume { get; set; }
        public double? WeightedVolume { get; set; }
        public bool? OTC { get; set; }
    }
}
