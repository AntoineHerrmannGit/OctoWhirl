using Microsoft.Extensions.DependencyInjection;
using OctoWhirl.Core.Models.Enums;
using OctoWhirl.Services.Data.Clients.YahooFinanceClient;

namespace OctoWhirl.Services.Data.Loaders
{
    public class DataLoaderFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public DataLoaderFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IDataService GetService(DataSource source)
        {
            return source switch
            {
                DataSource.YahooFinance => _serviceProvider.GetRequiredService<YahooFinanceClient>(),
                DataSource.DataBase => _serviceProvider.GetRequiredService<DataBaseLoader>(),
                _ => throw new ArgumentOutOfRangeException(nameof(source)),
            };
        }
    }
}
