using Microsoft.Extensions.DependencyInjection;
using Validot;

namespace Weather.Core.Configuration
{
    public static class ContainerConfigurationExtension
    {
        public static IServiceCollection AddCore(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddValidation();
        }

        internal static IServiceCollection AddValidation(this IServiceCollection serviceCollection) 
        {
            var holderAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            var holders = Validator.Factory.FetchHolders(holderAssemblies)
                .GroupBy(h => h.SpecifiedType)
                .Select(s => new
                {
                    ValidatorType = s.First().ValidatorType,
                    ValidatorInstance = s.First().CreateValidator()
                });
            foreach (var holder in holders)
            {
                serviceCollection.AddSingleton(holder.ValidatorType, holder.ValidatorInstance);
            }

            return serviceCollection;
        }
    }
}
