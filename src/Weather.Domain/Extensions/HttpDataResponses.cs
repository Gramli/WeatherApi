using Weather.Domain.Http;

namespace Weather.Domain.Extensions
{
    public static class HttpDataResponses
    {
        public static HttpDataResponse<T> AsBadRequest<T>(params string[] errorMessages)
        {
            return new HttpDataResponse<T>
            {
                StatusCode = System.Net.HttpStatusCode.BadRequest,
                Errors = errorMessages
            };
        }

        public static HttpDataResponse<T> AsInternalServerError<T>(params string[] errorMessages)
        {
            return new HttpDataResponse<T>
            {
                StatusCode = System.Net.HttpStatusCode.InternalServerError,
                Errors = errorMessages
            };
        }

        public static HttpDataResponse<T> AsOK<T>(T data)
        {
            return new HttpDataResponse<T>
            {
                Data = data,
                StatusCode = System.Net.HttpStatusCode.OK,
            };
        }

        public static HttpDataResponse<T> AsNoContent<T>()
        {
            return new HttpDataResponse<T>
            {
                StatusCode = System.Net.HttpStatusCode.NoContent,
            };
        }
    }
}
