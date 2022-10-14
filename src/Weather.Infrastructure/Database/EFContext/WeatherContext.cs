using Microsoft.EntityFrameworkCore;
using Weather.Infrastructure.Database.EFContext.Entities;

namespace Weather.Infrastructure.Database.EFContext
{
    internal sealed class WeatherContext : DbContext
    {
        public WeatherContext(DbContextOptions<WeatherContext> options)
            : base(options) { }

        public DbSet<FavoriteLocationEntity> FavoriteLocations => Set<FavoriteLocationEntity>();
    }
}
