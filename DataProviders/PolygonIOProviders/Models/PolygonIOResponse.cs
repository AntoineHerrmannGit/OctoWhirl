namespace PolygonIOProviders.Models
{
    internal class PolygonIOResponse<T>
    {
        public string ticker { get; set; }
        public int queryCount { get; set; }
        public int resultCount { get; set; }
        public bool adjusted { get; set; }
        public List<T> results { get; set; }
        public string status { get; set; }
        public string request_id { get; set; }
        public int count { get; set; }
        public string message { get; set; }
        public string next_url { get; set; }
    }
}
