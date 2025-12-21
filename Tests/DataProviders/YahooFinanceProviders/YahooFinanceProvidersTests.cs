using DataProviders.GenericProvider.DataGetters;
using DataProviders.GenericProvider.Registration;
using DataProviders.YahooFinanceProviders.Registration;
using Microsoft.Extensions.DependencyInjection;
using Models.Models.Enums;
using Models.Models.Requests.CorporateActions;
using Models.Models.Requests.Dividends;
using Models.Models.Requests.Spots;

namespace Tests.DataProviders.YahooFinanceProviders;

[TestClass]
public class YahooFinanceProvidersTests
{
    private static IServiceProvider _provider;  

    [ClassInitialize]
    public static void Setup(TestContext context)
    {
        _provider = new ServiceCollection()
                       .RegisterGenericServices()
                       .RegisterYahooServices()
                       .BuildServiceProvider();
    }

    [ClassCleanup]
    public static void Cleanup()
    {
    }

    [TestMethod]
    public async Task GetSpotsTest()
    {
        var dataGetter = _provider.GetRequiredService<IDataGetter>();

        var request = new YahooFinanceSpotRequest
        {
            Instruments = new List<string> { "TTE", "^FCHI" },
            StartDate = new DateTime(2025, 1, 1),
            EndDate = new DateTime(2025, 1, 31),
            Resolution = ResolutionInterval.Day,
        };

        var result = await dataGetter.Get(request).ConfigureAwait(false);
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Any());
        Assert.IsTrue(result.All(s => s.Instrument is not null && s.Value is not null && s.Value != 0));
        Assert.IsTrue(result.Count() == result.Select(s => s.Timestamp).Distinct().Count());
    }

    [TestMethod]
    public async Task GetAdjustedSpotsTest()
    {
        var dataGetter = _provider.GetRequiredService<IDataGetter>();

        var request = new YahooFinanceSpotRequest
        {
            Instruments = new List<string> { "TTE", "^FCHI" },
            StartDate = new DateTime(2025, 1, 1),
            EndDate = new DateTime(2025, 1, 31),
            Resolution = ResolutionInterval.Day,
            AdjustClose = true,
        };

        var result = await dataGetter.Get(request).ConfigureAwait(false);
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Any());
        Assert.IsTrue(result.All(s => s.Instrument is not null && s.Value is not null && s.Value != 0));
        Assert.IsTrue(result.Count() == result.Select(s => s.Timestamp).Distinct().Count());
    }

    [TestMethod]
    public async Task GetDividendsTest()
    {
        var dataGetter = _provider.GetRequiredService<IDataGetter>();

        var request = new YahooFinanceDividendRequest
        {
            Instruments = new List<string> { "TTE", "^FCHI" },
            StartDate = new DateTime(2025, 1, 1),
            EndDate = new DateTime(2025, 1, 31),
            Resolution = ResolutionInterval.Day,
        };

        var result = await dataGetter.Get(request).ConfigureAwait(false);
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Any());
        Assert.IsTrue(result.All(s => s.Instrument is not null && s.Value is not null && s.Value != 0));
    }

    [TestMethod]
    public async Task GetSplitsTest()
    {
        var dataGetter = _provider.GetRequiredService<IDataGetter>();

        var request = new YahooFinanceSplitRequest
        {
            Instruments = new List<string> { "AAPL" },
            StartDate = new DateTime(2020, 8, 1),
            EndDate = new DateTime(2020, 9, 30),
            Resolution = ResolutionInterval.Day,
        };

        var result = await dataGetter.Get(request).ConfigureAwait(false);
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Any());
        Assert.IsTrue(result.All(s => s.Instrument is not null && s.SplitRatio is not null && s.SplitRatio != 0));
    }
}
