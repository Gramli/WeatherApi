using Weather.Domain.Dtos;

namespace Weather.Core.Abstractions
{
    public interface IAddFavoriteHandler : IRequestHandler<bool, LocationDto>
    {
    }
}
