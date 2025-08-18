using OctoWhirl.Core.Models.Models.Technicals;

namespace OctoWhirl.Core.Pricing.Dynamics
{
    public interface IDynamics
    {
        Task<TimeSerie<double>> GeneratePath();
        void Reset();
    }
}
