using OctoWhirl.Core.Models.Enums;
using OctoWhirl.Core.Models.Technicals;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. 
// Reference might be null before assignment, e.g compute a correlation between 2 references that result in a new reference.

namespace OctoWhirl.Core.Models.Requests
{
    public class GetCandlesRequest
    {
        public List<string> References { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DataSource Source { get; set; }
        public ResolutionInterval Interval { get; set; }
    }
}
