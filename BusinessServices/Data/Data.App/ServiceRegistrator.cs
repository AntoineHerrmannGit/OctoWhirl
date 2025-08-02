using DataService.Factory;
using OctoWhirl.BusinessServices.Data.Core;
using OctoWhirl.TechnicalServices.DataService.LocalDataBase;
using OctoWhirl.TechnicalServices.DataService.PolygonIO;
using OctoWhirl.TechnicalServices.DataService.YahooFinance;

namespace OctoWhirl.BusinessServices.Data.App
{
    public static class ServiceRegistrator
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddTransient<IBusinessDataService, BusinessDataService>();
            services.AddSingleton<FinancialServiceFactory>();

            services.AddHttpClient<YahooFinanceClient>();
            services.AddHttpClient<PolygonClient>();
            services.AddScoped<DataBaseLoader>();
        }

        public static void RegisterConfigurations(this ConfigurationManager manager)
        {
            manager.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            manager.AddJsonFile("service.config.json", optional: false, reloadOnChange: true);
        }
    }
}
