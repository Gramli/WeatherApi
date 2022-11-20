namespace Weather.Infrastructure.Database.EFContext.Entities
{
    public sealed class FavoriteLocationEntity
    {
        public int Id { get; set; }
        public long Latitude { get; set; }
        public long Longitude { get; set; }
    }
}
