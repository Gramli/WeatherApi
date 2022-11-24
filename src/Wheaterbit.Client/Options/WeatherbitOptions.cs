namespace Wheaterbit.Client.Options
{
    public class WeatherbitOptions
    {
        public const string Weatherbit = "Weatherbit";

        public string BaseUrl { get; set; } = string.Empty;
        public string XRapidAPIKey { get; set; } = string.Empty;
        public string XRapidAPIHost { get; set; } = string.Empty;
    }
}
