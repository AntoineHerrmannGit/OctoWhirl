using OctoWhirl.Core.Models.Models.Common;
using OctoWhirl.Core.Pricing.Models.Interfaces;

namespace OctoWhirl.Core.Pricing.Models
{
    public class BlackSholesPricingResponse
    {
        public IBlackSholesOption Option { get; set; }
        public List<Greek> Greeks { get; set; }
        public double Price { get; set; }
    }
}
