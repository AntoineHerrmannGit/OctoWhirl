namespace PolygonIOProviders.Models
{
    internal class PolygonIOCorporateActionResult
    {
        public string? id { get; set; }

        public string? ticker { get; set; }

        public string? ca_type { get; set; }

        public double? cash_amount { get; set; }

        public string? currency { get; set; }

        public string? declaration_date { get; set; }       // DateTime at wrong format

        public string? dividend_type { get; set; }

        public string? ex_dividend_date { get; set; }       // DateTime at wrong format

        public int? frequency { get; set; }

        public string? pay_date { get; set; }               // DateTime at wrong format

        public string? record_date { get; set; }            // DateTime at wrong format

        public string? description { get; set; }

        public string? notes { get; set; }

        public double? split_from { get; set; }

        public double? split_to { get; set; }

        public string? old_ticker { get; set; }

        public string? new_ticker { get; set; }

        public string? target_ticker { get; set; }
    }
}
