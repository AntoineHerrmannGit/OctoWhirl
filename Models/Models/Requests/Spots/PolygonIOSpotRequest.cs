using Models.Models.Enums;
using Models.Models.Objects.Spots;
using Models.Models.Requests.Genericity;

namespace Models.Models.Requests.Spots
{
    public class PolygonIOSpotRequest : GenericMarketDataRequest<PolygonIOSpot>
    {
        public override DataSource Source => DataSource.PolygonIO;
        public override MarketDataType MarketDataType => MarketDataType.Spot;

        public ResolutionInterval Resolution { get; set; }
        public bool AdjustSpots { get; set; }
    }
}
