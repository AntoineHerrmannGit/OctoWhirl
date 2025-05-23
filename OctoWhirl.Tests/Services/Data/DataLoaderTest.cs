﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OctoWhirl.Core.Extensions;
using OctoWhirl.Core.Models.Enums;
using OctoWhirl.Core.Models.Technicals;
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
                .AddJsonFile("appsettings.json", optional: true)
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

        [TestMethod]
        public async Task TestGetStocksFromYahooFinance()
        {
            var yahooClient = _provider.GetRequiredService<YahooFinanceClient>();

            var request = new GetStocksRequest
            {
                Tickers = new List<string> { "AAPL" },
                StartDate = DateTime.Now.AddDays(-5),
                EndDate = DateTime.Now,
                Source = ClientSource.YahooFinance,
                Interval = ResolutionInterval.Day,
            };

            var candles = await yahooClient.GetStocks(request).ConfigureAwait(false);
            Assert.IsNotNull(candles);
            Assert.IsTrue(candles.IsNotEmpty());
        }
    }
}
