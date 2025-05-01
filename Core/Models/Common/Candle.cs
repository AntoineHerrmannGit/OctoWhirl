namespace OctoWhirl.Core.Models.Common
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