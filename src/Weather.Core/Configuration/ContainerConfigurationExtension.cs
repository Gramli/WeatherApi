using Microsoft.Extensions.DependencyInjection;
using SmallApiToolkit.Core.RequestHandlers;
using SmallApiToolkit.Core.Validation;
using Validot;
using Weather.Core.Abstractions;
using Weather.Core.Commands;
using Weather.Core.Extensions;
using Weather.Core.Queries;
using Weather.Core.Validation;
using Weather.Domain.Commands;
using Weather.Domain.Dtos;
using Weather.Domain.Queries;

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
                .AddScoped<IHttpRequestHandler<CurrentWeatherDto, GetCurrentWeatherQuery>, GetCurrentWeatherHandler>()
                .AddScoped<IGetFavoritesHandler, GetFavoritesHandler>()
                .AddScoped<IGetForecastWeatherHandler, GetForecastWeatherHandler>()
                .AddScoped<IAddFavoriteHandler, AddFavoriteHandler>()
                .AddScoped<IDeleteFavoriteHandler, DeleteFavoriteHandler>();

        private static IServiceCollection AddValidation(this IServiceCollection serviceCollection) 
            => serviceCollection
                .AddValidotSingleton<IValidator<CurrentWeatherDto>, CurrentWeatherDtoValidator, CurrentWeatherDto>()
                .AddValidotSingleton<IValidator<ForecastWeatherDto>, ForecastWeatherDtoValidator, ForecastWeatherDto>()
                .AddValidotSingleton<IValidator<LocationDto>, LocationDtoValidator, LocationDto>()
                .AddSingleton<IRequestValidator<AddFavoriteCommand>, AddFavoriteCommandValidator>()
                .AddValidotSingleton<IValidator<GetCurrentWeatherQuery>, GetCurrentWeatherQueryValidator, GetCurrentWeatherQuery>()
                .AddValidotSingleton<IValidator<GetForecastWeatherQuery>, GetForecastWeatherValidator, GetForecastWeatherQuery>()
                .AddValidotSingleton<IValidator<DeleteFavoriteCommand>, DeleteFavoriteCommandValidator, DeleteFavoriteCommand>();
    }
}
