namespace Batches.Generic.Configuration
{
    internal class RunnerConfiguration
    {
        public string Name => "Runner";
        public Dictionary<string, string>? Batches { get; set; }
    }
}
