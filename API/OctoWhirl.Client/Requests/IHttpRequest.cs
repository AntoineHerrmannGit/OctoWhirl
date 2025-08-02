namespace OctoWhirl.Client.Requests
{
    public interface IHttpRequest
    {
        HttpRequestMethod Method { get; }

        string BaseUrl { get; }
        string Route { get; }

        Dictionary<string, string> Headers { get; set; }
        Dictionary<string, object?> QueryParameters { get; set; }

        // Optionnal for POST/PUT Requests
        HttpContent? Content { get; set; }

        void AddQueryParameter(string key, object? value);
        void AddHeader(string key, string value);
        HttpRequestMessage Build();
    }
}
