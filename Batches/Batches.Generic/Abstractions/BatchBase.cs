using Batches.Generic.Configuration.Abstractions;
using Batches.Generic.Enums;
using Batches.Generic.Factory;
using Batches.Generic.Interfaces;
using Batches.Generic.Reporting;
using Batches.Generic.Tracking;
using Core.Technicals.Extensions;
using OctoWhirl.Core.Tools.Technicals.FileManagement;

namespace Batches.Generic.Abstractions
{
    public abstract class BatchBase<TConfiguration> : IBatch
        where TConfiguration : BatchConfigurationBase
    {
        #region IBatch Properties
        public abstract string Name { get; }
        public BatchState State { get; set; }
        public Report Report { get; set; }
        #endregion IBatch Properties

        #region Protected Fields
        protected readonly Tracker Tracker;
        protected readonly IBatchFactory Factory;
        protected string ConfigPath;
        protected TConfiguration Configuration;
        protected List<IBatch> Batches;
        #endregion Protected Fields

        #region Ctor
        protected BatchBase(Tracker tracker, IBatchFactory factory)
        {
            Tracker = tracker;
            Factory = factory;
            Batches = new List<IBatch>();

            Report = new Report();
            ConfigPath = null;
            Configuration = null;

            State = BatchState.NotStarted;
            Report = new Report
            {
                StartTime = DateTime.Now,
                Name = Name,
                State = State,
                Trace = new List<TraceElement> { Tracker.Track(Name, $"Batch : \"{Name}\" Created.") }
            };
        }
        #endregion Ctor

        #region IBatch Methods
        public void SetConfiguration(string configPath)
            => ConfigPath = configPath;

        public virtual Task Initialize(CancellationToken token = default)
        {
            SetState(BatchState.Started);
            Report.Trace.Add(Tracker.Track(Name, "Initializing Batch..."));
            LoadConfiguration(ConfigPath!);
            CreateChildBatches();
            SetState(BatchState.Initialized);
            return Task.CompletedTask;
        }

        public virtual Task Terminate(CancellationToken token = default)
        {
            Report.Trace.Add(Tracker.Track(Name, "Terminating Batch..."));
            return Task.CompletedTask;
        }

        public virtual async Task<Report> GenerateReport(CancellationToken token = default)
        {
            try
            {
                Report.Trace.Add(Tracker.Track(Name, "Generating Report..."));

                var childReports = await Task.WhenAll(Batches.Select(batch => batch.GenerateReport(token)));
                Report.InnerReports = childReports.ToList();

                var globalTrace = Report.Trace;
                globalTrace.AddRange(Report.InnerReports.SelectMany(report => report.Trace));
                Report.Trace = globalTrace.GroupBy(trace => trace.BatchName)
                                          .SelectMany(group => group.OrderBy(g => g.Timestamp))
                                          .ToList();
                SetState(BatchState.Succeeded);
            }
            catch (Exception ex)
            {
                Report.Exception = ex;
                SetState(BatchState.Failed);
            }

            Report.EndTime = DateTime.Now;
            Report.Trace.Add(Tracker.Track(Name, "Report Generated !"));
            return Report;
        }
        #endregion IBatch Methods

        #region IBatch Abstractions
        public abstract Task Run(CancellationToken token = default);
        #endregion IBatch Abstractions

        #region Protected Virtual Methods
        protected virtual void LoadConfiguration(string configPath)
            => Configuration = File.ReadAllText(FileManager.FindFilePath(ConfigPath)).Deserialize<TConfiguration>();

        protected virtual void CreateChildBatches()
        {
            Batches = Configuration!.Batches?.Select(batch =>
            {
                var childBatch = Factory.Create(batch.Key);
                childBatch.SetConfiguration(batch.Value);
                return childBatch;
            }).ToList() ?? new List<IBatch>();
        }
        #endregion Protected Virtual Methods

        #region Protected Mathods
        protected void SetState(BatchState state)
        {
            Report.State = state;
            State = state;
        }

        protected Task RunSubBatches(CancellationToken token)
        {
            if (!Batches.IsNullOrEmpty())
            {
                return Task.WhenAll(Batches.Select(async batch =>
                {
                    await batch.Initialize(token).ConfigureAwait(false);
                    await batch.Run(token).ConfigureAwait(false);
                    await batch.Terminate(token).ConfigureAwait(false);
                }));
            }
            else
                return Task.CompletedTask;
        }
        #endregion Protected Mathods
    }
}
