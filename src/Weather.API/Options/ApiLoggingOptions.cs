namespace Weather.API.Options
{
    public sealed class ApiLoggingOptions
    {
        public static string Key = "Logging:Api";
        public bool LogRequest { get; init; }
        public bool LogResponse { get; init; }
    }
}
