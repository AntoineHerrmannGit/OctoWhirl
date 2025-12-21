using Models.Models.Enums;
using Models.Models.Interfaces;

namespace Models.Models.Requests.Genericity
{
    public abstract class YahooFinanceCorporateActionsRequest<TMarketData> : YahooFinanceRequest<TMarketData>
        where TMarketData : ICorporateAction
    {
        #region IMarketDataRequest Properties
        public override MarketDataType MarketDataType => MarketDataType.CorporateAction;
        #endregion IMarketDataRequest Properties

        #region Specific Properties
        public ResolutionInterval Resolution { get; set; }
        public abstract CorporateActionType CorporateActionType { get; }
        #endregion Specific Properties
    }
}
