namespace Weather.Infrastructure.Options
{
    public sealed class WeatherServiceRetryPolicyOptions
    {
        public static readonly string Key = "WeatherService:RetryPolicy";
        public int RetryCount { get; init; }
        public int Delay { get; init; }
    }
}
