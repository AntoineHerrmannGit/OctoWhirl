using DataProviders.GenericProvider.DataGetters;
using DataProviders.GenericProvider.Registration;
using DataProviders.YahooFinanceProviders.Registration;
using Microsoft.Extensions.DependencyInjection;
using Models.Models.Enums;
using Models.Models.Requests.CorporateActions;
using Models.Models.Requests.Dividends;
using Models.Models.Requests.Spots;

namespace DataProvidersTests.YahooFinanceProviders;

public class YahooFinanceProvidersTests
{
    private static IServiceProvider _provider;


    public YahooFinanceProvidersTests()
    {
        _provider = new ServiceCollection()
                       .RegisterGenericServices()
                       .RegisterYahooServices()
                       .BuildServiceProvider();
    }

    [Fact]
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

        var result = await dataGetter.Get(request);
        Assert.NotNull(result);
        Assert.True(result.Any());
        Assert.True(result.All(s => s.Instrument is not null && s.Value is not null && s.Value != 0));
        Assert.True(result.Count() == result.Select(s => s.Timestamp).Distinct().Count());
    }

    [Fact]
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

        var result = await dataGetter.Get(request);
        Assert.NotNull(result);
        Assert.True(result.Any());
        Assert.True(result.All(s => s.Instrument is not null && s.Value is not null && s.Value != 0));
        Assert.True(result.Count() == result.Select(s => s.Timestamp).Distinct().Count());
    }

    [Fact]
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

        var result = await dataGetter.Get(request);
        Assert.NotNull(result);
        Assert.True(result.Any());
        Assert.True(result.All(s => s.Instrument is not null && s.Value is not null && s.Value != 0));
    }

    [Fact]
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

        var result = await dataGetter.Get(request);
        Assert.NotNull(result);
        Assert.True(result.Any());
        Assert.True(result.All(s => s.Instrument is not null && s.SplitRatio is not null && s.SplitRatio != 0));
    }
}
