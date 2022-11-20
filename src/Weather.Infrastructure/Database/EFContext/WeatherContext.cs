using Microsoft.EntityFrameworkCore;
using Weather.Infrastructure.Database.EFContext.Entities;

namespace Weather.Infrastructure.Database.EFContext
{
    public class WeatherContext : DbContext
    {
        public WeatherContext(DbContextOptions<WeatherContext> options)
            : base(options) { }

        public virtual DbSet<FavoriteLocationEntity> FavoriteLocations => Set<FavoriteLocationEntity>();
    }
}
