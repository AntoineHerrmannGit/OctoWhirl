using Batches.Generic.Configuration.Abstractions;

namespace BatchesTests.Configurations
{
    internal class SimpleBatchConfiguration : BatchConfigurationBase
    {
        public override string Name => "SimpleBatchConfiguration";
        public string Message => "All good on SimpleBatchConfiguration.";
    }
}
