using OctoWhirl.Core.Models.Models.Common.Interfaces;
using OctoWhirl.Core.Models.Models.Enums;

namespace OctoWhirl.Core.Models.Models.Common
{
    public abstract class CorporateAction : IMarketData
    {
        public abstract CorporateActionType ActionType { get; }
        public string Instrument { get; set; }
        public DataSource Source { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
