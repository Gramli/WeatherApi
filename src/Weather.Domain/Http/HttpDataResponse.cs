using System.Net;

namespace Weather.Domain.Http
{
    public class HttpDataResponse<T>
    {
        public T Data { get; init; }

        public HttpStatusCode StatusCode { get; init; }

        public IReadOnlyCollection<string> Errors { get; init; }

    }
}
