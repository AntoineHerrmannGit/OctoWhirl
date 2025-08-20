using OctoWhirl.Core.Models.Models.Common;
using OctoWhirl.Core.Pricing.Models.Interfaces;

namespace OctoWhirl.Core.Pricing.Models
{
    public class BlackSholesVanillaOption : IBlackSholesOption
    {
        #region IInstrument Properties
        public string Reference { get; set; }
        #endregion IInstrument Properties

        #region IOption Properties
        public OptionType OptionType { get; set; }
        public DateTime Maturity { get; set; }
        public double Strike { get; set; }
        public string Underlying { get; set; }
        #endregion IOption Properties

        #region IBlackSholesOption Properties
        public double Spot { get; set; }
        public DateTime TimeStamp { get; set; }
        public double Volatility { get; set; }
        public double Rate { get; set; }
        #endregion IBlackSholesOption Properties
    }
}
