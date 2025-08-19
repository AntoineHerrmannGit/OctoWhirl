using OctoWhirl.Core.Models.Models.Common.Interfaces;

namespace OctoWhirl.Core.Pricing.Models.Interfaces
{
    public interface IBlackSholesOption : IOption
    {
        public double Spot { get; set; }
        public DateTime TimeStamp { get; set; }
        public double Volatility { get; set; }
        public double Rate { get; set; }
    }
}
