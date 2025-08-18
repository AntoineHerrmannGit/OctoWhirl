using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OctoWhirl.Core.Models.Models.Common;
using OctoWhirl.Core.Models.Models.Requests;
using OctoWhirl.Core.Models.Models.Technicals;
using OctoWhirl.Core.Tools.Technicals.Extensions;
using OctoWhirl.TechnicalServices.DataService.LocalDataBase;
using OctoWhirl.TechnicalServices.DataService.PolygonIO;
using OctoWhirl.TechnicalServices.DataService.YahooFinance;

namespace OctoWhirl.Test.Data
{
    [TestClass]
    public sealed class DataTest
    {
        private IServiceProvider _provider;

        [TestInitialize]
        public void Setup()
        {
            // Test Configuration
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("service.config.json", optional: false, reloadOnChange: true)
                .Build();

            var services = new ServiceCollection();

            // Register Services
            services.AddSingleton<IConfiguration>(config);
            services.AddTransient<DataBaseLoader>();
            services.AddHttpClient<PolygonClient>();
            services.AddHttpClient<YahooFinanceClient>();

            // Build Dependency-Injection
            _provider = services.BuildServiceProvider();
        }

        #region YahooFinance client tests
        [TestMethod]
        public async Task TestGetStocksFromYahooFinance()
        {
            var yahooClient = _provider.GetRequiredService<YahooFinanceClient>();

            var request = new GetCandlesRequest
            {
                References = new List<string> { "AAPL" },
                StartDate = DateTime.Now.AddDays(-5),
                EndDate = DateTime.Now,
                Interval = ResolutionInterval.Day,
            };

            var candles = await yahooClient.GetCandles(request).ConfigureAwait(false);
            Assert.IsNotNull(candles);
            Assert.IsTrue(candles.IsNotEmpty());
        }

        [TestMethod]
        public async Task TestGetSplitsFromYahooFinance()
        {
            var yahooClient = _provider.GetRequiredService<YahooFinanceClient>();

            var request = new GetCorporateActionsRequest
            {
                Tickers = new List<string> { "AAPL" },
                StartDate = DateTime.Now.AddMonths(-6),
                EndDate = DateTime.Now,
                CorporateActionType = CorporateActionType.Split,
            };

            var corporateActions = await yahooClient.GetSplits(request).ConfigureAwait(false);
            Assert.IsNotNull(corporateActions);
        }

        [TestMethod]
        public async Task TestGetDividendsFromYahooFinance()
        {
            var yahooClient = _provider.GetRequiredService<YahooFinanceClient>();

            var request = new GetCorporateActionsRequest
            {
                Tickers = new List<string> { "AAPL" },
                StartDate = DateTime.Now.AddMonths(-6),
                EndDate = DateTime.Now,
                CorporateActionType = CorporateActionType.Dividend,
            };

            var dividends = await yahooClient.GetDividends(request).ConfigureAwait(false);
            Assert.IsNotNull(dividends);
            Assert.IsTrue(dividends.IsNotEmpty());
            Assert.IsTrue(dividends.All(div => div.DividendAmount != 0));
        }
        #endregion YahooFinance client tests

        #region PolygonIO client tests
        [TestMethod]
        public async Task TestGetStocksFromPolygonIO()
        {
            var polygonClient = _provider.GetRequiredService<PolygonClient>();

            var request = new GetCandlesRequest
            {
                References = new List<string> { "AAPL" },
                StartDate = DateTime.Now.AddDays(-5),
                EndDate = DateTime.Now,
                Interval = ResolutionInterval.Day,
            };

            var candles = await polygonClient.GetCandles(request).ConfigureAwait(false);
            Assert.IsNotNull(candles);
            Assert.IsTrue(candles.IsNotEmpty());
        }


        [TestMethod]
        public async Task TestGetOptionsFromPolygonIO()
        {
            var polygonClient = _provider.GetRequiredService<PolygonClient>();

            var optionListRequest = new GetListedOptionRequest
            {
                References = new List<string> { "AAPL" },
                AsOfDate = new DateTime(2025, 06, 09)
            };
            var options = await polygonClient.GetListedOptions(optionListRequest).ConfigureAwait(false);
            var option = options.First();

            var optionRequest = new GetOptionRequest
            {
                References = new List<string> { option.Underlying },
                Maturity = option.Maturity,
                Strike = option.Strike,
                StartDate = new DateTime(2025, 05, 09),
                EndDate = new DateTime(2025, 05, 20),
                Interval = ResolutionInterval.Day,
                OptionType = option.OptionType
            };

            var optionSpot = await polygonClient.GetOption(optionRequest).ConfigureAwait(false);
            Assert.IsNotNull(options);
            Assert.IsTrue(options.IsNotEmpty());
        }


        [TestMethod]
        public async Task TestGetListedOptionsFromPolygonIO()
        {
            var polygonClient = _provider.GetRequiredService<PolygonClient>();

            var request = new GetListedOptionRequest
            {
                References = new List<string> { "AAPL" },
                AsOfDate = new DateTime(2025, 06, 09)
            };

            var options = await polygonClient.GetListedOptions(request).ConfigureAwait(false);
            Assert.IsNotNull(options);
            Assert.IsTrue(options.Count > 1000);
            Assert.IsTrue(options.IsNotEmpty());
        }

        [TestMethod]
        public async Task TestGetSplitsFromPolygon()
        {
            var polygonClient = _provider.GetRequiredService<PolygonClient>();

            var request = new GetCorporateActionsRequest
            {
                Tickers = new List<string> { "AAPL" },
                StartDate = DateTime.Now.AddMonths(-6),
                EndDate = DateTime.Now,
                CorporateActionType = CorporateActionType.Split,
            };

            var splits = await polygonClient.GetSplits(request).ConfigureAwait(false);
            Assert.IsNotNull(splits);
        }

        [TestMethod]
        public async Task TestGetDividendsFromPolygon()
        {
            var polygonClient = _provider.GetRequiredService<PolygonClient>();

            var request = new GetCorporateActionsRequest
            {
                Tickers = new List<string> { "AAPL" },
                StartDate = DateTime.Now.AddMonths(-6),
                EndDate = DateTime.Now,
                CorporateActionType = CorporateActionType.Dividend,
            };

            var dividends = await polygonClient.GetDividends(request).ConfigureAwait(false);
            Assert.IsNotNull(dividends);
            Assert.IsTrue(dividends.IsNotEmpty());
            Assert.IsTrue(dividends.All(div => div.DividendAmount != 0));
        }
        #endregion PolygonIO client tests
    }
}
