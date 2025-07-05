namespace OctoWhirl.Services.Strategy.Events
{
    public class MarketEvent : EventArgs
    {
        public DateTime TimeStamp { get; set; }
        public string Reference { get; set; }
    }
}
