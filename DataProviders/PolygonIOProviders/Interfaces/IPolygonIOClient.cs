using Core.Http.HttpRequests;

namespace PolygonIOProviders.Interfaces
{
    internal interface IPolygonIOClient
    {
        Task<TPolygonIOModel> CallApi<TPolygonIOModel>(HttpRequest request, CancellationToken cancellationToken = default)
            where TPolygonIOModel : class;
    }
}
