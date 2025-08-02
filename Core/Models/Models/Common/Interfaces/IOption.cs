namespace OctoWhirl.Core.Models.Common.Interfaces
{
    public interface IOption : IInstrument
    {
        OptionType OptionType { get; set; }
        DateTime Maturity { get; set; }
        double Strike { get; set; }
        string Underlying { get; set; }
    }
}
