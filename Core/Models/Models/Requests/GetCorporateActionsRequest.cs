using OctoWhirl.Core.Models.Common;

namespace OctoWhirl.Services.Models.Requests
{
    public class GetCorporateActionsRequest
    {
        public List<string> Tickers { get; set; } = new List<string>();
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public CorporateActionType? CorporateActionType { get; set; }
    }
}
