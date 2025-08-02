namespace OctoWhirl.TechnicalServices.Strategy.Events
{
    public class PriceUpdateEvent : MarketEvent
    {
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }
        public string Currency { get; set; }
        public double Volume { get; set; }
    }
}
