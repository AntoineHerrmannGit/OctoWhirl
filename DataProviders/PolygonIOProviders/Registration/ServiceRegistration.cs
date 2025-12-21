using DataProviders.GenericProviders.Registration;
using Microsoft.Extensions.DependencyInjection;
using Models.Models.Objects.CorporateActions;
using Models.Models.Objects.Options;
using Models.Models.Objects.Spots;
using PolygonIOProviders.Client;
using PolygonIOProviders.Configuration;
using PolygonIOProviders.Interfaces;
using PolygonIOProviders.Providers;

namespace PolygonIOProviders.Registration
{
    public static class ServiceRegistration
    {
        public static IServiceCollection RegisterPolygonServices(this IServiceCollection services)
            => services.RegisterProvider<PolygonIOOption, PolygonIOOptionsProvider>()
                       .RegisterProvider<PolygonIOSpot, PolygonIOSpotProvider>()
                       .RegisterProvider<PolygonIODividend, PolygonIODividendProvider>()
                       .RegisterProvider<PolygonIOSplit, PolygonIOSplitProvider>()
                       .RegisterConfiguration<PolygonIOConfiguration>("polygonio_settings.json", "PolygonIO")
                       .AddScoped<IPolygonIOClient, PolygonIOClient>();
    }
}
