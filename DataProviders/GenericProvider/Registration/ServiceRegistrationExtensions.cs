using DataProviders.GenericProvider.DataProvider;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Models.Models.Interfaces;
using OctoWhirl.Core.Tools.Technicals.FileManagement;

namespace DataProviders.GenericProviders.Registration
{
    public static class ServiceRegistrationExtensions
    {
        public static IServiceCollection RegisterProvider<TMakretData, TImplementation>(this IServiceCollection services)
            where TMakretData : IMarketData
            where TImplementation : class, IDataProvider<TMakretData>
            => services.AddScoped<IDataProvider<TMakretData>, TImplementation>();

        public static IServiceCollection RegisterConfiguration<TConfiguration>(this IServiceCollection services, string filename, string section)
            where TConfiguration : class
            => services.Configure<TConfiguration>(options =>
                           new ConfigurationBuilder()
                               .AddJsonFile(FileManager.FindFilePath(filename), optional: false, reloadOnChange: true)
                               .Build()
                               .GetRequiredSection(section)
                               .Bind(options)
                       );
    }
}
