using Weather.Domain.Commands;

namespace Weather.Core.Abstractions
{
    public interface IAddFavoriteHandler : IRequestHandler<int, AddFavoriteCommand>
    {

    }
}
