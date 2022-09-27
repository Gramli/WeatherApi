namespace Weather.Core.Abstractions
{
    public interface IRequestHandler<TResponse, in TRequest>
    {
        Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken);
    }
}
