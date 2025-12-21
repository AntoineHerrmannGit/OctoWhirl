using Models.Models.Interfaces;

namespace Models.Models.Objects.Spots
{
    public class YahooFinanceSpot : MarketData, ISpot
    {
        #region ISpot Properties
        public double? Value { get; set; }
        #endregion ISpot Properties

        #region Specific Properties
        public double? Open { get; set; }
        public double? High { get; set; }
        public double? Low { get; set; }
        public double? Close { get; set; }
        public int? Volume { get; set; }
        #endregion Specific Properties
    }
}
