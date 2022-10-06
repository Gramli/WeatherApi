using System.Net;

namespace Weather.Domain
{
    public class HttpDataResponse<T>
    {
        public T Data { get; init; }

        public HttpStatusCode StatusCode { get; init; }

    }
}
