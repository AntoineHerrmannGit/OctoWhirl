namespace OctoWhirl.Services.Models.Requests
{
    public class GetOptionRequest : GetStocksRequest
    {
        public List<double> Strikes { get; set; }
        public List<DateTime> Maturities { get; set; }
    }
}
