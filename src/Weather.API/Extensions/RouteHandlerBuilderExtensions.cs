using Weather.Core.HandlerModel;

namespace Weather.API.Extensions
{
    public static class RouteHandlerBuilderExtensions
    {
        public static RouteHandlerBuilder ProducesDataResponse<TResponse>(
            this RouteHandlerBuilder builder,
            params string[] additionalContentTypes)
            => builder.Produces<DataResponse<TResponse>>(StatusCodes.Status200OK, null, additionalContentTypes);
    }
}
