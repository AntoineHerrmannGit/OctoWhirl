using Core.Http.HttpRequests;

namespace Core.Http.Helpers
{
    internal static class HttpRequestMethodHelper
    {
        public static HttpMethod ToHttpMethod(this HttpRequestMethod @this)
            => @this switch
            {
                HttpRequestMethod.Get => HttpMethod.Get,
                HttpRequestMethod.Post => HttpMethod.Post,
                HttpRequestMethod.Put => HttpMethod.Put,
                HttpRequestMethod.Delete => HttpMethod.Delete,
                HttpRequestMethod.Head => HttpMethod.Head,
                HttpRequestMethod.Options => HttpMethod.Options,
                HttpRequestMethod.Trace => HttpMethod.Trace,
                HttpRequestMethod.Patch => HttpMethod.Patch,
                HttpRequestMethod.Connect => HttpMethod.Connect,
                _ => throw new NotSupportedException(),
            };
    }
}
