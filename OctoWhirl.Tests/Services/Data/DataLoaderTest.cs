using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OctoWhirl.Core.Extensions;
using OctoWhirl.Core.Models.Common;
using OctoWhirl.Core.Models.Enums;
using OctoWhirl.Core.Models.Technicals;
using OctoWhirl.Core.Tools.FileManager;
using OctoWhirl.Services.Data.Clients.PolygonClient;
using OctoWhirl.Services.Data.Clients.YahooFinanceClient;
using OctoWhirl.Services.Data.Loaders;
using OctoWhirl.Services.Models.Requests;

namespace OctoWhirl.Tests.Services.Data
{
    [TestClass]
    public class DataLoaderTest
    {
        private ServiceProvider _provider;

        [TestInitialize]
        public void Setup()
        {
            // Test Configuration
            var config = new ConfigurationBuilder()
                .AddJsonFile(FileManager.FindFilePath("appsettings.app.json"), optional: true)
                .AddJsonFile(FileManager.FindFilePath("appsettings.gui.json"), optional: true)
                .AddJsonFile(FileManager.FindFilePath("appsettings.services.json"), optional: true)
                .Build();

            var services = new ServiceCollection();

            // Register Services
            services.AddSingleton<IConfiguration>(config);
            services.AddTransient<DataLoader>();
            services.AddTransient<DataBaseLoader>();
            services.AddHttpClient<PolygonClient>();
            services.AddHttpClient<YahooFinanceClient>();

            // Build Dependency-Injection
            _provider = services.BuildServiceProvider();
        }


        #region Database client tests
        [TestMethod]
        public async Task TestLoadDataFromDataBase()
        {
            var dataLoader = _provider.GetRequiredService<DataBaseLoader>();

            var request = new GetStocksRequest
            {
                Tickers = new List<string> { "AAPL" },
                StartDate = DateTime.Now.AddDays(-5),
                EndDate = DateTime.Now,
                Source = DataSource.YahooFinance,
                Interval = ResolutionInterval.Day,
            };

            var candles = await dataLoader.GetStocks(request).ConfigureAwait(false);
            Assert.IsNotNull(candles);
            Assert.IsTrue(candles.IsNotEmpty());
        }
        #endregion Database client tests


        #region YahooFinance client tests
        [TestMethod]
        public async Task TestGetStocksFromYahooFinance()
        {
            var yahooClient = _provider.GetRequiredService<YahooFinanceClient>();

            var request = new GetStocksRequest
            {
                Tickers = new List<string> { "AAPL" },
                StartDate = DateTime.Now.AddDays(-5),
                EndDate = DateTime.Now,
                Interval = ResolutionInterval.Day,
            };

            var candles = await yahooClient.GetStocks(request).ConfigureAwait(false);
            Assert.IsNotNull(candles);
            Assert.IsNotEmpty(candles);
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
            Assert.IsNotEmpty(dividends);
            Assert.IsTrue(dividends.All(div => div.DividendAmount != 0));
        }
        #endregion YahooFinance client tests


        #region PolygonIO client tests
        [TestMethod]
        public async Task TestGetStocksFromPolygonIO()
        {
            var polygonClient = _provider.GetRequiredService<PolygonClient>();

            var request = new GetStocksRequest
            {
                Tickers = new List<string> { "AAPL" },
                StartDate = DateTime.Now.AddDays(-5),
                EndDate = DateTime.Now,
                Interval = ResolutionInterval.Day,
            };

            var candles = await polygonClient.GetStocks(request).ConfigureAwait(false);
            Assert.IsNotNull(candles);
            Assert.IsTrue(candles.IsNotEmpty());
        }


        [TestMethod]
        public async Task TestGetOptionsFromPolygonIO()
        {
            var polygonClient = _provider.GetRequiredService<PolygonClient>();

            var optionListRequest = new GetListedOptionRequest
            {
                Tickers = new List<string> { "AAPL" },
                AsOfDate = new DateTime(2025, 06, 09)
            };
            var options = await polygonClient.GetListedOptions(optionListRequest).ConfigureAwait(false);
            var option = options.First();

            var optionRequest = new GetOptionRequest
            {
                Tickers = new List<string> { option.Underlying },
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
                Tickers = new List<string> { "AAPL" },
                AsOfDate = new DateTime(2025, 06, 09)
            };

            var options = await polygonClient.GetListedOptions(request).ConfigureAwait(false);
            Assert.IsNotNull(options);
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
            Assert.IsNotEmpty(dividends);
            Assert.IsTrue(dividends.All(div => div.DividendAmount != 0));
        }
        #endregion PolygonIO client tests
    }
}
