using System.IO;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OctoWhirl.GUI.ViewModels;
using OctoWhirl.GUI.ViewModels.Technical;
using OctoWhirl.GUI.Views;
using OctoWhirl.Services.Data;
using OctoWhirl.Services.Data.Clients.FinnHubClient;
using OctoWhirl.Services.Data.Clients.YahooFinanceClient;

namespace OctoWhirl
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IConfiguration Configuration { get; private set; }
        public static IServiceProvider ServiceProvider { get; private set; }

        private readonly IStatusService _statusService = new StatusService();

        protected override void OnStartup(StartupEventArgs e)
        {
            GenerateConfiguration();
            RegisterServices();
            ShowMainWindow();
        }

        private void ShowMainWindow()
        {
            var mainViewModel = ServiceProvider.GetRequiredService<MainViewModel>();
            var mainWindow = new MainWindow { DataContext = mainViewModel };
            mainWindow.Show();
        }

        private void GenerateConfiguration()
        {
            Configuration = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                            .Build();
        }

        private void RegisterServices()
        {
            var services = new ServiceCollection();

            services.AddSingleton(Configuration);

            // Register services & viewmodels
            services.AddSingleton<IStatusService, StatusService>();
            services.AddSingleton<MainViewModel>();
            services.AddTransient<HistoricalViewModel>();
            services.AddTransient<PricingViewModel>();

            // Register HTTP Clients
            services.AddHttpClient<FinnHubClient>();
            services.AddHttpClient<YahooFinanceClient>();

            // Register Views
            services.AddSingleton<IViewFactory, ViewFactory>();

            services.AddTransient<HistoricalView>();
            services.AddTransient<HistoricalViewModel>();

            services.AddTransient<PricingView>();
            services.AddTransient<PricingViewModel>();

            // Register the MainWindow with DI
            services.AddTransient<MainWindow>();

            // Register other Services
            services.AddScoped<DataLoader>();

            ServiceProvider = services.BuildServiceProvider();
        }
    }
}
