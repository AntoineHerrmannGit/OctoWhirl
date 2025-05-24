using OctoWhirl.Core.Models.Technicals;

namespace OctoWhirl.Services.Strategy
{
    public interface IStrategy
    {
        #region Proprties
        List<string> Universe { get; set; }
        TimeSerie<double> Valuation { get; set; }
        #endregion Properties

        #region Methods
        Task Initialize(List<string> universe, DateTime startDate, DateTime endDate);

        Task Run();
        #endregion Methods
    }
}
