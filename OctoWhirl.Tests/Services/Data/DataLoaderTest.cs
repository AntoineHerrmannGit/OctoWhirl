using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OctoWhirl.Core.Extensions;
using OctoWhirl.Core.Models.Enums;
using OctoWhirl.Core.Models.Technicals;
using OctoWhirl.Services.Data;

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

            var candles = await dataLoader.GetStocks("AAPL", DateTime.Now.AddDays(-5), DateTime.Now, ClientSource.YahooFinance, ResolutionInterval.Day).ConfigureAwait(false);
            Assert.IsNotNull(candles);
            Assert.IsTrue(candles.IsNotEmpty());
        }
    }
}
