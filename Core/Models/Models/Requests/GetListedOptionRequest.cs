namespace OctoWhirl.Services.Models.Requests
{
    public class GetListedOptionRequest
    {
        public List<string> Tickers { get; set; } = new List<string>();
        public DateTime AsOfDate { get; set; }
    }
}
