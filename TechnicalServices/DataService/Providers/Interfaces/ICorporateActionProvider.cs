using OctoWhirl.Core.Models.Models.Common;
using OctoWhirl.Core.Models.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OctoWhirl.TechnicalServices.DataService.Providers.Interfaces
{
    public interface ICorporateActionProvider : IDataProvider<CorporateAction>
    {
        Task<IEnumerable<CorporateAction>> GetActions(IMarketDataRequest request, CancellationToken cancellation);
    }
}
