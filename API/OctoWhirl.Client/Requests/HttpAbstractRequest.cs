namespace OctoWhirl.API.Client.Requests
{
    public abstract class HttpAbstractRequest : IHttpRequest
    {
        public virtual HttpRequestMethod Method => throw new NotImplementedException($"{nameof(Method)} is not defined.");

        public string BaseUrl { get; }
        public string Route { get; }

        public Dictionary<string, string> Headers { get; set; }
        public Dictionary<string, object?> QueryParameters { get; set; }

        public virtual HttpContent? Content { get; set; }

        public Uri FullUri => new Uri(new Uri(BaseUrl), Route);

        protected HttpAbstractRequest(string url, string route)
        {
            BaseUrl = url ?? throw new ArgumentNullException(nameof(url));
            Route = route ?? throw new ArgumentNullException(nameof(route));
            Headers = new Dictionary<string, string>();
            QueryParameters = new Dictionary<string, object?>();
        }

        public void AddQueryParameter(string key, object? value)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));
            if (QueryParameters.ContainsKey(key))
                throw new ArgumentException($"Query parameter '{key}' already exists.", nameof(key));

            QueryParameters.Add(key, value);
        }

        public void AddHeader(string key, string value)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));
            if (Headers.ContainsKey(key))
                throw new ArgumentException($"Header '{key}' already exists.", nameof(key));

            Headers.Add(key, value);
        }

        public abstract HttpRequestMessage Build();

        protected HttpMethod GetHttpMethod() => Method switch
        {
            HttpRequestMethod.GET => HttpMethod.Get,
            HttpRequestMethod.POST => HttpMethod.Post,
            HttpRequestMethod.PUT => HttpMethod.Put,
            HttpRequestMethod.DELETE => HttpMethod.Delete,
            _ => throw new NotSupportedException(Method.ToString())
        };
    }
}
