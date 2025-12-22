using Weather.Core.HandlerModel;

namespace Weather.Core.Abstractions
{
    public interface IStatusRequestHandler<TResponse, in TRequest> : IRequestHandler<HandlerResponse<TResponse>, TRequest>
    {
    }
}
