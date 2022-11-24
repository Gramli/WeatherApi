namespace Wheaterbit.Client.UnitTests.Extensions
{
    internal class CustomHttpClientProxy : HttpClient
    {
        private readonly HttpClient _httpClient;
        public CustomHttpClientProxy(HttpMessageHandler messageHandler)
        {
            _httpClient = new HttpClient(messageHandler);
        }
    }
}
