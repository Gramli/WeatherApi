using Microsoft.AspNetCore.Mvc;
using Weather.API.Extensions;
using Weather.Core.Abstractions;
using Weather.Core.HandlerModel;
using Weather.Domain.Commands;
using Weather.Domain.Dtos;
using Weather.Domain.Queries;

namespace Weather.API.EndpointBuilders
{
    public static class WeatherBuilder
    {
        public static IEndpointRouteBuilder BuildWeatherEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
        {

            endpointRouteBuilder
                .MapGroup("weather")
                .MapVersionGroup(1)
                .BuildActualWeatherEndpoints()
                .BuildForecastWeatherEndpoints()
                .BuildFavoriteWeatherEndpoints();

            return endpointRouteBuilder;
        }

        private static IEndpointRouteBuilder BuildActualWeatherEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapGet("/current",
                async (double latitude, double longitude, [FromServices] IStatusRequestHandler<CurrentWeatherDto, GetCurrentWeatherQuery> handler, CancellationToken cancellationToken) =>
                    await handler.SendAsync(new GetCurrentWeatherQuery(latitude, longitude), cancellationToken))
                        .ProducesDataResponse<CurrentWeatherDto>()
                        .WithName("GetCurrentWeather")
                        .WithTags("Getters");
            return endpointRouteBuilder;
        }

        private static IEndpointRouteBuilder BuildForecastWeatherEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapGet("/forecast",
                async (double latitude, double longitude, [FromServices] IStatusRequestHandler<ForecastWeatherDto, GetForecastWeatherQuery> handler, CancellationToken cancellationToken) =>
                    await handler.SendAsync(new GetForecastWeatherQuery(latitude, longitude), cancellationToken))
                        .ProducesDataResponse<ForecastWeatherDto>()
                        .WithName("GetForecastWeather")
                        .WithTags("Getters");

            return endpointRouteBuilder;
        }

        private static IEndpointRouteBuilder BuildFavoriteWeatherEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapGet("/favorites",
                async ([FromServices] IStatusRequestHandler<FavoritesWeatherDto, EmptyRequest> handler, CancellationToken cancellationToken) =>
                    await handler.SendAsync(new EmptyRequest(), cancellationToken))
                        .ProducesDataResponse<FavoritesWeatherDto>()
                        .WithName("GetFavorites")
                        .WithTags("Getters");

            endpointRouteBuilder.MapPost("/favorites",
                async ([FromBody] AddFavoriteCommand addFavoriteCommand, [FromServices] IStatusRequestHandler<int, AddFavoriteCommand> handler, CancellationToken cancellationToken) =>
                    await handler.SendAsync(addFavoriteCommand, cancellationToken))
                        .ProducesDataResponse<int>()
                        .WithName("AddFavorite")
                        .WithTags("Setters");

            endpointRouteBuilder.MapDelete("/favorites/{id}",
                async (int id, [FromServices] IStatusRequestHandler<bool, DeleteFavoriteCommand> handler, CancellationToken cancellationToken) =>
                    await handler.SendAsync(new DeleteFavoriteCommand { Id = id }, cancellationToken))
                        .ProducesDataResponse<bool>()
                        .WithName("DeleteFavorite")
                        .WithTags("Delete");

            return endpointRouteBuilder;
        }
    }
}
