namespace PolygonIOProviders.Models
{
    internal class PolygonIOOptionResponse
    {
        public string cfi { get; set; }
        public string contract_type { get; set; }
        public string exercise_style { get; set; }
        public string expiration_date { get; set; }
        public string primary_exchange { get; set; }
        public string shares_per_contract { get; set; }
        public double strike_price { get; set; }
        public string ticker { get; set; }
        public string underlying_ticker { get; set; }
    }
}
