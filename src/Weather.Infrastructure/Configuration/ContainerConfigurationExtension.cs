using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Weather.Infrastructure.Database.EFContext;
using Weather.Infrastructure.Mapping.Profiles;

namespace Weather.Infrastructure.Configuration
{
    public static class ContainerConfigurationExtension
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            return services
                .AddMapping()
                .AddDatabase();
        }

        private static IServiceCollection AddMapping(this IServiceCollection services)
        {
            return services
                .AddAutoMapper(typeof(WeatherEntitiesProfile));
        }

        private static IServiceCollection AddDatabase(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddDbContext<WeatherContext>(opt => opt.UseInMemoryDatabase("Weather"));
        }
    }
}
