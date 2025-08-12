namespace OctoWhirl.Core.Models.Common
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. 
// Reference and Currency might be null before assignment, e.g compute a correlation between 2 references that result in a new reference.
{
    public class Candle
    {
        public string Reference { get; set; }
        public string Currency { get; set; }
        public DateTime Timestamp { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }
        public int Volume { get; set; }
    }
}