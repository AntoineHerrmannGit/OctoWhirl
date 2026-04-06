namespace Models.Models.Interfaces
{
    public interface IVolatility : IMarketData
    {
        DateTime Maturity { get; set; }
        double Strike { get; set; }
        double? Volatility { get; set; }
    }
}
