using FluentResults;
using Microsoft.Extensions.Logging;
using System.Net;
using Validot;
using Weather.Core.Abstractions;
using Weather.Core.Commands;
using Weather.Domain.Dtos;
using Weather.Domain.Logging;
using Weather.UnitTests.Common.Extensions;

namespace Weather.Core.UnitTests.Commands
{
    public class AddFavoriteHandlerTests
    {
        private readonly Mock<IWeatherCommandsRepository> _weatherCommandsRepositoryMock;
        private readonly Mock<IValidator<LocationDto>> _locationValidatorMock;
        private readonly Mock<ILogger<IAddFavoriteHandler>> _loggerMock;

        private readonly IAddFavoriteHandler _uut;
        public AddFavoriteHandlerTests()
        {
            _weatherCommandsRepositoryMock = new Mock<IWeatherCommandsRepository>();
            _locationValidatorMock = new Mock<IValidator<LocationDto>>();
            _loggerMock = new Mock<ILogger<IAddFavoriteHandler>>();

            _uut = new AddFavoriteHandler(_weatherCommandsRepositoryMock.Object, _locationValidatorMock.Object, _loggerMock.Object);
        }


        [Fact]
        public async Task InvalidLocation()
        {
            //Arrange
            var locationDto = new LocationDto() { Latitude = 1, Longitude = 1 };

            _locationValidatorMock.Setup(x => x.IsValid(It.IsAny<LocationDto>())).Returns(false);

            //Act
            var result = await _uut.HandleAsync(locationDto, CancellationToken.None);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Single(result.Errors);
            Assert.False(result.Data);
            _locationValidatorMock.Verify(x => x.IsValid(It.Is<LocationDto>(y => y.Equals(locationDto))), Times.Once);
        }

        [Fact]
        public async Task AddFavoriteLocation_Failed()
        {
            //Arrange
            var locationDto = new LocationDto() { Latitude = 1, Longitude = 1 };
            var errorMessage = "errorMessage";

            _locationValidatorMock.Setup(x => x.IsValid(It.IsAny<LocationDto>())).Returns(true);
            _weatherCommandsRepositoryMock.Setup(x => x.AddFavoriteLocation(It.IsAny<LocationDto>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Fail(errorMessage));

            //Act
            var result = await _uut.HandleAsync(locationDto, CancellationToken.None);

            //Assert
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Single(result.Errors);
            Assert.False(result.Data);
            Assert.Equal(ErrorMessages.CantStoreLocation, result.Errors.Single());
            _locationValidatorMock.Verify(x => x.IsValid(It.Is<LocationDto>(y => y.Equals(locationDto))), Times.Once);
            _weatherCommandsRepositoryMock.Verify(x => x.AddFavoriteLocation(It.Is<LocationDto>(y=>y.Equals(locationDto)), It.IsAny<CancellationToken>()), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Error, LogEvents.FavoriteWeathersStoreToDatabase, errorMessage, Times.Once());
        }

        [Fact]
        public async Task Success()
        {
            //Arrange
            var locationDto = new LocationDto() { Latitude = 1, Longitude = 1 };
            var locationId = 1;

            _locationValidatorMock.Setup(x => x.IsValid(It.IsAny<LocationDto>())).Returns(true);
            _weatherCommandsRepositoryMock.Setup(x => x.AddFavoriteLocation(It.IsAny<LocationDto>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Ok(locationId));

            //Act
            var result = await _uut.HandleAsync(locationDto, CancellationToken.None);

            //Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Empty(result.Errors);
            Assert.True(result.Data);
            _locationValidatorMock.Verify(x => x.IsValid(It.Is<LocationDto>(y => y.Equals(locationDto))), Times.Once);
            _weatherCommandsRepositoryMock.Verify(x => x.AddFavoriteLocation(It.Is<LocationDto>(y => y.Equals(locationDto)), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
