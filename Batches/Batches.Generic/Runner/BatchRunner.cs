using Batches.Generic.Configuration;
using Batches.Generic.Enums;
using Batches.Generic.Factory;
using Batches.Generic.Interfaces;
using Batches.Generic.Reporting;
using Batches.Generic.Tracking;
using Core.Technicals.Extensions;
using Microsoft.Extensions.Logging;

namespace Batches.Generic.Runner
{
    internal class BatchRunner : IBatch
    {
        public string Name => "Runner";
        public BatchState State { get; set; }
        public Report Report { get; set; }

        private readonly ILogger<BatchRunner> _logger;
        private readonly IBatchFactory _batchFactory;

        private string _configPath;
        private RunnerConfiguration _configuration;

        private List<IBatch> _childBatches;


        public BatchRunner(ILogger<BatchRunner> logger, IBatchFactory batchFactory)
        {
            _logger = logger;
            _batchFactory = batchFactory;

            _childBatches = new List<IBatch>();

            _configPath = null;
            _configuration = null;

            State = BatchState.NotStarted;

            Report = new Report
            {
                StartTime = DateTime.Now,
                Name = Name,
                State = State,
                Trace = new List<TraceElement> { Tracker.Track(Name, "Runner Created") }
            };
        }

        #region Public Methods
        public void SetConfiguration(string configuration)
        {
            _configPath = configuration;
        }
        #endregion Public Methods

        #region IBatch Methods
        public Task Initialize(CancellationToken token = default)
        {
            Report.Trace.Add(Tracker.Track(Name, "Initializing Runner..."));
            LoadConfiguration();
            CreateChildBatches();

            State = BatchState.Initialized;
            Report.Trace.Add(Tracker.Track(Name, "Runner Initialized !"));
            return Task.CompletedTask;
        }

        public async Task Run(CancellationToken token = default)
        {
            Report.Trace.Add(Tracker.Track(Name, "Started Run..."));
            State = BatchState.Running;
            Report.State = State;

            var tasks = _childBatches.Select(async batch =>
            {
                await batch.Initialize(token).ConfigureAwait(false);
                await batch.Run(token).ConfigureAwait(false);
                await batch.Terminate(token).ConfigureAwait(false);
            });

            await Task.WhenAll(tasks);
            Report.Trace.Add(Tracker.Track(Name, "Run Completed !"));
        }

        public Task Terminate(CancellationToken token = default)
        {
            Report.Trace.Add(Tracker.Track(Name, "Terminating Runner..."));
            State = BatchState.Terminated;
            Report.Trace.Add(Tracker.Track(Name, "Runner Terminated !"));

            return Task.CompletedTask;
        }

        public async Task<Report> GenerateReport(CancellationToken token = default)
        {
            Report.Trace.Add(Tracker.Track(Name, "Generating Report..."));

            var childReports = await Task.WhenAll(_childBatches.Select(batch => batch.GenerateReport(token)));
            Report.InnerReports = childReports.ToList();

            var globalTrace = Report.Trace;
            globalTrace.AddRange(Report.InnerReports.SelectMany(report => report.Trace));
            Report.Trace = globalTrace.GroupBy(trace => trace.BatchName)
                                      .SelectMany(group => group.OrderBy(g => g.Timestamp))
                                      .ToList();

            Report.Trace.Add(Tracker.Track(Name, "Generating Report..."));
            State = BatchState.Succeeded;
            Report.State = State;

            Report.EndTime = DateTime.Now;

            Report.Trace.Add(Tracker.Track(Name, "Report Generated !"));
            return Report;
        }
        #endregion IBatch Methods


        #region Private Methods
        private void LoadConfiguration()
        {
            _configuration = File.ReadAllText(_configPath).Deserialize<RunnerConfiguration>();
        }

        private void CreateChildBatches()
        {
            _childBatches = _configuration!.ChildBatches?.Select(batch => 
            {
                var childBatch = _batchFactory.Create(batch.Key);
                childBatch.SetConfiguration(batch.Value);
                return childBatch;
            }).ToList() ?? new List<IBatch>();
        }
        #endregion Private Methods
    }
}
