using Batches.Generic.Reporting;

namespace Batches.Generic.Interfaces
{
    public interface IBatchRunner
    {
        Task<Report> Run(string configuration, CancellationToken token = default);
    }
}
