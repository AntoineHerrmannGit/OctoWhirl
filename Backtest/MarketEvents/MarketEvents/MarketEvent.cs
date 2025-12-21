namespace MarketEvents.MarketEvents
{
    public class MarketEvent : EventArgs
    {
        public DateTime TimeStamp { get; set; }
        public string Instrument { get; set; }
    }
}
