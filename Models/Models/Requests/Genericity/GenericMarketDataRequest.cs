using Models.Models.Enums;
using Models.Models.Interfaces;

namespace Models.Models.Requests.Genericity
{
    public abstract class GenericMarketDataRequest<TMarketData> : IMarketDataRequest<TMarketData>
        where TMarketData : IMarketData
    {
        public IEnumerable<string> Instruments { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public abstract DataSource Source { get; }

        public abstract MarketDataType MarketDataType { get; }  

        public bool SkipCache { get; set; }
    }
}
