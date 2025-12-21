using Core.Technicals.Extensions;
using DataProviders.GenericProvider.DataGetters;
using DataProviders.GenericProvider.Registration;
using Microsoft.Extensions.DependencyInjection;
using Models.Models.Enums;
using Models.Models.Requests.CorporateActions;
using Models.Models.Requests.Options;
using Models.Models.Requests.Spots;
using PolygonIOProviders.Registration;

namespace Tests.DataProviders.PolygonIOProviders
{
    [TestClass]
    public class PolygonIOProvidersTests
    {
        private static IServiceProvider _provider;

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            _provider = new ServiceCollection()
                        .AddLogging()
                        .RegisterGenericServices()
                        .RegisterPolygonServices()
                        .BuildServiceProvider();
        }

        [ClassCleanup]
        public static void Cleanup()
        {
        }

        [TestMethod]
        public async Task GetOptionsTest()
        {
            var dataGetter = _provider.GetRequiredService<IDataGetter>();
            Assert.IsNotNull(dataGetter);

            var request = new PolygonIOOptionsRequest
            {
                Instruments = new[] { "SPY" },
                StartDate = new DateTime(2025, 11, 27),
                EndDate = new DateTime(2025, 11, 27),
                IncludeExpired = false,
            };

            var options = await dataGetter.Get(request).ConfigureAwait(false);
            Assert.IsFalse(options.IsNullOrEmpty());
        }

        [TestMethod]
        public async Task GetSpotsTest()
        {
            var dataGetter = _provider.GetRequiredService<IDataGetter>();
            Assert.IsNotNull(dataGetter);

            var request = new PolygonIOSpotRequest
            {
                Instruments = new[] { "AAPL" },
                StartDate = new DateTime(2025, 11, 20),
                EndDate = new DateTime(2025, 11, 27),
                Resolution = ResolutionInterval.Day,
                AdjustSpots = false,
            };

            var spots = await dataGetter.Get(request).ConfigureAwait(false);
            Assert.IsFalse(spots.IsNullOrEmpty());
            Assert.IsTrue(spots.None(spot => spot.Instrument.IsNullOrEmpty()));
            Assert.IsTrue(spots.None(spot => !spot.Value.HasValue));
        }

        [TestMethod]
        public async Task GetDividendsTest()
        {
            var dataGetter = _provider.GetRequiredService<IDataGetter>();
            Assert.IsNotNull(dataGetter);

            var request = new PolygonIODividendRequest
            {
                Instruments = new[] { "AAPL" },
                StartDate = new DateTime(2025, 1, 1),
                EndDate = new DateTime(2025, 12, 31),
            };

            var dividends = await dataGetter.Get(request).ConfigureAwait(false);
            Assert.IsFalse(dividends.IsNullOrEmpty());
            Assert.IsTrue(dividends.None(dividend => dividend.Instrument.IsNullOrEmpty()));
            Assert.IsTrue(dividends.None(dividend => !dividend.Value.HasValue));
        }

        [TestMethod]
        public async Task GetSplitsTest()
        {
            var dataGetter = _provider.GetRequiredService<IDataGetter>();
            Assert.IsNotNull(dataGetter);

            var request = new PolygonIOSplitRequest
            {
                Instruments = new[] { "AAPL" },
                StartDate = new DateTime(2020, 1, 1),
                EndDate = new DateTime(2025, 12, 31),
            };

            var splits = await dataGetter.Get(request).ConfigureAwait(false);
            Assert.IsFalse(splits.IsNullOrEmpty());
            Assert.IsTrue(splits.None(split => split.Instrument.IsNullOrEmpty()));
            Assert.IsTrue(splits.None(split => !split.SplitRatio.HasValue));
        }
    }
}
