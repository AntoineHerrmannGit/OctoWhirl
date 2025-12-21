using Models.Models.Enums;

namespace DataProviders.YahooFincnaceProviders.Core
{
    internal static class CorporateActionTypeHelper
    {
        public static string ToYahooString(this CorporateActionType type)
            => type switch
            {
                CorporateActionType.Dividend => "div",
                CorporateActionType.Split => "split",
                _ => throw new NotSupportedException(type.ToString())
            };
    }
}
