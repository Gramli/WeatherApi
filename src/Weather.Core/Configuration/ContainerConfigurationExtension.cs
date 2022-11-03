using Microsoft.Extensions.DependencyInjection;
using Validot;
using Weather.Core.Abstractions;
using Weather.Core.Commands;
using Weather.Core.Extensions;
using Weather.Core.Queries;
using Weather.Core.Validation;
using Weather.Domain.Dtos;
using Wheaterbit.Client.Validation;

namespace Weather.Core.Configuration
{
    public static class ContainerConfigurationExtension
    {
        public static IServiceCollection AddCore(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddValidation()
                .AddHandlers();
        }

        private static IServiceCollection AddHandlers(this IServiceCollection serviceCollection) 
        {
            return serviceCollection
                .AddScoped<IGetCurrentWeatherHandler, GetCurrentWeatherHandler>()
                .AddScoped<IGetFavoritesHandler, GetFavoritesHandler>()
                .AddScoped<IGetForecastWeatherHandler, GetForecastWeatherHandler>()
                .AddScoped<IAddFavoriteHandler, AddFavoriteHandler>();
        }

        private static IServiceCollection AddValidation(this IServiceCollection serviceCollection) 
        {
            return serviceCollection.AddValidotSingleton<IValidator<CurrentWeatherDto>, CurrentWeatherDtoSpecificationHolder, CurrentWeatherDto>()
                .AddValidotSingleton<IValidator<ForecastWeatherDto>, ForecastWeatherDtoSpecificationHolder, ForecastWeatherDto>()
                .AddValidotSingleton<IValidator<LocationDto>, LocationDtoSpecificationHolder,LocationDto>();
        }
    }
}
