using Models.Models.Enums;
using Models.Models.Interfaces;

namespace Models.Models.Objects.Options
{
    public class PolygonIOOption : MarketData, IOption
    {
        public double RelativeStrike { get; set; }
        public double AbsoluteStrike { get; set; }
        public DateTime Expiration { get; set; }
        public OptionTypeEnum OptionType { get; set; }
        public string Underlying { get; set; }

        public string Ticker { get; set; }
        public string Exchange { get; set; }
    }
}
