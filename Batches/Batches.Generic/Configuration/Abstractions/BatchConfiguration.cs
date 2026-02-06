namespace Batches.Generic.Configuration.Abstractions
{
    internal abstract class BatchConfiguration<TParameters>
    {
        public TParameters Parameters { get; set; }
        public string Name { get; set; }
        public Dictionary<string, string>? ChildBatches { get; set; }
    }
}
