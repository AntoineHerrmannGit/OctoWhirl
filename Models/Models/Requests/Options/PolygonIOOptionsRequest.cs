using Models.Models.Enums;
using Models.Models.Objects.Options;
using Models.Models.Requests.Genericity;

namespace Models.Models.Requests.Options
{
    public class PolygonIOOptionsRequest : GenericMarketDataRequest<PolygonIOOption>
    {
        #region GenericMarketDataRequest Properties
        public override DataSource Source => DataSource.PolygonIO;
        public override MarketDataType MarketDataType => MarketDataType.Option;
        #endregion GenericMarketDataRequest Properties

        #region Specific Properties
        public bool IncludeExpired { get; set; }
        #endregion Specific Properties
    }
}
