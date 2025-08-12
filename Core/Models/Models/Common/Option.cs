using OctoWhirl.Core.Models.Common.Interfaces;

namespace OctoWhirl.Core.Models.Common
{
    public class Option : IOption, IInstrument
    {
        #pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. 
// Reference might be null before assignment, e.g compute a correlation between 2 references that result in a new reference.
        public OptionType OptionType { get; set; }
        public DateTime Maturity { get; set; }
        public double Strike { get; set; }
        public string Reference { get; set; }
        public string Underlying { get; set; }
        public string OOCReference => $"O:{Underlying}{Maturity.ToString("yyMMdd")}{OptionType.ToString().First()}";
    }
}
