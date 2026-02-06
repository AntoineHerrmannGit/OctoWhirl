using Batches.Generic.Enums;
using Batches.Generic.Tracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Batches.Generic.Reporting
{
    internal class Report
    {
        public string Name { get; set; }
        public BatchState State { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public TimeSpan Duration => EndTime - StartTime;
        public Exception? Exception { get; set; }
        public string Message { get; set; }
        public List<TraceElement> Trace { get; set; }
        public List<Report> InnerReports { get; set; }
    }
}
