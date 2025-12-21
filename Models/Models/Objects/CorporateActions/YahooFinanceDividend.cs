using Models.Models.Enums;
using Models.Models.Interfaces;

namespace Models.Models.Objects.CorporateActions
{
    public class YahooFinanceDividend : MarketData, IDividend
    {
        #region ICorporateAction Properties
        public DateTime? ExecutionDate { get; set; }
        public double? Value { get; set; }
        #endregion ICorporateAction Properties

        #region IDividend Properties
        public CorporateActionType CorporateActionType => CorporateActionType.Dividend;
        #endregion IDividend Properties
    }
}
