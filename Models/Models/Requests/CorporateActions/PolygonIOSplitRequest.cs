using Models.Models.Enums;
using Models.Models.Objects.CorporateActions;
using Models.Models.Requests.Genericity;

namespace Models.Models.Requests.CorporateActions
{
    public class PolygonIOSplitRequest : PolygonIOCorporateActionRequest<PolygonIOSplit>
    {
        public CorporateActionType CorporateActionType => CorporateActionType.Split;
    }
}
