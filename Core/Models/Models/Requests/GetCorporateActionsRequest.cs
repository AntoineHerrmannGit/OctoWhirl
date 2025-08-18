using OctoWhirl.Core.Models.Models.Common;

namespace OctoWhirl.Core.Models.Models.Requests
{
    public class GetCorporateActionsRequest
    {
        public List<string> Tickers { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public CorporateActionType? CorporateActionType { get; set; }
    }
}
