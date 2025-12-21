using Models.Models.Enums;
using Models.Models.Objects.CorporateActions;
using Models.Models.Requests.Genericity;

namespace Models.Models.Requests.Dividends
{
    public class YahooFinanceDividendRequest : YahooFinanceCorporateActionsRequest<YahooFinanceDividend>
    {
        #region YahooFinanceCorporateActionsRequest Properties
        public override CorporateActionType CorporateActionType => CorporateActionType.Dividend;
        #endregion YahooFinanceCorporateActionsRequest Properties
    }
}
