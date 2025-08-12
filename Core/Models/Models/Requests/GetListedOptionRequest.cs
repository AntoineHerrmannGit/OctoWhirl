namespace OctoWhirl.Services.Models.Requests
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. 
{
    public class GetListedOptionRequest
    {
        public List<string> Tickers { get; set; }
        public DateTime AsOfDate { get; set; }
    }
}
