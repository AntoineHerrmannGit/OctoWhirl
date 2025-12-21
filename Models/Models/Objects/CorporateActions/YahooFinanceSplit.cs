using Models.Models.Enums;
using Models.Models.Interfaces;

namespace Models.Models.Objects.CorporateActions
{
    public class YahooFinanceSplit : MarketData, ISplit
    {
        #region ICorporateAction Properties
        public CorporateActionType CorporateActionType => CorporateActionType.Split;
        #endregion ICorporateAction Properties
        
        #region ISplit Properties
        public double? SplitRatio { get; set; }
        #endregion ISplit Properties
    }
}
