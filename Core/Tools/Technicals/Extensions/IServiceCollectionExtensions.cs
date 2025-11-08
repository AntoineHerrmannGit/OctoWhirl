using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OctoWhirl.Core.Tools.Technicals.FileManagement;

namespace OctoWhirl.Core.Tools.Technicals.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddConfiguration<TConfiguration>(this IServiceCollection services, string filename, string section) where TConfiguration : class
            => services.Configure<TConfiguration>(options => 
                new ConfigurationBuilder().AddJsonFile(FileManager.FindFilePath(filename), optional: false)
                                          .Build()
                                          .GetSection(section)
                                          .Get<TConfiguration>()
                );
    }
}
