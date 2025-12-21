namespace PolygonIOProviders.Models
{
    internal class PolygonIOCorporateActionResponse
    {
        public string status { get; set; }

        public string request_id { get; set; }

        public string next_url { get; set; }

        public List<PolygonIOCorporateActionResult> results { get; set; }
    }
}
