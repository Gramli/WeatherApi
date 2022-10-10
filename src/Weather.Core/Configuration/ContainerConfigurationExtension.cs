using Microsoft.Extensions.DependencyInjection;
using Validot;
using Weather.Core.Abstractions;
using Weather.Core.Queries;
using Weather.Domain.Extensions;

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
                .AddScoped<IGetActualWeatherHandler, GetActualWeatherHandler>()
                .AddScoped<IGetFavoritesHandler, GetFavoritesHandler>();
        }

        private static IServiceCollection AddValidation(this IServiceCollection serviceCollection) 
        {
            var holderAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            var holders = Validator.Factory.FetchHolders(holderAssemblies)
                .GroupBy(h => h.SpecifiedType)
                .Select(s => new
                {
                    ValidatorType = s.First().ValidatorType,
                    ValidatorInstance = s.First().CreateValidator()
                });

            holders.ForEach((holder) => serviceCollection.AddSingleton(holder.ValidatorType, holder.ValidatorInstance));

            return serviceCollection;
        }
    }
}
