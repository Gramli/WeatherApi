namespace Wheaterbit.Client.UnitTests.Extensions.Http
{
    internal class CustomHttpClientProxy : HttpClient
    {
        public CustomHttpClientProxy(HttpMessageHandler messageHandler)
            : base(messageHandler) { }
    }
}
