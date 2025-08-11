using OctoWhirl.Core.Models.Common;
using OctoWhirl.Core.Models.Technicals;

namespace OctoWhirl.Models.Technicals
{
    public class CandleSerie : TimeSerie<Candle>
    {
        #region Properties
        public string Reference { get; set; } = string.Empty;
        #endregion Properties

        public CandleSerie() :
            base()
        {
        }

        public CandleSerie(string reference) :
            base()
        {
            Reference = reference;
        }

        public CandleSerie(string reference, IEnumerable<KeyValuePair<DateTime, Candle>> enumerable) :
            base(enumerable.ToDictionary())
        {
            Reference = reference;
        }

        public CandleSerie(string reference, IEnumerable<DateTime> dates, IEnumerable<Candle> values)
            : base(Build(dates, values))
        {
            Reference = reference;
        }
    }
}
