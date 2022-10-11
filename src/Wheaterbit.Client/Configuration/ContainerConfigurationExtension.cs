using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wheaterbit.Client.Abstractions;
using Wheaterbit.Client.Options;

namespace Wheaterbit.Client.Configuration
{
    public static class ContainerConfigurationExtension
    {
        public static IServiceCollection AddWeatherbit(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.Configure<WeatherbitOptions>(configuration.GetSection(WeatherbitOptions.Weatherbit));
            
            return serviceCollection.AddSingleton<IWheaterbitHttpClient, WheaterbitHttpClient>();
        }
    }
}
