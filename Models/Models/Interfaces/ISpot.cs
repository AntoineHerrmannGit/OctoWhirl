namespace Models.Models.Interfaces
{
    public interface ISpot : IMarketData
    {
        double? Value { get; set; }
    }
}
