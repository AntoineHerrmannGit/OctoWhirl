using OctoWhirl.Core.Models.Models.Common;
using OctoWhirl.Core.Models.Models.Enums;

namespace OctoWhirl.Core.Models.Models.Requests
{
    public class GetCorporateActionsRequest : IMarketDataRequest
    {
        public IEnumerable<string> Instruments { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DataSource Source { get; set; }
        public CorporateActionType? CorporateActionType { get; set; }
    }
}
