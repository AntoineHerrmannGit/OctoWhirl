using Models.Models.Enums;
using Models.Models.Interfaces;

namespace Models.Models.Requests.Genericity
{
    public abstract class PolygonIORequest<TMarketData> : GenericMarketDataRequest<TMarketData>
        where TMarketData : IMarketData
    {
        public override DataSource Source => DataSource.PolygonIO;
    }
}
