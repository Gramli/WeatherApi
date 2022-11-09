using Weather.Domain.Dtos.Commands;

namespace Weather.Core.Abstractions
{
    public interface IAddFavoriteHandler : IRequestHandler<bool, AddFavoriteCommand>
    {

    }
}
