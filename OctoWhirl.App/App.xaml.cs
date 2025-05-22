using System;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OctoWhirl.GUI.ViewModels;
using OctoWhirl.GUI.ViewModels.Technical;
using OctoWhirl.GUI.Views;
using OctoWhirl.Services.Data.Clients.FinnHubClient;
using OctoWhirl.Services.Data.Clients.YahooFinanceClient;
using OctoWhirl.Services.Data.Loaders;

namespace OctoWhirl.App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public ServiceProvider ServiceProvider { get; private set; }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            var services = new ServiceCollection();

            ConfigureServices(services);

            ServiceProvider = services.BuildServiceProvider();

            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // Configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.app.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.gui.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.services.json", optional: true, reloadOnChange: true)
                .Build();

            services.AddSingleton<IConfiguration>(configuration);

            // ViewModels
            services.AddTransient<MainViewModel>();
            services.AddTransient<HistoricalViewModel>();
            services.AddTransient<PricingViewModel>();

            // Views
            services.AddTransient<MainWindow>();
            services.AddTransient<HistoricalView>();
            services.AddTransient<PricingView>();

            // Services
            services.AddSingleton<IStatusService, StatusService>();
            services.AddSingleton<IViewFactory, ViewFactory>();

            // OctoWhirl Services
            services.AddTransient<DataBaseLoader>();
            services.AddTransient<DataLoader>();

            // OctoWhirl Clients
            services.AddHttpClient<YahooFinanceClient>();
            services.AddHttpClient<FinnHubClient>();
        }
    }

}
