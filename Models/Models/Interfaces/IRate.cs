namespace Models.Models.Interfaces
{
    public interface IRate : IMarketData
    {
        DateTime Maturity { get; set; }
        double? Rate { get; set; }
    }
}
