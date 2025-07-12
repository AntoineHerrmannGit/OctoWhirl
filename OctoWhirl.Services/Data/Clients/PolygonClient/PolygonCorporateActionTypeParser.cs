using OctoWhirl.Core.Models.Common;

namespace OctoWhirl.Services.Data.Clients.PolygonClient
{
    public static class PolygonCorporateActionTypeParser
    {
        public static string Parse(CorporateActionType actionType)
        {
            return actionType switch
            {
                CorporateActionType.Dividend => "dividends",
                CorporateActionType.Split => "splits",
                _ => throw new NotSupportedException(actionType.ToString())
            };
        }
    }
}
