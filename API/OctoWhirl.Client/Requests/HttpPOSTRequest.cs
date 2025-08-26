namespace OctoWhirl.API.Client.Requests
{
    public class HttpPOSTRequest : HttpAbstractRequest
    {
        public override HttpRequestMethod Method => HttpRequestMethod.POST;

        public HttpPOSTRequest(string url, string route) : base(url, route)
        {
        }

        public override HttpRequestMessage Build()
        {
            var uriBuilder = new UriBuilder(new Uri(new Uri(BaseUrl), Route));

            if (QueryParameters.Any())
                uriBuilder.Query = string.Join("&", QueryParameters.Select(kvp =>
                    $"{Uri.EscapeDataString(kvp.Key)}={Uri.EscapeDataString(kvp.Value?.ToString() ?? string.Empty)}")
                );

            var request = new HttpRequestMessage(GetHttpMethod(), uriBuilder.Uri);

            foreach (var header in Headers)
                request.Headers.TryAddWithoutValidation(header.Key, header.Value);

            if (Content != null)
                request.Content = Content;

            return request;
        }
    }
}
