using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Weather.Core.Abstractions;
using Weather.Domain.Commands;
using Weather.Domain.Dtos;
using Weather.Domain.Logging;
using Weather.Infrastructure.Database.EFContext.Entities;
using Weather.Infrastructure.Database.Repositories;
using Weather.UnitTests.Common.Extensions;

namespace Weather.Infrastructure.UnitTests.Database.Repositories
{
    public class WeatherCommandsRepositoryTests
    {
        private readonly Mock<DbSet<FavoriteLocationEntity>> _favoriteLocationEntityDbSetMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<TestWeatherContext> _weatherDbContextMock;
        private readonly Mock<ILogger<IWeatherCommandsRepository>> _loggerMock;

        private readonly IWeatherCommandsRepository _uut;

        public WeatherCommandsRepositoryTests()
        {
            _favoriteLocationEntityDbSetMock = new Mock<DbSet<FavoriteLocationEntity>>();
            _weatherDbContextMock = new Mock<TestWeatherContext>();
            _weatherDbContextMock.Setup(x => x.FavoriteLocations).Returns(_favoriteLocationEntityDbSetMock.Object);

            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<IWeatherCommandsRepository>>();

            _uut = new WeatherCommandsRepository(_weatherDbContextMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task AddFavoriteLocation_Success()
        {
            //Arrange
            var addFavoriteCommand = new AddFavoriteCommand { Location = new LocationDto { Latitude = 1, Longitude = 1 } };
            var favoriteLocationEntity = new FavoriteLocationEntity();

            _mapperMock.Setup(x => x.Map<FavoriteLocationEntity>(It.IsAny<LocationDto>())).Returns(favoriteLocationEntity);

            //Act
            var result = await _uut.AddFavoriteLocation(addFavoriteCommand, CancellationToken.None);

            //Assert
            Assert.True(result.IsSuccess);
            _mapperMock.Verify(x => x.Map<FavoriteLocationEntity>(It.IsAny<LocationDto>()), Times.Once);
            _weatherDbContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            _favoriteLocationEntityDbSetMock.Verify(x => x.AddAsync(It.IsAny<FavoriteLocationEntity>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task AddFavoriteLocation_Failed()
        {
            //Arrange
            var addFavoriteCommand = new AddFavoriteCommand { Location = new LocationDto { Latitude = 1, Longitude = 1 } };
            var favoriteLocationEntity = new FavoriteLocationEntity();

            _mapperMock.Setup(x => x.Map<FavoriteLocationEntity>(It.IsAny<LocationDto>())).Returns(favoriteLocationEntity);
            _favoriteLocationEntityDbSetMock.Setup(x => x.AddAsync(It.IsAny<FavoriteLocationEntity>(), It.IsAny<CancellationToken>())).Throws(new DbUpdateException());

            //Act
            var result = await _uut.AddFavoriteLocation(addFavoriteCommand, CancellationToken.None);

            //Assert
            Assert.True(result.IsFailed);
            _loggerMock.VerifyLog(LogLevel.Error, LogEvents.FavoriteWeathersStoreToDatabase, Times.Once());
            _mapperMock.Verify(x => x.Map<FavoriteLocationEntity>(It.IsAny<LocationDto>()), Times.Once);
            _weatherDbContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
            _favoriteLocationEntityDbSetMock.Verify(x => x.AddAsync(It.IsAny<FavoriteLocationEntity>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task AddFavoriteLocation_Throw()
        {
            //Arrange
            var addFavoriteCommand = new AddFavoriteCommand { Location = new LocationDto { Latitude = 1, Longitude = 1 } };
            var favoriteLocationEntity = new FavoriteLocationEntity();
            var exception = new ArgumentException("some message");

            _mapperMock.Setup(x => x.Map<FavoriteLocationEntity>(It.IsAny<LocationDto>())).Returns(favoriteLocationEntity);
            _favoriteLocationEntityDbSetMock.Setup(x => x.AddAsync(It.IsAny<FavoriteLocationEntity>(), It.IsAny<CancellationToken>())).Throws(exception);

            //Act
            var exceptionResult = await Assert.ThrowsAsync<ArgumentException>(() => _uut.AddFavoriteLocation(addFavoriteCommand, CancellationToken.None));

            //Assert
            Assert.Equivalent(exception, exceptionResult);
            _mapperMock.Verify(x => x.Map<FavoriteLocationEntity>(It.IsAny<LocationDto>()), Times.Once);
            _weatherDbContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
            _favoriteLocationEntityDbSetMock.Verify(x => x.AddAsync(It.IsAny<FavoriteLocationEntity>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteFavoriteLocationSafeAsync_Success()
        {
            //Arrange
            var deleteFavoriteCommand = new DeleteFavoriteCommand { Id = 1 };
            var favoriteLocationEntity = new FavoriteLocationEntity();
            _favoriteLocationEntityDbSetMock.Setup(x => x.FindAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(favoriteLocationEntity);

            //Act
            var result = await _uut.DeleteFavoriteLocationSafeAsync(deleteFavoriteCommand, CancellationToken.None);

            //Assert
            Assert.True(result.IsSuccess);
            _weatherDbContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            _favoriteLocationEntityDbSetMock.Verify(x => x.FindAsync(deleteFavoriteCommand.Id, CancellationToken.None), Times.Once);
            _favoriteLocationEntityDbSetMock.Verify(x => x.Remove(favoriteLocationEntity), Times.Once);
        }

        [Fact]
        public async Task DeleteFavoriteLocationSafeAsync_Failed()
        {
            //Arrange
            var deleteFavoriteCommand = new DeleteFavoriteCommand { Id = 1 };
            var favoriteLocationEntity = new FavoriteLocationEntity();
            _favoriteLocationEntityDbSetMock.Setup(x => x.FindAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(favoriteLocationEntity);
            _weatherDbContextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new DbUpdateException());

            //Act
            var result = await _uut.DeleteFavoriteLocationSafeAsync(deleteFavoriteCommand, CancellationToken.None);

            //Assert
            Assert.True(result.IsFailed);
            _weatherDbContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            _favoriteLocationEntityDbSetMock.Verify(x => x.FindAsync(deleteFavoriteCommand.Id, CancellationToken.None), Times.Once);
            _favoriteLocationEntityDbSetMock.Verify(x => x.Remove(favoriteLocationEntity), Times.Once);
        }

        [Fact]
        public async Task DeleteFavoriteLocationSafeAsync_Throw()
        {
            //Arrange
            var deleteFavoriteCommand = new DeleteFavoriteCommand { Id = 1 };
            var favoriteLocationEntity = new FavoriteLocationEntity();
            _favoriteLocationEntityDbSetMock.Setup(x => x.FindAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(favoriteLocationEntity);

            _weatherDbContextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ArgumentException());

            //Act
            await Assert.ThrowsAsync<ArgumentException>(() => _uut.DeleteFavoriteLocationSafeAsync(deleteFavoriteCommand, CancellationToken.None));

            //Assert
            _weatherDbContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            _favoriteLocationEntityDbSetMock.Verify(x => x.FindAsync(deleteFavoriteCommand.Id, CancellationToken.None), Times.Once);
            _favoriteLocationEntityDbSetMock.Verify(x => x.Remove(favoriteLocationEntity), Times.Once);
        }
    }
}
