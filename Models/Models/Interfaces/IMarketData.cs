namespace Models.Models.Interfaces
{
    public interface IMarketData
    {
        string Instrument { get; set; }
        DateTime? Timestamp { get; set; }
    }
}
