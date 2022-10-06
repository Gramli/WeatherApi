using Weather.Domain;

namespace Weather.Core.Abstractions
{
    public interface IRequestHandler<TResponse, in TRequest> 
    {
        Task<DataResponse<TResponse>> HandleAsync(TRequest request, CancellationToken cancellationToken);
    }
}
