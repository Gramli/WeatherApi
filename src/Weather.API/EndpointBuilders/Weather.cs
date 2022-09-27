using Microsoft.AspNetCore.Mvc;
using Weather.Core.Abstractions;
using Weather.Domain;
using Weather.Domain.Dtos;
using Weather.Domain.Payloads;

namespace Weather.API.EndpointBuilders
{
    public static class Weather
    {
        public static IEndpointRouteBuilder CreateWeatherEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapGet("weather/actual", 
                async ([FromBody] LocationPayload locationPayload, [FromServices]IGetActualWeatherHandler handler, CancellationToken cancellationToken) =>
                    await handler.HandleAsync(locationPayload, cancellationToken))
                        .Produces<ActualWeatherDto>()
                        .WithName("GetActualWeather")
                        .WithTags("Getters");

            endpointRouteBuilder.MapGet("weather/forecast", 
                async ([FromBody] LocationPayload locationPayload, [FromServices] IForecastWeatherHandler handler, CancellationToken cancellationToken) =>
                    await handler.HandleAsync(locationPayload, cancellationToken))
                        .Produces<ForecastWeatherDto>()
                        .WithName("GetForecastWeather")
                        .WithTags("Getters");

            endpointRouteBuilder.MapGet("weather/favorites",
                async ([FromServices] IGetFavoritesHandler handler, CancellationToken cancellationToken) =>
                    await handler.HandleAsync(EmptyRequest.Instance, cancellationToken))
                        .Produces<FavoritesWeatherDto>()
                        .WithName("GetFavorites")
                        .WithTags("Getters");

            endpointRouteBuilder.MapPost("weather/favorite",
                async ([FromBody] AddFavoritePayload addFavoritePayload, [FromServices] IAddFavoriteHandler handler, CancellationToken cancellationToken) =>
                    await handler.HandleAsync(addFavoritePayload, cancellationToken))
                        .Produces<bool>()
                        .WithName("AddFavorite")
                        .WithTags("Setters");

            return endpointRouteBuilder;
        }
    }
}
