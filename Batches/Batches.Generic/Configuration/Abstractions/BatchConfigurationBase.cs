namespace Batches.Generic.Configuration.Abstractions
{
    public abstract class BatchConfigurationBase
    {
        public abstract string Name { get; }
        public Dictionary<string, string>? Batches { get; set; }
    }
}
