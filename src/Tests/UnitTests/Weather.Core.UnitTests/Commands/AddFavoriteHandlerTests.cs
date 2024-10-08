﻿using SmallApiToolkit.Core.RequestHandlers;
using SmallApiToolkit.Core.Validation;
using Weather.Core.Abstractions;
using Weather.Core.Commands;
using Weather.Core.Resources;
using Weather.Domain.Commands;
using Weather.Domain.Logging;
using Weather.UnitTests.Common.Extensions;

namespace Weather.Core.UnitTests.Commands
{
    public class AddFavoriteHandlerTests
    {
        private readonly Mock<IWeatherCommandsRepository> _weatherCommandsRepositoryMock;
        private readonly Mock<IRequestValidator<AddFavoriteCommand>> _addFavoriteCommandValidatorMock;
        private readonly Mock<ILogger<AddFavoriteHandler>> _loggerMock;

        private readonly IHttpRequestHandler<int, AddFavoriteCommand> _uut;
        public AddFavoriteHandlerTests()
        {
            _weatherCommandsRepositoryMock = new();
            _addFavoriteCommandValidatorMock = new();
            _loggerMock = new();

            _uut = new AddFavoriteHandler(_weatherCommandsRepositoryMock.Object, _addFavoriteCommandValidatorMock.Object, _loggerMock.Object);
        }


        [Fact]
        public async Task InvalidLocation()
        {
            //Arrange
            var addFavoriteCommand = new AddFavoriteCommand { Location = new Domain.Dtos.LocationDto { Latitude = 1, Longitude = 1 } };

            _addFavoriteCommandValidatorMock.Setup(x => x.Validate(It.IsAny<AddFavoriteCommand>())).Returns(new RequestValidationResult { IsValid = false});

            //Act
            var result = await _uut.HandleAsync(addFavoriteCommand, CancellationToken.None);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Single(result.Errors);
            _addFavoriteCommandValidatorMock.Verify(x => x.Validate(It.Is<AddFavoriteCommand>(y => y.Equals(addFavoriteCommand))), Times.Once);
        }

        [Fact]
        public async Task AddFavoriteLocation_Failed()
        {
            //Arrange
            var addFavoriteCommand = new AddFavoriteCommand { Location = new Domain.Dtos.LocationDto { Latitude = 1, Longitude = 1 } };
            var errorMessage = "errorMessage";

            _addFavoriteCommandValidatorMock.Setup(x => x.Validate(It.IsAny<AddFavoriteCommand>())).Returns(new RequestValidationResult { IsValid = true });
            _weatherCommandsRepositoryMock.Setup(x => x.AddFavoriteLocation(It.IsAny<AddFavoriteCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Fail(errorMessage));

            //Act
            var result = await _uut.HandleAsync(addFavoriteCommand, CancellationToken.None);

            //Assert
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Single(result.Errors);
            Assert.Equal(ErrorMessages.CantStoreLocation, result.Errors.Single());
            _addFavoriteCommandValidatorMock.Verify(x => x.Validate(It.Is<AddFavoriteCommand>(y => y.Equals(addFavoriteCommand))), Times.Once);
            _weatherCommandsRepositoryMock.Verify(x => x.AddFavoriteLocation(It.Is<AddFavoriteCommand>(y=>y.Equals(addFavoriteCommand)), It.IsAny<CancellationToken>()), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Error, LogEvents.FavoriteWeathersStoreToDatabase, errorMessage, Times.Once());
        }

        [Fact]
        public async Task Success()
        {
            //Arrange
            var addFavoriteCommand = new AddFavoriteCommand { Location = new Domain.Dtos.LocationDto { Latitude = 1, Longitude = 1 } };
            var locationId = 1;

            _addFavoriteCommandValidatorMock.Setup(x => x.Validate(It.IsAny<AddFavoriteCommand>())).Returns(new RequestValidationResult { IsValid = true });
            _weatherCommandsRepositoryMock.Setup(x => x.AddFavoriteLocation(It.IsAny<AddFavoriteCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Ok(locationId));

            //Act
            var result = await _uut.HandleAsync(addFavoriteCommand, CancellationToken.None);

            //Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Empty(result.Errors);
            Assert.Equal(locationId, result.Data);
            _addFavoriteCommandValidatorMock.Verify(x => x.Validate(It.Is<AddFavoriteCommand>(y => y.Equals(addFavoriteCommand))), Times.Once);
            _weatherCommandsRepositoryMock.Verify(x => x.AddFavoriteLocation(It.Is<AddFavoriteCommand>(y => y.Equals(addFavoriteCommand)), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
