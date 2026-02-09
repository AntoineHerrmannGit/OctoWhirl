using Batches.Generic.Enums;
using Batches.Generic.Tracking;

namespace Batches.Generic.Reporting
{
    public class Report
    {
        public string Name { get; set; }
        public BatchState State { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public TimeSpan Duration => EndTime - StartTime;
        public Exception? Exception { get; set; }
        public string Message { get; set; }
        public List<TraceElement> Trace { get; set; } = new List<TraceElement>();
        public List<Report> InnerReports { get; set; } = new List<Report>();
    }
}
