using DataProviders.GenericProviders.Registration;
using DataProviders.YahooFinanceProviders.Client;
using DataProviders.YahooFinanceProviders.Configuration;
using DataProviders.YahooFinanceProviders.Providers;
using DataProviders.YahooFincnaceProviders.Interfaces;
using DataProviders.YahooFincnaceProviders.Providers;
using Microsoft.Extensions.DependencyInjection;
using Models.Models.Objects.CorporateActions;
using Models.Models.Objects.Spots;

namespace DataProviders.YahooFinanceProviders.Registration
{
    public static class ServiceRegistration
    {
        public static IServiceCollection RegisterYahooServices(this IServiceCollection services)
            => services.AddScoped<IYahooClient, YahooClient>()
                       .RegisterProvider<YahooFinanceSpot, YahooFinanceSpotProvider>()
                       .RegisterProvider<YahooFinanceDividend, YahooFinanceDividendProvider>()
                       .RegisterProvider<YahooFinanceSplit, YahooFinanceSplitProvider>()
                       .RegisterConfiguration<YahooFinanceConfiguration>("yahoo_finance_settings.json", "Yahoo");
    }
}
