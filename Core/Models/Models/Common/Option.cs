using OctoWhirl.Core.Models.Models.Common.Interfaces;
using OctoWhirl.Core.Models.Models.Enums;

namespace OctoWhirl.Core.Models.Models.Common
{
    public class Option : IOption
    {
        public string Instrument { get; set; }
        public DataSource Source { get; set; }
        public DateTime Timestamp { get; set; }
        public OptionType OptionType { get; set; }
        public DateTime Maturity { get; set; }
        public double Strike { get; set; }
        public string Underlying { get; set; }
        public string OOCReference => $"O:{Underlying}{Maturity.ToString("yyMMdd")}{OptionType.ToString().First()}";
    }
}
