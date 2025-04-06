namespace OctoWhirl.Services.Clients.FinnHubClient
{
    public class CandleResponse
    {
        public List<float> c { get; set; } 
        public List<float> h { get; set; }
        public List<float> l { get; set; }
        public List<float> o { get; set; }
        public List<long> t { get; set; }   // timestamp (Unix)
        public List<long> v { get; set; }
        public string s { get; set; }       // status
    }

    public class OptionContractResponse
    {
        public string contractType { get; set; }
        public string symbol { get; set; }
        public float strike { get; set; }
        public float lastPrice { get; set; }
    }

    public class OptionExpirationResponse
    {
        public string expirationDate { get; set; }
        public List<OptionContractResponse> calls { get; set; }
        public List<OptionContractResponse> puts { get; set; }
    }

    public class OptionChainResponse
    {
        public string code { get; set; }
        public List<OptionExpirationResponse> optionChain { get; set; }
    }
}