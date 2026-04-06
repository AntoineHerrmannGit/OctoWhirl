namespace Models.Models.Interfaces
{
    public interface IInstrument
    {
        string Instrument { get; set; }
        string Currency { get; set; }
        string RateCurve { get; set; }
    }
}
