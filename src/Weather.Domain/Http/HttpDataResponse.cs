using System.Net;

namespace Weather.Domain.Http
{
    public class HttpDataResponse<T> : DataResponse<T>
    {
        public HttpStatusCode StatusCode { get; init; }

    }
}
