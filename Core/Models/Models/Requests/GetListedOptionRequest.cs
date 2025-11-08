namespace OctoWhirl.Core.Models.Models.Requests
{
    public class GetListedOptionRequest
    {
        public IEnumerable<string> Instruments { get; set; }
        public DateTime AsOfDate { get; set; }
    }
}
