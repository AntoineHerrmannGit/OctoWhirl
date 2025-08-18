using OctoWhirl.Core.Models.Models.Common;

namespace OctoWhirl.Core.Models.Models.Requests
{
    public class GetOptionRequest : GetCandlesRequest
    {
        public double Strike { get; set; }
        public DateTime Maturity { get; set; }
        public OptionType OptionType { get; set; }
    }
}
