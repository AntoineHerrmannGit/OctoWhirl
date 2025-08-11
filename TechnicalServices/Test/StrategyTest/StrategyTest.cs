using Microsoft.Extensions.DependencyInjection;
using OctoWhirl.TechnicalServices.Strategy.EventProviders;
using OctoWhirl.TechnicalServices.Strategy.Simulator;
using Strategy.Test.Mocks;

namespace Strategy.Test
{
    [TestClass]
    public sealed class StrategyTest
    {
        private IServiceProvider _provider = null!;

        [TestInitialize]
        public void Setup()
        {
            var services = new ServiceCollection();

            // Register Services
            services.AddTransient<ITimeLineSimulator, TimeLineSimulator>();
            services.AddTransient<DummyStrategy>();
            services.AddTransient<IMarketEventProvider, MarketEventProviderMock>();

            // Build Dependency-Injection
            _provider = services.BuildServiceProvider();
        }

        [TestMethod]
        public async Task TestStrategy()
        {
            var strategy = _provider.GetRequiredService<DummyStrategy>();
            await strategy.Init().ConfigureAwait(false);

            var runner = _provider.GetRequiredService<ITimeLineSimulator>();
            await runner.Init().ConfigureAwait(false);

            await strategy.Attach(runner).ConfigureAwait(false);
            await runner.RegisterStrategy(strategy).ConfigureAwait(false);

            await runner.Run().ConfigureAwait(false);
            await runner.Close().ConfigureAwait(false);

            Assert.IsTrue(strategy.EventsOccured == 10);
        }
    }
}
