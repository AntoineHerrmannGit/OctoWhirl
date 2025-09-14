using OctoWhirl.Core.Models.Models.Common.Interfaces;
using OctoWhirl.Core.Models.Models.Enums;

namespace OctoWhirl.Core.Models.Models.Common
{
    public class Candle : IMarketData
    {
        public string Instrument { get; set; }
        public DataSource Source { get; set; }
        public DateTime Timestamp { get; set; }
        public string Currency { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }
        public int Volume { get; set; }
    }
}