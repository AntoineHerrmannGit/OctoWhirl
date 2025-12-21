using Models.Models.Enums;
using Models.Models.Interfaces;

namespace Models.Models.Requests
{
    public interface IMarketDataRequest<TMarketData>
        where TMarketData : IMarketData
    {
        IEnumerable<string> Instruments { get; set; }
        DateTime StartDate { get; set; }
        DateTime EndDate { get; set; }
        DataSource Source { get; }
        MarketDataType MarketDataType { get; }
        bool SkipCache { get; set; }
    }
}
