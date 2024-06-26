using SmallApiToolkit.Core.RequestHandlers;
using Weather.Domain.Commands;

namespace Weather.Core.Abstractions
{
    public interface IDeleteFavoriteHandler : IHttpRequestHandler<bool, DeleteFavoriteCommand>
    {
    }
}
