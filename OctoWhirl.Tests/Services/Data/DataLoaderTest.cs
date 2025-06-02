using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OctoWhirl.Core.Extensions;
using OctoWhirl.Core.Models.Enums;
using OctoWhirl.Core.Models.Technicals;
using OctoWhirl.Core.Tools.FileManager;
using OctoWhirl.Services.Data.Clients.YahooFinanceClient;
using OctoWhirl.Services.Data.Loaders;
using OctoWhirl.Services.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            // Build Dependency-Injection
            _provider = services.BuildServiceProvider();
        }

        [TestMethod]
        public async Task TestLoadDataFromDataBase()
        {
            var dataLoader = _provider.GetRequiredService<DataBaseLoader>();

            var request = new GetStocksRequest
            {
                Tickers = new List<string> { "AAPL" },
                StartDate = DateTime.Now.AddDays(-5),
                EndDate = DateTime.Now,
                Source = ClientSource.YahooFinance,
                Interval = ResolutionInterval.Day,
            };

            var candles = await dataLoader.GetStocks(request).ConfigureAwait(false);
            Assert.IsNotNull(candles);
            Assert.IsTrue(candles.IsNotEmpty());
        }
    }
}
