namespace OctoWhirl.Core.Models.Models.Technicals.Rates
{
    public interface IRate
    {
        string Reference { get; set; }
        DateTime TimeStamp { get; set; }
        TimeSerie<double> Curve { get; set; }

        double GetRate(DateTime date);
    }
}
