namespace OctoWhirl.Services.Models.Requests
{
    public class GetListedOptionRequest
    {
        public List<string> Tickers { get; set; }
        public DateTime AsOfDate { get; set; }
    }
}
