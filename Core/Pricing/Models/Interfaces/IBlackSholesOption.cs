using OctoWhirl.Core.Models.Models.Common.Interfaces;

namespace OctoWhirl.Core.Pricing.Models.Interfaces
{
    public interface IBlackSholesOption : IOption
    {
        double Spot { get; set; }
        DateTime TimeStamp { get; set; }
        double Volatility { get; set; }
        double Rate { get; set; }
    }
}
