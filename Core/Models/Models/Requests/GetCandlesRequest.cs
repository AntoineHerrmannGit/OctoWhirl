using OctoWhirl.Core.Models.Enums;
using OctoWhirl.Core.Models.Technicals;

namespace OctoWhirl.Core.Models.Requests
{
    public class GetCandlesRequest
    {
        public List<string> References { get; set; } = new List<string>();
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DataSource Source { get; set; }
        public ResolutionInterval Interval { get; set; }
    }
}
