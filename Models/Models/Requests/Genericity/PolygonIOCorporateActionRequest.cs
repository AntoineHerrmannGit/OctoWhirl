using Models.Models.Enums;
using Models.Models.Interfaces;

namespace Models.Models.Requests.Genericity
{
    public abstract class PolygonIOCorporateActionRequest<TMarketData> : PolygonIORequest<TMarketData>
        where TMarketData : IMarketData
    {
        public override MarketDataType MarketDataType => MarketDataType.CorporateAction;
    }
}
