using OctoWhirl.Core.Models.Common;

namespace OctoWhirl.Services.Models.Requests
{
    public class GetOptionRequest : GetStocksRequest
    {
        public double Strike { get; set; }
        public DateTime Maturity { get; set; }
        public OptionType OptionType { get; set; }
    }
}
