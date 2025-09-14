using OctoWhirl.Core.Models.Models.Common.Interfaces;

namespace OctoWhirl.Core.Models.Models.Technicals.Rates
{
    public interface IRate : IMarketData
    {
        string Currency { get; set; }
        TimeSerie<double> Curve { get; set; }

        double GetRate(DateTime date);
    }
}
