using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Weather.Core.Abstractions;
using Weather.Infrastructure.Database.EFContext;
using Weather.Infrastructure.Database.Repositories;
using Weather.Infrastructure.Mapping.Profiles;
using Weather.Infrastructure.Services;
using Wheaterbit.Client.Configuration;

namespace Weather.Infrastructure.Configuration
{
    public static class ContainerConfigurationExtension
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection serviceCollection, IConfiguration configuration)
            => serviceCollection
                .AddMapping()
                .AddDatabase()
                .AddExternalHttpServices(configuration)
                .AddServices();

        private static IServiceCollection AddServices(this IServiceCollection serviceCollection)
            => serviceCollection
                .AddScoped<IWeatherService, WeatherService>();

        private static IServiceCollection AddMapping(this IServiceCollection serviceCollection)
            => serviceCollection
                .AddAutoMapper(typeof(WeatherEntitiesProfile))
                .AddAutoMapper(typeof(WeatherbitClientProfile));

        private static IServiceCollection AddDatabase(this IServiceCollection serviceCollection)
            => serviceCollection
                .AddDbContext<WeatherContext>(opt => opt.UseInMemoryDatabase("Weather"))
                .AddScoped<IWeatherQueriesRepository, WeatherQueriesRepository>()
                .AddScoped<IWeatherCommandsRepository, WeatherCommandsRepository>();

        private static IServiceCollection AddExternalHttpServices(this IServiceCollection serviceCollection, IConfiguration configuration) 
            => serviceCollection
                .AddHttpClient()
                .AddWeatherbit(configuration);
    }
}
