using OctoWhirl.Core.Models.Models.Common.Interfaces;

namespace OctoWhirl.Core.Models.Models.Technicals.Volatility
{
    public interface IVolatilitySurface : IMarketData
    {
        string Currency { get; set; }
        List<VolatilityPoint> Surface { get; set; }

        double GetVolatility(DateTime maturity, double strike);
    }
}
