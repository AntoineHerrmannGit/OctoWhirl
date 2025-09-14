using OctoWhirl.Core.Models.Models.Enums;

namespace OctoWhirl.Core.Models.Models.Common.Interfaces
{
    public interface IMarketData
    {
        string Instrument { get; set; }
        DataSource Source { get; set; }
        DateTime Timestamp { get; set; }
    }
}
