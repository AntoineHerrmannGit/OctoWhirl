using OctoWhirl.Client.Requests;
using OctoWhirl.Core.Models.Requests;
using OctoWhirl.Models.Technicals;

namespace OctoWhirl.Client
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
