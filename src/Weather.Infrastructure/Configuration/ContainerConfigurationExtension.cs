using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Weather.Infrastructure.Database.EFContext;

namespace Weather.Infrastructure.Configuration
{
    public static class ContainerConfigurationExtension
    {
        public static IServiceCollection AddDatabase(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddDbContext<WeatherContext>(opt => opt.UseInMemoryDatabase("Weather"));
            return serviceCollection;
        }
    }
}
