namespace OctoWhirl.Services.Models.Requests
{
    public class GetListedOptionRequest
    {
        public List<string> References { get; set; }
        public DateTime AsOfDate { get; set; }
    }
}
