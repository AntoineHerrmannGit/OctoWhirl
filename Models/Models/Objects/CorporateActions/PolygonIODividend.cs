using Models.Models.Enums;
using Models.Models.Interfaces;

namespace Models.Models.Objects.CorporateActions
{
    public class PolygonIODividend : MarketData, IDividend
    {
        public DateTime? ExecutionDate { get; set; }
        public double? Value { get; set; }

        public CorporateActionType CorporateActionType => CorporateActionType.Dividend;

        public string Currency { get; set; }
    }
}
