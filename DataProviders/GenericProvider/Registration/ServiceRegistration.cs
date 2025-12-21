using DataProviders.GenericProvider.DataGetters;
using DataProviders.GenericProvider.ProviderSelectors;
using Microsoft.Extensions.DependencyInjection;

namespace DataProviders.GenericProvider.Registration
{
    public static class ServiceRegistration
    {
        public static IServiceCollection RegisterGenericServices(this IServiceCollection services)
            => services.AddLogging()
                       .AddScoped<IDataGetter, DataGetter>()
                       .AddScoped<IProviderSelector, ProviderSelector>();
    }
}
