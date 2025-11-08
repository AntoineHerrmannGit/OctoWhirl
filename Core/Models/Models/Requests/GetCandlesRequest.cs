using OctoWhirl.Core.Models.Models.Enums;
using OctoWhirl.Core.Models.Models.Technicals;

namespace OctoWhirl.Core.Models.Models.Requests
{
    public class GetCandlesRequest : IMarketDataRequest
    {
        public IEnumerable<string> Instruments { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DataSource Source { get; set; }
        public ResolutionInterval Interval { get; set; }
    }
}
