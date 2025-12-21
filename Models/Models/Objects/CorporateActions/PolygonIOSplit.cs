using Models.Models.Enums;
using Models.Models.Interfaces;

namespace Models.Models.Objects.CorporateActions
{
    public class PolygonIOSplit : MarketData, ISplit
    {
        #region ICorporateAction Properties
        public CorporateActionType CorporateActionType => CorporateActionType.Split;
        #endregion ICorporateAction Properties

        #region ISplit Properties
        public double? SplitRatio { get; set; }
        public string? OldInstrument { get; set; }
        public string? NewInstrument { get; set; }
        public string? TargetInstrument { get; set; }
        #endregion ISplit Properties
    }
}
