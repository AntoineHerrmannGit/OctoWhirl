using Models.Models.Enums;

namespace Models.Models.Interfaces
{
    public interface ICorporateAction : IMarketData
    {
        CorporateActionType CorporateActionType { get; }
    }
}
