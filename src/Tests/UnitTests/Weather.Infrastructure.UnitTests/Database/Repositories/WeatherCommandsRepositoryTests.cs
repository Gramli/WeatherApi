using Microsoft.EntityFrameworkCore;
using Weather.Core.Abstractions;
using Weather.Domain.Dtos;
using Weather.Domain.Dtos.Commands;
using Weather.Infrastructure.Database.EFContext.Entities;
using Weather.Infrastructure.Database.Repositories;

namespace Weather.Infrastructure.UnitTests.Database.Repositories
{
    public class WeatherCommandsRepositoryTests
    {
        private readonly Mock<DbSet<FavoriteLocationEntity>> _favoriteLocationEntityDbSetMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<TestWeatherContext> _weatherDbContextMock;

        private readonly IWeatherCommandsRepository _uut;

        public WeatherCommandsRepositoryTests()
        {
            _favoriteLocationEntityDbSetMock = new Mock<DbSet<FavoriteLocationEntity>>();
            _weatherDbContextMock = new Mock<TestWeatherContext>();
            _weatherDbContextMock.Setup(x => x.FavoriteLocations).Returns(_favoriteLocationEntityDbSetMock.Object);

            _mapperMock = new Mock<IMapper>();

            _uut = new WeatherCommandsRepository(_weatherDbContextMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task AddFavoriteLocation_Success()
        {
            //Arrange
            var addFacoriteCommand = new AddFavoriteCommand { Location = new LocationDto { Latitude = 1, Longitude = 1 } };
            var favoriteLocationEntity = new FavoriteLocationEntity();

            _mapperMock.Setup(x => x.Map<FavoriteLocationEntity>(It.IsAny<LocationDto>())).Returns(favoriteLocationEntity);

            //Act
            var result = await _uut.AddFavoriteLocation(addFacoriteCommand, CancellationToken.None);

            //Assert
            Assert.True(result.IsSuccess);
            _mapperMock.Verify(x => x.Map<FavoriteLocationEntity>(It.IsAny<LocationDto>()), Times.Once);
            _weatherDbContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            _favoriteLocationEntityDbSetMock.Verify(x => x.AddAsync(It.IsAny<FavoriteLocationEntity>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task AddFavoriteLocation_Throw()
        {
            //Arrange
            var addFacoriteCommand = new AddFavoriteCommand { Location = new LocationDto { Latitude = 1, Longitude = 1 } };
            var favoriteLocationEntity = new FavoriteLocationEntity();
            var exception = new ArgumentException("some message");

            _mapperMock.Setup(x => x.Map<FavoriteLocationEntity>(It.IsAny<LocationDto>())).Returns(favoriteLocationEntity);
            _favoriteLocationEntityDbSetMock.Setup(x => x.AddAsync(It.IsAny<FavoriteLocationEntity>(), It.IsAny<CancellationToken>())).Throws(exception);

            //Act
            var exceptionResult = await Assert.ThrowsAsync<ArgumentException>(() => _uut.AddFavoriteLocation(addFacoriteCommand, CancellationToken.None));

            //Assert
            Assert.Equivalent(exception, exceptionResult);
            _mapperMock.Verify(x => x.Map<FavoriteLocationEntity>(It.IsAny<LocationDto>()), Times.Once);
            _weatherDbContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
            _favoriteLocationEntityDbSetMock.Verify(x => x.AddAsync(It.IsAny<FavoriteLocationEntity>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
