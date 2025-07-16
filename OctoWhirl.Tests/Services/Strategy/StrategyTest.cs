using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OctoWhirl.Core.Tools.FileManager;
using OctoWhirl.Services.Data.Clients.PolygonClient;
using OctoWhirl.Services.Data.Clients.YahooFinanceClient;
using OctoWhirl.Services.Data.Loaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OctoWhirl.Tests.Services.Strategy
{
    [TestClass]
    public class StrategyTest
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

    }
}
