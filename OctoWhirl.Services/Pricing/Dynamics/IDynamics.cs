using OctoWhirl.Core.Models.Technicals;

namespace OctoWhirl.Services.Pricing.Dynamics
{
    public interface IDynamics
    {
        Task<TimeSerie<double>> GeneratePath();
        void Reset();
    }
}
