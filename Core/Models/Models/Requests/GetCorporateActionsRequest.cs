using OctoWhirl.Core.Models.Common;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. 

namespace OctoWhirl.Services.Models.Requests
{
    public class GetCorporateActionsRequest
    {
        public List<string> Tickers { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public CorporateActionType? CorporateActionType { get; set; }
    }
}
