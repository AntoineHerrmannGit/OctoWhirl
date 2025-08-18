using OctoWhirl.Core.Models.Models.Common;

namespace OctoWhirl.TechnicalServices.DataService.YahooFinance
{
    public static class YahooFinanceCorporateActionTypeParser
    {
        public static string Parse(this CorporateActionType actionType)
        {
            return actionType switch
            {
                CorporateActionType.Dividend => "div",
                CorporateActionType.Split => "split",
                _ => throw new NotSupportedException(actionType.ToString())
            };
        }
    }
}
