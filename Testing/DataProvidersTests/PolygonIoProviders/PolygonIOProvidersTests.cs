using Core.Technicals.Extensions;
using DataProviders.GenericProvider.DataGetters;
using DataProviders.GenericProvider.Registration;
using Microsoft.Extensions.DependencyInjection;
using Models.Models.Enums;
using Models.Models.Requests.CorporateActions;
using Models.Models.Requests.Options;
using Models.Models.Requests.Spots;
using PolygonIOProviders.Registration;

namespace DataProvidersTests.PolygonIOProviders
{
    public class PolygonIOProvidersTests
    {
        private static IServiceProvider _provider;

        public PolygonIOProvidersTests()
        {
            _provider = new ServiceCollection()
                        .AddLogging()
                        .RegisterGenericServices()
                        .RegisterPolygonServices()
                        .BuildServiceProvider();
        }

        [Fact]
        public async Task GetOptionsTest()
        {
            var dataGetter = _provider.GetRequiredService<IDataGetter>();
            Assert.NotNull(dataGetter);

            var request = new PolygonIOOptionsRequest
            {
                Instruments = new[] { "SPY" },
                StartDate = new DateTime(2025, 11, 27),
                EndDate = new DateTime(2025, 11, 27),
                IncludeExpired = false,
            };

            var options = await dataGetter.Get(request);
            Assert.False(options.IsNullOrEmpty());
        }

        [Fact]
        public async Task GetSpotsTest()
        {
            var dataGetter = _provider.GetRequiredService<IDataGetter>();
            Assert.NotNull(dataGetter);

            var request = new PolygonIOSpotRequest
            {
                Instruments = new[] { "AAPL" },
                StartDate = new DateTime(2025, 11, 20),
                EndDate = new DateTime(2025, 11, 27),
                Resolution = ResolutionInterval.Day,
                AdjustSpots = false,
            };

            var spots = await dataGetter.Get(request);
            Assert.False(spots.IsNullOrEmpty());
            Assert.True(spots.None(spot => spot.Instrument.IsNullOrEmpty()));
            Assert.True(spots.None(spot => !spot.Value.HasValue));
        }

        [Fact]
        public async Task GetDividendsTest()
        {
            var dataGetter = _provider.GetRequiredService<IDataGetter>();
            Assert.NotNull(dataGetter);

            var request = new PolygonIODividendRequest
            {
                Instruments = new[] { "AAPL" },
                StartDate = new DateTime(2025, 1, 1),
                EndDate = new DateTime(2025, 12, 31),
            };

            var dividends = await dataGetter.Get(request);
            Assert.False(dividends.IsNullOrEmpty());
            Assert.True(dividends.None(dividend => dividend.Instrument.IsNullOrEmpty()));
            Assert.True(dividends.None(dividend => !dividend.Value.HasValue));
        }

        [Fact]
        public async Task GetSplitsTest()
        {
            var dataGetter = _provider.GetRequiredService<IDataGetter>();
            Assert.NotNull(dataGetter);

            var request = new PolygonIOSplitRequest
            {
                Instruments = new[] { "AAPL" },
                StartDate = new DateTime(2020, 1, 1),
                EndDate = new DateTime(2025, 12, 31),
            };

            var splits = await dataGetter.Get(request);
            Assert.False(splits.IsNullOrEmpty());
            Assert.True(splits.None(split => split.Instrument.IsNullOrEmpty()));
            Assert.True(splits.None(split => !split.SplitRatio.HasValue));
        }
    }
}
