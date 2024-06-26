﻿using Microsoft.AspNetCore.Mvc;
using SmallApiToolkit.Core.Response;
using SmallApiToolkit.Extensions;
using Weather.API.Extensions;
using Weather.Core.Abstractions;
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
            endpointRouteBuilder.MapGet("current",
                async (double latitude, double longitude, [FromServices] IGetCurrentWeatherHandler handler, CancellationToken cancellationToken) =>
                    await handler.SendAsync(new GetCurrentWeatherQuery(latitude, longitude), cancellationToken))
                        .ProducesDataResponse<CurrentWeatherDto>()
                        .WithName("GetCurrentWeather")
                        .WithTags("Getters");
            return endpointRouteBuilder;
        }

        private static IEndpointRouteBuilder BuildForecastWeatherEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapGet("forecast",
                async (double latitude, double longitude, [FromServices] IGetForecastWeatherHandler handler, CancellationToken cancellationToken) =>
                    await handler.SendAsync(new GetForecastWeatherQuery(latitude, longitude), cancellationToken))
                        .ProducesDataResponse<ForecastWeatherDto>()
                        .WithName("GetForecastWeather")
                        .WithTags("Getters");

            return endpointRouteBuilder;
        }

        private static IEndpointRouteBuilder BuildFavoriteWeatherEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapGet("favorites",
                async ([FromServices] IGetFavoritesHandler handler, CancellationToken cancellationToken) =>
                    await handler.SendAsync(EmptyRequest.Instance, cancellationToken))
                        .ProducesDataResponse<FavoritesWeatherDto>()
                        .WithName("GetFavorites")
                        .WithTags("Getters");

            endpointRouteBuilder.MapPost("favorite",
                async ([FromBody] AddFavoriteCommand addFavoriteCommand, [FromServices] IAddFavoriteHandler handler, CancellationToken cancellationToken) =>
                    await handler.SendAsync(addFavoriteCommand, cancellationToken))
                        .ProducesDataResponse<int>()
                        .WithName("AddFavorite")
                        .WithTags("Setters");

            endpointRouteBuilder.MapDelete("favorite/{id}",
                async (int id, [FromServices] IDeleteFavoriteHandler handler, CancellationToken cancellationToken) =>
                    await handler.SendAsync(new DeleteFavoriteCommand { Id = id }, cancellationToken))
                        .ProducesDataResponse<bool>()
                        .WithName("DeleteFavorite")
                        .WithTags("Delete");

            return endpointRouteBuilder;
        }
    }
}
