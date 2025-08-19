namespace OctoWhirl.Core.Models.Models.Technicals.Volatility
{
    public class VolatilityPoint
    {
        public DateTime Maturity { get; set; }
        public double Strike { get; set; }
        public double Value { get; set; }
    }
}
