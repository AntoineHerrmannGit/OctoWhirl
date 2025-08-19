using OctoWhirl.Core.Models.Models.Common.Interfaces;

namespace OctoWhirl.Core.Models.Models.Common
{
    public class Option : IOption
    {
        public OptionType OptionType { get; set; }
        public DateTime Maturity { get; set; }
        public double Strike { get; set; }
        public string Reference { get; set; }
        public string Underlying { get; set; }
        public string OOCReference => $"O:{Underlying}{Maturity.ToString("yyMMdd")}{OptionType.ToString().First()}";
    }
}
