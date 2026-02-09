using Batches.Generic.Configuration.Abstractions;

namespace BatchesTests.Configurations
{
    internal class LonelyBatchConfiguration : BatchConfigurationBase
    {
        public override string Name => "LonelyBatchConfiguration";
        public string Message => "Sooo lonely...";
    }
}
