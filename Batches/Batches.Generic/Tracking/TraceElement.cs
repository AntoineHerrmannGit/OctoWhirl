namespace Batches.Generic.Tracking
{
    public class TraceElement
    {
        public DateTime Timestamp { get; set; }
        public string BatchName { get; set; }
        public string Message { get; set; }
        public string File { get; set; }
        public string Method { get; set; }
        public int Line { get; set; }

        public string ToString()
            => $"[{Timestamp}] | [{BatchName}] | [{File}] - [{Method}] : line {Line} : {Message}";
    }
}
