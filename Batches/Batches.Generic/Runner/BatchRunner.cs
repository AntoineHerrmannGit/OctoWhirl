using Batches.Generic.Configuration;
using Batches.Generic.Enums;
using Batches.Generic.Factory;
using Batches.Generic.Interfaces;
using Batches.Generic.Reporting;
using Batches.Generic.Tracking;
using Core.Technicals.Extensions;
using OctoWhirl.Core.Tools.Technicals.FileManagement;

namespace Batches.Generic.Runner
{
    internal class BatchRunner : IBatchRunner
    {
        private const string Name = "Runner";

        private readonly IBatchFactory Factory;
        private readonly Tracker Tracker;

        private List<IBatch> Batches;
        private RunnerConfiguration? Configuration;
        private Report Report;
        private BatchState State;

        #region Ctor
        public BatchRunner(IBatchFactory factory, Tracker tracker)
        {
            Factory = factory;
            Tracker = tracker;
            Batches = new List<IBatch>();
            Report = new Report();
            State = BatchState.NotStarted;
            Configuration = null;
        }
        #endregion Ctor

        #region IBatchRunner Methods
        public async Task<Report> Run(string configuration, CancellationToken token = default)
        {
            State = BatchState.Started;
            try
            {
                await Initialize(configuration, token).ConfigureAwait(false);

                Report.Trace.Add(Tracker.Track(Name, "Started Run..."));
                State = BatchState.Running;
                Report.State = State;

                var tasks = Batches.Select(async batch =>
                {
                    await batch.Initialize(token).ConfigureAwait(false);
                    await batch.Run(token).ConfigureAwait(false);
                    await batch.Terminate(token).ConfigureAwait(false);
                });

                await Task.WhenAll(tasks);
                Report.Trace.Add(Tracker.Track(Name, "Run Completed !"));

                await Terminate(token).ConfigureAwait(false);
                SetState(BatchState.Succeeded);
            }
            catch (Exception ex)
            {
                Report.Exception = ex;
                SetState(BatchState.Failed);
            }

            return await GenerateReport(token).ConfigureAwait(false);
        }
        #endregion IBatchRunner Methods

        #region Private Methods
        private Task Initialize(string configuration, CancellationToken token = default)
        {
            Report.Trace.Add(Tracker.Track(Name, "Initializing Runner..."));
            LoadConfiguration(configuration);
            CreateChildBatches();
            SetState(BatchState.Initialized);

            Report.Trace.Add(Tracker.Track(Name, "Runner Initialized !"));
            return Task.CompletedTask;
        }

        private void LoadConfiguration(string configPath)
            => Configuration = File.ReadAllText(FileManager.FindFilePath(configPath)).Deserialize<RunnerConfiguration>();

        private void CreateChildBatches()
        {
            Batches = Configuration!.Batches?.Select(batch =>
            {
                var childBatch = Factory.Create(batch.Key);
                childBatch.SetConfiguration(batch.Value);
                return childBatch;
            }).ToList() ?? new List<IBatch>();
        }

        private Task Terminate(CancellationToken token = default)
        {
            Report.Trace.Add(Tracker.Track(Name, "Terminating Runner..."));
            SetState(BatchState.Terminated);
            Report.Trace.Add(Tracker.Track(Name, "Runner Terminated !"));
            return Task.CompletedTask;
        }

        private async Task<Report> GenerateReport(CancellationToken token = default)
        {
            Report.Trace.Add(Tracker.Track(Name, "Generating Report..."));

            var childReports = await Task.WhenAll(Batches.Select(batch => batch.GenerateReport(token)));
            Report.InnerReports = childReports.ToList();

            var globalTrace = Report.Trace;
            globalTrace.AddRange(Report.InnerReports.SelectMany(report => report.Trace));
            Report.Trace = globalTrace.GroupBy(trace => trace.BatchName)
                                      .SelectMany(group => group.OrderBy(g => g.Timestamp))
                                      .ToList();

            Report.EndTime = DateTime.Now;
            Report.Trace.Add(Tracker.Track(Name, "Report Generated !"));
            return Report;
        }

        private void SetState(BatchState state)
        {
            Report.State = state;
            State = state;
        }
        #endregion Private Methods
    }
}
