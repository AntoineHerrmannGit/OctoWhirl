using Core.Technicals.Extensions;
using Core.Http.Authentication;
using Core.Http.Helpers;
using System.Net.Http.Headers;


namespace Core.Http.HttpRequests
{
    public class HttpRequest
    {
        #region Public Properties
        public string Url { get; set; }
        public string Route { get; set; }
        public HttpRequestMethod Method { get; set; }
        
        public Dictionary<string, string> Headers { get; set; }
        public Dictionary<string, List<string>> Parameters { get; set; }
        public string Body { get; set; }
        public HttpAuthentication Authentication { get; set; }

        public Version Version { get; set; }
        #endregion Public Properties

        #region Public Filling Methods
        public HttpRequest SetUrl(string url)
        {
            Url = url;
            return this;
        }

        public HttpRequest SetMethod(HttpRequestMethod method)
        {
            Method = method;
            return this;
        }

        public HttpRequest AddRoutes(IEnumerable<string> routes)
        {
            if (routes.IsNullOrEmpty())
                return this;

            var route = routes?.Stringify("/");
            if (Route is null)
                Route = route;
            else
                Route = $"{Route}/{route}";

            return this;
        }

        public HttpRequest AddRoute(string route)
        {
            Route = route ?? $"{Route}/{route}";
            return this;
        }

        public HttpRequest AddHeader<T>(string key, T value)
            => AddHeader(key, value.Serialize());

        public HttpRequest AddHeader(string key, string value)
        {
            if (Headers is null)
                Headers = new Dictionary<string, string>();

            Headers.Add(key, value);
            return this;
        }

        public HttpRequest AddParameter<T>(string key, T value)
            => AddParameter(key, value.Serialize());

        public HttpRequest AddParameter(string key, string value)
        {
            if (key is null)
                throw new ArgumentNullException(nameof(key));

            if (Parameters is null)
                Parameters = new Dictionary<string, List<string>>();

            if(Parameters.ContainsKey(key) && Parameters[key] is not null && !Parameters[key].IsEmpty())
                Parameters[key].Add(value);

            Parameters[key] = new List<string> { value };
            return this;
        }

        public HttpRequest AddParameters<T>(string key, IEnumerable<T> values)
        {
            foreach (T value in values)
                AddParameter(key, value);
            return this;
        }

        public HttpRequest AddParameters(string key, IEnumerable<string> values)
        {
            foreach(string value in values)
                AddParameter(key, value);
            return this;
        }

        public HttpRequest AddParameters<T>(IEnumerable<KeyValuePair<string, T>> values)
        {
            foreach (var value in values)
                AddParameter(value.Key, value.Value);
            return this;
        }

        public HttpRequest AddParameters(IEnumerable<KeyValuePair<string, string>> values)
        {
            foreach (var value in values)
                AddParameter(value.Key, value.Value);
            return this;
        }

        public HttpRequest AddBody<T>(T body) 
            => AddBody(body.Serialize());

        public HttpRequest AddBody(string body)
        {
            Body = body;
            return this;
        }

        public HttpRequest AddAuthentication(string scheme, string username, string password)
            => AddAuthentication(new HttpAuthentication
            {
                Scheme = scheme,
                Username = username,
                Password = password
            });

        public HttpRequest AddAuthentication(HttpAuthentication authentication)
        {
            Authentication = authentication;
            return this;
        }

        public HttpRequest AddVersion(int version)
            => AddVersion(version.ToString());

        public HttpRequest AddVersion(string version)
        {
            if (!version.IsNullOrEmpty())
                Version = new Version(version);
            return this;
        }
        #endregion Public Filling Methods

        #region Public Method
        public HttpRequestMessage Build()
        {
            if (Url.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Url));

            var url = Url;
            if (!Route.IsNullOrEmpty())
                url += $"/{Route}";


            if (Parameters is not null && !Parameters.IsEmpty())
            {
                if (!url.Contains("?"))
                    url += "?";
                else 
                    url += "&";
                url += Parameters.SelectMany(p => p.Value.Select(v => $"{p.Key}={v}")).Stringify("&");
            }

            var request = new HttpRequestMessage(Method.ToHttpMethod(), url);

            if (Headers is not null)
                foreach(var kv in Headers)
                    request.Headers.Add(kv.Key, kv.Value);

            if (Body is not null)
                request.Content = new StringContent(Body);

            if (Authentication is not null)
                request.Headers.Authorization = new AuthenticationHeaderValue(Authentication.Scheme, $"{Authentication.Username}:{Authentication.Password}");
            
            if (Version is not null)
                request.Version = Version;

            return request;
        }
        #endregion Public Method
    }
}
