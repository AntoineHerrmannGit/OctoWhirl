namespace OctoWhirl.Core.Models.Models.Technicals.Volatility
{
    public interface IVolatilitySurface
    {
        string Reference { get; set; }
        DateTime TimeStamp { get; set; }
        string Currency { get; set; }
        List<VolatilityPoint> Surface { get; set; }

        double GetVolatility(DateTime maturity, double strike);
    }
}
