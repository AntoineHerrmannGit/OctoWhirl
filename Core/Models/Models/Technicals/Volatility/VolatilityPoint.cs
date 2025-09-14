using OctoWhirl.Core.Models.Models.Common.Interfaces;
using OctoWhirl.Core.Models.Models.Enums;

namespace OctoWhirl.Core.Models.Models.Technicals.Volatility
{
    public class VolatilityPoint : IMarketData
    {
        
        public DateTime Maturity { get; set; }
        public double Strike { get; set; }
        public double Value { get; set; }
        public string Instrument { get; set; }
        public DataSource Source { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
