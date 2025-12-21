using Weather.Core.HandlerModel;

namespace Weather.Core.Abstractions
{
    public interface ICoreRequestHandler<TResponse, in TRequest> : IRequestHandler<HandlerResponse<TResponse>, TRequest>
    {
    }
}
