using Microsoft.Extensions.DependencyInjection;
using OctoWhirl.Core.Models.Models.Enums;
using OctoWhirl.TechnicalServices.DataService;
using OctoWhirl.TechnicalServices.DataService.LocalDataBase;
using OctoWhirl.TechnicalServices.DataService.PolygonIO;
using OctoWhirl.TechnicalServices.DataService.YahooFinance;

namespace DataService.Factory
{
    public class FinancialServiceFactory
    {
        private readonly IServiceProvider _provider;

        public FinancialServiceFactory(IServiceProvider serviceProvider)
        {
            _provider = serviceProvider;
        }

        public IFinanceService GetService(DataSource source)
            => source switch
            {
                DataSource.YahooFinance => _provider.GetRequiredService<YahooFinanceClient>(),
                DataSource.PolygonIO => _provider.GetRequiredService<PolygonClient>(),
                DataSource.LocalDataBase => _provider.GetRequiredService<DataBaseLoader>(),
                _ => throw new NotSupportedException(source.ToString())
            };
    }
}
