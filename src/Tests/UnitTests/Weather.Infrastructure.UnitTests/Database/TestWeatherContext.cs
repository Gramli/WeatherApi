using Microsoft.EntityFrameworkCore;
using Weather.Infrastructure.Database.EFContext;
using Weather.Infrastructure.Database.EFContext.Entities;

namespace Weather.Infrastructure.UnitTests.Database
{
    public class TestWeatherContext : WeatherContext
    {
        public TestWeatherContext()
            : base(new DbContextOptions<WeatherContext>())
        {
        }

        public override DbSet<FavoriteLocationEntity>? FavoriteLocations { get; }
    }
}
