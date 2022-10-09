using Microsoft.AspNetCore.Mvc;
using Weather.API.Extensions;
using Weather.Core.Abstractions;
using Weather.Domain;
using Weather.Domain.Dtos;

namespace Weather.API.EndpointBuilders
{
    public static class WeatherBuilder
    {
        public static IEndpointRouteBuilder BuildWeatherEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder
                .BuildActualWeatherEndpoints()
                .BuildForecastWeatherEndpoints()
                .BuildFavoriteWeatherEndpoints();

            return endpointRouteBuilder;
        }

        private static IEndpointRouteBuilder BuildActualWeatherEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapGet("weather/actual",
                async ([FromBody] LocationDto locationDto, [FromServices] IGetActualWeatherHandler handler, CancellationToken cancellationToken) =>
                    await handler.SendAsync(locationDto, cancellationToken))
                        .Produces<ActualWeatherDto>()
                        .WithName("GetActualWeather")
                        .WithTags("Getters");
            return endpointRouteBuilder;
        }

        private static IEndpointRouteBuilder BuildForecastWeatherEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapGet("weather/forecast",
                async ([FromBody] LocationDto locationDto, [FromServices] IForecastWeatherHandler handler, CancellationToken cancellationToken) =>
                    await handler.SendAsync(locationDto, cancellationToken))
                        .Produces<ForecastWeatherDto>()
                        .WithName("GetForecastWeather")
                        .WithTags("Getters");

            return endpointRouteBuilder;
        }

        private static IEndpointRouteBuilder BuildFavoriteWeatherEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapGet("weather/favorites",
                async ([FromServices] IGetFavoritesHandler handler, CancellationToken cancellationToken) =>
                    await handler.SendAsync(EmptyRequest.Instance, cancellationToken))
                        .Produces<FavoritesWeatherDto>()
                        .WithName("GetFavorites")
                        .WithTags("Getters");

            endpointRouteBuilder.MapPost("weather/favorite",
                async ([FromBody] LocationDto locationDto, [FromServices] IAddFavoriteHandler handler, CancellationToken cancellationToken) =>
                    await handler.SendAsync(locationDto, cancellationToken))
                        .Produces<bool>()
                        .WithName("AddFavorite")
                        .WithTags("Setters");

            return endpointRouteBuilder;
        }
    }
}
