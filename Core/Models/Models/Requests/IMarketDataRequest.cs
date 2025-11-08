using OctoWhirl.Core.Models.Models.Enums;

namespace OctoWhirl.Core.Models.Models.Requests
{
    public interface IMarketDataRequest
    {
        IEnumerable<string> Instruments { get; set; }
        DateTime StartDate { get; set; }
        DateTime EndDate { get; set; }
        DataSource Source { get; set; }
    }
}
