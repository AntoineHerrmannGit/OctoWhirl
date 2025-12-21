using Models.Models.Interfaces;

namespace Models.Models.Objects
{
    public abstract class MarketData : IMarketData
    {
        public string Instrument { get; set; }
        public DateTime? Timestamp { get; set; }
    }
}
