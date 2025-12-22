using Weather.Core.HandlerModel;

namespace Weather.API.Extensions
{
    public static class RouteHandlerBuilderExtensions
    {
        public static RouteHandlerBuilder ProducesDataResponse<TResponse>(
            this RouteHandlerBuilder builder,
            int statusCode = StatusCodes.Status200OK,
            params string[] additionalContentTypes)
            => builder.Produces<DataResponse<TResponse>>(statusCode, null, additionalContentTypes);

        public static RouteHandlerBuilder ProducesDataResponse<TResponse>(
            this RouteHandlerBuilder builder,
            params string[] additionalContentTypes)
            => builder.Produces<DataResponse<TResponse>>(StatusCodes.Status200OK, null, additionalContentTypes);
    }
}
