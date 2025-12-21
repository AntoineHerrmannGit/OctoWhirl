using Models.Models.Enums;
using Models.Models.Objects.Spots;
using Models.Models.Requests.Genericity;

namespace Models.Models.Requests.Spots
{
    public class YahooFinanceSpotRequest : YahooFinanceRequest<YahooFinanceSpot>
    {
        #region IMarketDataRequest Properties
        public override MarketDataType MarketDataType => MarketDataType.Spot;
        #endregion IMarketDataRequest Properties

        #region Specific Properties
        public ResolutionInterval Resolution { get; set; }
        public bool AdjustClose { get; set; }
        #endregion Specific Properties
    }
}
