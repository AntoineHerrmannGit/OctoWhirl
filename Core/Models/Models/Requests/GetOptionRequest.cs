using OctoWhirl.Core.Models.Common;
using OctoWhirl.Core.Models.Requests;

namespace OctoWhirl.Services.Models.Requests
{
    public class GetOptionRequest : GetCandlesRequest
    {
        public double Strike { get; set; }
        public DateTime Maturity { get; set; }
        public OptionType OptionType { get; set; }
    }
}
