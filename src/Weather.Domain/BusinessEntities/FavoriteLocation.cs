using Weather.Domain.Dtos;

namespace Weather.Domain.BusinessEntities
{
    public sealed class FavoriteLocation : LocationDto
    {
        public int Id { get; init; }
    }
}
