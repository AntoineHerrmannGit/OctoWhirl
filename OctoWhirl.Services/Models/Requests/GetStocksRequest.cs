using OctoWhirl.Core.Models.Enums;
using OctoWhirl.Core.Models.Technicals;

namespace OctoWhirl.Services.Models.Requests
{
    public class GetStocksRequest
    {
        public List<string> Tickers { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ClientSource Source { get; set; }
        public ResolutionInterval Interval { get; set; }
    }
}
