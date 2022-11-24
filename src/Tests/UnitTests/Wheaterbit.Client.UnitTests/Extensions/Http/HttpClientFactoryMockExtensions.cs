using Moq;

namespace Wheaterbit.Client.UnitTests.Extensions.Http
{
    internal static class HttpClientFactoryMockExtensions
    {
        internal static Mock<IHttpClientFactory> Setup(this Mock<IHttpClientFactory> httpClientFactoryMock, Mock<ICusomHttpMessageHandler> customHttpMessageHandlerMock)
        {
            httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(new CustomHttpClientProxy(new CustomHttpMessageHandler(customHttpMessageHandlerMock.Object)));
            return httpClientFactoryMock;
        }
    }
}
