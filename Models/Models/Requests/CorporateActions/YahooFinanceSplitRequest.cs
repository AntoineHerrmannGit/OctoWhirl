using Models.Models.Enums;
using Models.Models.Objects.CorporateActions;
using Models.Models.Requests.Genericity;

namespace Models.Models.Requests.CorporateActions
{
    public class YahooFinanceSplitRequest : YahooFinanceCorporateActionsRequest<YahooFinanceSplit>
    {
        #region YahooFinanceCorporateActionsRequest Properties
        public override CorporateActionType CorporateActionType => CorporateActionType.Split;
        #endregion YahooFinanceCorporateActionsRequest Properties
    }
}
