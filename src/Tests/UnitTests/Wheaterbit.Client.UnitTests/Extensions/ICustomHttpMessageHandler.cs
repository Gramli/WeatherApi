namespace Wheaterbit.Client.UnitTests.Extensions
{
    public interface ICusomHttpMessageHandler
    {
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken);
    }
}
