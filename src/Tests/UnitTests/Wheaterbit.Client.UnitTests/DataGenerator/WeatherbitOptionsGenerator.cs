namespace Wheaterbit.Client.UnitTests.DataGenerator
{
    internal static class WeatherbitOptionsGenerator
    {
        public static Microsoft.Extensions.Options.IOptions<Options.WeatherbitOptions> CreateInvalidOptions()
        {
            return Microsoft.Extensions.Options.Options.Create(new Options.WeatherbitOptions
            {
                BaseUrl = "baseUrl",
                XRapidAPIHost = "xRapidAPIHost",
                XRapidAPIKey = "xRapidAPIKey",
            });
        }

        public static Microsoft.Extensions.Options.IOptions<Options.WeatherbitOptions> CreateValidOptions()
        {
            return Microsoft.Extensions.Options.Options.Create(new Options.WeatherbitOptions
            {
                BaseUrl = "https://baseUrl",
                XRapidAPIHost = "xRapidAPIHost",
                XRapidAPIKey = "xRapidAPIKey",
            });
        }
    }
}
