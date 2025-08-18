using OctoWhirl.Core.Models.Models.Common;

namespace OctoWhirl.TechnicalServices.DataService.PolygonIO
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
