using Weather.Core.HandlerModel;

namespace Weather.API.Extensions
{
    public static class RouteHandlerBuilderExtensions
    {
        public static RouteHandlerBuilder ProducesDataResponse<TResponse>(
            this RouteHandlerBuilder builder,
            int statusCode = StatusCodes.Status200OK,
            string? contentType = null,
            params string[] additionalContentTypes)
        => builder.Produces<DataResponse<TResponse>>(statusCode, contentType, additionalContentTypes);
    }
}
