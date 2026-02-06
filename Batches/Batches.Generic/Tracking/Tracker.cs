using System.Runtime.CompilerServices;

namespace Batches.Generic.Tracking
{
    internal static class Tracker
    {
        public static TraceElement Track(
            string batchName,
            string message,
            [CallerFilePath] string? file = null,
            [CallerLineNumber] int line = 0,
            [CallerMemberName] string? method = null
        )
            => new TraceElement
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
