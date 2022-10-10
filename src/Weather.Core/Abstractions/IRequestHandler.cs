using Weather.Domain.Http;

namespace Weather.Core.Abstractions
{
    public interface IRequestHandler<TResponse, in TRequest> 
    {
        Task<HttpDataResponse<TResponse>> HandleAsync(TRequest request, CancellationToken cancellationToken);
    }
}
