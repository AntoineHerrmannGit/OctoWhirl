namespace OctoWhirl.Core.Models.Common
{
    public class Option : IInstrument
    {
        public OptionType OptionType { get; set; }
        public DateTime Maturity { get; set; }
        public double Strike { get; set; }
        public string Reference { get; set; }
        public string Underlying { get; set; }
    }
}
