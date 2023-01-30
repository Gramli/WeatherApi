using Microsoft.AspNetCore.Mvc;
using Weather.API.Extensions;
using Weather.Core.Abstractions;
using Weather.Domain.Dtos;
using Weather.Domain.Dtos.Commands;
using Weather.Domain.Dtos.Queries;
using Weather.Domain.Http;

namespace Weather.API.EndpointBuilders
{
    public static class WeatherBuilder
    {
        public static IEndpointRouteBuilder BuildWeatherEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
        {

            endpointRouteBuilder
                .MapGroup("weather")
                .BuildActualWeatherEndpoints()
                .BuildForecastWeatherEndpoints()
                .BuildFavoriteWeatherEndpoints();

            return endpointRouteBuilder;
        }

        private static IEndpointRouteBuilder BuildActualWeatherEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapGet("v1/current",
                async (long latitude, long longtitude, [FromServices] IGetCurrentWeatherHandler handler, CancellationToken cancellationToken) =>
                    await handler.SendAsync(new GetCurrentWeatherQuery(latitude,longtitude), cancellationToken))
                        .Produces<CurrentWeatherDto>()
                        .WithName("GetCurrentWeather")
                        .WithTags("Getters");
            return endpointRouteBuilder;
        }

        private static IEndpointRouteBuilder BuildForecastWeatherEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapGet("v1/forecast",
                async (long latitude, long longtitude, [FromServices] IGetForecastWeatherHandler handler, CancellationToken cancellationToken) =>
                    await handler.SendAsync(new GetForecastWeatherQuery(latitude, longtitude), cancellationToken))
                        .Produces<ForecastWeatherDto>()
                        .WithName("GetForecastWeather")
                        .WithTags("Getters");

            return endpointRouteBuilder;
        }

        private static IEndpointRouteBuilder BuildFavoriteWeatherEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapGet("v1/favorites",
                async ([FromServices] IGetFavoritesHandler handler, CancellationToken cancellationToken) =>
                    await handler.SendAsync(EmptyRequest.Instance, cancellationToken))
                        .Produces<FavoritesWeatherDto>()
                        .WithName("GetFavorites")
                        .WithTags("Getters");

            endpointRouteBuilder.MapPost("v1/favorite",
                async ([FromBody] AddFavoriteCommand addFavoriteCommand, [FromServices] IAddFavoriteHandler handler, CancellationToken cancellationToken) =>
                    await handler.SendAsync(addFavoriteCommand, cancellationToken))
                        .Produces<bool>()
                        .WithName("AddFavorite")
                        .WithTags("Setters");

            return endpointRouteBuilder;
        }
    }
}
