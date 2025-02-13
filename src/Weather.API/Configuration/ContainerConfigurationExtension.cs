using Microsoft.FeatureManagement;
using Weather.API.Options;
using Weather.Domain.FeatureFlags;

namespace Weather.API.Configuration
{
    public static class ContainerConfigurationExtension
    {
        public static IServiceCollection AddApi(this IServiceCollection serviceCollection, IConfiguration configuration)
            => serviceCollection.AddApiOptions(configuration)
                .AddApiFeatureManagement(configuration);

        private static IServiceCollection AddApiOptions(this IServiceCollection serviceCollection, IConfiguration configuration)
            => serviceCollection.Configure<ApiLoggingOptions>(configuration.GetSection(ApiLoggingOptions.Key));

        private static IServiceCollection AddApiFeatureManagement(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            /*builder.Configuration.AddAzureAppConfiguration(options =>
            {
                options.Connect("")
                .UseFeatureFlags(featureFlagOptions =>
                {
                    featureFlagOptions.SetRefreshInterval(TimeSpan.FromMinutes(1));
                });
            });*/

            serviceCollection.AddFeatureManagement(configuration.GetSection(FeatureFlagKeys.FeatureFlagsKey));
            return serviceCollection;
        }
    }
}
