using Weather.Domain.Commands;

namespace Weather.Core.Abstractions
{
    public interface IDeleteFavoriteHandler : IRequestHandler<bool, DeleteFavoriteCommand>
    {
    }
}
