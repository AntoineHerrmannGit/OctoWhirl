using Batches.Generic.Enums;
using Batches.Generic.Interfaces;
using Batches.Generic.Registration;
using BatchesTests.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace BatchesTests
{
    public class BatchTests
    {
        private readonly IServiceProvider _provider;

        public BatchTests()
        {
            _provider = new ServiceCollection()
                .RegisterBatchRunner()
                .RegisterBatch<SimpleBatch>("SimpleBatch")
                .RegisterBatch<LonelyBatch>("LonelyBatch")
                .AddLogging()
                .BuildServiceProvider();
        }

        [Fact]
        public async Task SimpleBatchTest()
        {
            var runner = _provider.GetRequiredService<IBatchRunner>();
            var report = await runner.Run("simple_run.json");
            Assert.NotNull(report);
            Assert.Equal(BatchState.Succeeded, report.State);
            Assert.True(report.InnerReports.All(r => r.State == BatchState.Succeeded));
        }

        [Fact]
        public async Task MultiBatchTest()
        {
            var runner = _provider.GetRequiredService<IBatchRunner>();
            var report = await runner.Run("multiple_run.json");
            Assert.NotNull(report);
            Assert.Equal(BatchState.Succeeded, report.State);
            Assert.True(report.InnerReports.All(r => r.State == BatchState.Succeeded));
        }
    }
}
