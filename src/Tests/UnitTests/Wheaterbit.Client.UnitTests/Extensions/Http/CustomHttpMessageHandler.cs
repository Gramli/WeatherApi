using Ardalis.GuardClauses;

namespace Wheaterbit.Client.UnitTests.Extensions.Http
{
    internal class CustomHttpMessageHandler : HttpMessageHandler
    {
        private readonly ICusomHttpMessageHandler _cusomHttpMessageHandler;
        public CustomHttpMessageHandler(ICusomHttpMessageHandler cusomHttpMessageHandler)
        {
            _cusomHttpMessageHandler = Guard.Against.Null(cusomHttpMessageHandler);
        }
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return _cusomHttpMessageHandler.SendAsync(request, cancellationToken);
        }
    }
}
