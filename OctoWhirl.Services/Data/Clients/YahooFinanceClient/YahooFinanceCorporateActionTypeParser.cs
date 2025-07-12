using OctoWhirl.Core.Models.Common;

namespace OctoWhirl.Services.Data.Clients.YahooFinanceClient
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
