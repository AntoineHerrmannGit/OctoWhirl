namespace Models.Models.Interfaces
{
    public interface ICorrelation : IMarketData
    {
        string InstrumentAgainst { get; set; }
        TimeSpan Maturity { get; set; }
    }
}
