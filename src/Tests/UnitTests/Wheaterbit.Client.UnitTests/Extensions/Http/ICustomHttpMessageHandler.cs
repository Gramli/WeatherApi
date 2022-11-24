namespace Wheaterbit.Client.UnitTests.Extensions.Http
{
    public interface ICusomHttpMessageHandler
    {
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken);
    }
}
