using Batches.Generic.Enums;
using Batches.Generic.Reporting;

namespace Batches.Generic.Interfaces
{
    internal interface IBatch
    {
        string Name { get; }
        BatchState State { get; set; }
        Report Report { get; set; }

        void SetConfiguration(string configPath);
        Task Initialize(CancellationToken token = default);
        Task Run(CancellationToken token = default);
        Task Terminate(CancellationToken token = default);
        Task<Report> GenerateReport(CancellationToken token = default);
    }
}
