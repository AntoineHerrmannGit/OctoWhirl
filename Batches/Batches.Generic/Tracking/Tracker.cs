using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

namespace Batches.Generic.Tracking
{
    public class Tracker
    {
        private readonly ILogger<Tracker> _logger;

        public Tracker(ILogger<Tracker> logger)
        {
            _logger = logger;
        }

        public TraceElement Track(
            string batchName,
            string message,
            [CallerFilePath] string? file = null,
            [CallerLineNumber] int line = 0,
            [CallerMemberName] string? method = null
        )
        {
            _logger.LogInformation(message);
            return new TraceElement
            {
                File = file,
                Message = message,
                Line = line,
                Method = method,
                Timestamp = DateTime.Now,
                BatchName = batchName,
            };
        }
    }
}
