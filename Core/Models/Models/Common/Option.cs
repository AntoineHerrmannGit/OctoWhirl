using OctoWhirl.Core.Models.Common.Interfaces;

namespace OctoWhirl.Core.Models.Common
{
    public class Option : IOption, IInstrument
    {
        public OptionType OptionType { get; set; }
        public DateTime Maturity { get; set; }
        public double Strike { get; set; }
        public string Reference { get; set; } = string.Empty;
        public string Underlying { get; set; } = string.Empty;
        public string OOCReference => $"O:{Underlying}{Maturity.ToString("yyMMdd")}{OptionType.ToString().First()}";
    }
}
