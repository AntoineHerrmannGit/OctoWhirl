using Models.Models.Enums;

namespace PolygonIOProviders.Helpers
{
    internal static class PolygonIOCorporateActionTypeParser
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
