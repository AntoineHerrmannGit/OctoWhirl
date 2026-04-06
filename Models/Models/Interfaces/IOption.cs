using Models.Models.Enums;

namespace Models.Models.Interfaces
{
    public interface IOption : IMarketData
    {
        double RelativeStrike { get; set; }
        double AbsoluteStrike { get; set; }
        DateTime Expiration { get; set; }
        OptionTypeEnum OptionType { get; set; }
        string Underlying { get; set; }
    }
}
