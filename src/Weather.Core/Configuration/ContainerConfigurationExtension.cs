using Microsoft.Extensions.DependencyInjection;
using Validot;
using Weather.Core.Abstractions;
using Weather.Core.Commands;
using Weather.Core.Extensions;
using Weather.Core.Queries;
using Weather.Core.Validation;
using Weather.Domain.Commands;
using Weather.Domain.Dtos;
using Weather.Domain.Queries;
using Wheaterbit.Client.Validation;

namespace Weather.Core.Configuration
{
    public static class ContainerConfigurationExtension
    {
        public static IServiceCollection AddCore(this IServiceCollection serviceCollection)
            => serviceCollection
                .AddValidation()
                .AddHandlers();

        private static IServiceCollection AddHandlers(this IServiceCollection serviceCollection) 
            => serviceCollection
                .AddScoped<IGetCurrentWeatherHandler, GetCurrentWeatherHandler>()
                .AddScoped<IGetFavoritesHandler, GetFavoritesHandler>()
                .AddScoped<IGetForecastWeatherHandler, GetForecastWeatherHandler>()
                .AddScoped<IAddFavoriteHandler, AddFavoriteHandler>()
                .AddScoped<IDeleteFavoriteHandler, DeleteFavoriteHandler>();

        private static IServiceCollection AddValidation(this IServiceCollection serviceCollection) 
            => serviceCollection
                .AddValidotSingleton<IValidator<CurrentWeatherDto>, CurrentWeatherDtoSpecificationHolder, CurrentWeatherDto>()
                .AddValidotSingleton<IValidator<ForecastWeatherDto>, ForecastWeatherDtoSpecificationHolder, ForecastWeatherDto>()
                .AddValidotSingleton<IValidator<LocationDto>, LocationDtoSpecificationHolder, LocationDto>()
                .AddValidotSingleton<IValidator<AddFavoriteCommand>, AddFavoriteCommandSpecificationHolder, AddFavoriteCommand>()
                .AddValidotSingleton<IValidator<GetCurrentWeatherQuery>, GetCurrentWeatherQuerySpecificationHolder, GetCurrentWeatherQuery>()
                .AddValidotSingleton<IValidator<GetForecastWeatherQuery>, GetForecastWeatherSpecificationHolder, GetForecastWeatherQuery>()
                .AddValidotSingleton<IValidator<DeleteFavoriteCommand>, DeleteFavoriteCommandSpecificationHolder, DeleteFavoriteCommand>();
    }
}
