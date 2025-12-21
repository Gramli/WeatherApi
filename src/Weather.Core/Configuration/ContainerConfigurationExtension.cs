using Microsoft.Extensions.DependencyInjection;
using Weather.Core.Abstractions;
using Weather.Core.Commands;
using Weather.Core.HandlerModel;
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
                .AddScoped<IStatusRequestHandler<CurrentWeatherDto, GetCurrentWeatherQuery>, GetCurrentWeatherHandler>()
                .AddScoped<IStatusRequestHandler<FavoritesWeatherDto, EmptyRequest>, GetFavoritesHandler>()
                .AddScoped<IStatusRequestHandler<ForecastWeatherDto, GetForecastWeatherQuery>, GetForecastWeatherHandler>()
                .AddScoped<IStatusRequestHandler<int, AddFavoriteCommand>, AddFavoriteHandler>()
                .AddScoped<IStatusRequestHandler<bool, DeleteFavoriteCommand>, DeleteFavoriteHandler>();

        private static IServiceCollection AddValidation(this IServiceCollection serviceCollection) 
            => serviceCollection
                .AddSingleton<IRequestValidator<CurrentWeatherDto>, CurrentWeatherDtoValidator>()
                .AddSingleton<IRequestValidator<ForecastWeatherDto>, ForecastWeatherDtoValidator>()
                .AddSingleton<IRequestValidator<LocationDto>, LocationDtoValidator>()
                .AddSingleton<IRequestValidator<AddFavoriteCommand>, AddFavoriteCommandValidator>()
                .AddSingleton<IRequestValidator<GetCurrentWeatherQuery>, GetCurrentWeatherQueryValidator>()
                .AddSingleton<IRequestValidator<GetForecastWeatherQuery>, GetForecastWeatherValidator>()
                .AddSingleton<IRequestValidator<DeleteFavoriteCommand>, DeleteFavoriteCommandValidator>();
    }
}
