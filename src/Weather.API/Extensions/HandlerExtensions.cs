using System.Net;
using Weather.Core.Abstractions;
using Weather.Core.HandlerModel;

namespace Weather.API.Extensions
{
    public static class HandlerExtensions
    {
        /// <summary>
        /// Executes a request handler and maps the response to an appropriate HTTP result.
        /// </summary>
        public static async Task<IResult> SendAsync<TResponse, TRequest>(this IStatusRequestHandler<TResponse, TRequest> requestHandler, TRequest request, CancellationToken cancellationToken)
        {
            var response = await requestHandler.HandleAsync(request, cancellationToken);

            return response.StatusCode switch
            {
                HandlerStatusCode.SuccessWithEmptyResult => Results.NoContent(),
                HandlerStatusCode.Success => Results.Json(response, statusCode: (int)HttpStatusCode.OK),
                HandlerStatusCode.ValidationError => Results.Json(response, statusCode: (int)HttpStatusCode.BadRequest),
                HandlerStatusCode.InternalError => Results.Json(response, statusCode: (int)HttpStatusCode.InternalServerError),
                _ => throw new InvalidOperationException($"Unknown HandlerStatusCode: {response.StatusCode}"),
            };
        }
    }
}
