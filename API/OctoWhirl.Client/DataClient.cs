using OctoWhirl.API.Client.Requests;
using OctoWhirl.Core.Models.Models.Requests;
using OctoWhirl.Core.Models.Models.Technicals;

namespace OctoWhirl.API.Client
{
    public class DataClient : BaseClient
    {
        public DataClient() : base("data")
        {
        }

        public Task<List<CandleSerie>> GetSpots(GetCandlesRequest request)
        {
            return Execute<List<CandleSerie>>(CreateRequest("spots", HttpRequestMethod.POST));
        }
    }
}
