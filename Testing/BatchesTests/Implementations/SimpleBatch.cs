using Batches.Generic.Abstractions;
using Batches.Generic.Enums;
using Batches.Generic.Factory;
using Batches.Generic.Reporting;
using Batches.Generic.Tracking;
using BatchesTests.Configurations;

namespace BatchesTests.Implementations
{
    internal class SimpleBatch : BatchBase<SimpleBatchConfiguration>
    {
        public override string Name => "SimpleBatch";

        public SimpleBatch(Tracker tracker, IBatchFactory factory)
            : base(tracker, factory)
        {
            Report.StartTime = DateTime.Now;
        }

        public override Task Run(CancellationToken token = default)
        {
            SetState(BatchState.Running);
            Report.Trace.Add(Tracker.Track(Name, "Running"));
            Report.Trace.Add(Tracker.Track(Name, $"Message from SimpleBatch : {Configuration.Message} !"));
            return RunSubBatches(token);
        }

        public override async Task Terminate(CancellationToken token = default)
        {
            await base.Terminate(token).ConfigureAwait(false);
            Report.Trace.Add(Tracker.Track(Name, "Terminated"));
            SetState(BatchState.Terminated);
        }

        public override async Task<Report> GenerateReport(CancellationToken token = default)
        {
            Report = await base.GenerateReport(token).ConfigureAwait(false);
            SetState(BatchState.Succeeded);
            return Report;
        }
    }
}
