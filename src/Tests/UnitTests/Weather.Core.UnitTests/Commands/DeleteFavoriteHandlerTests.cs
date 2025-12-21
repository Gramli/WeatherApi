using Weather.Core.Abstractions;
using Weather.Core.Commands;
using Weather.Core.HandlerModel;
using Weather.Domain.Commands;

namespace Weather.Core.UnitTests.Commands
{
    public class DeleteFavoriteHandlerTests
    {
        private readonly Mock<IWeatherCommandsRepository> _weatherCommandsRepositoryMock;
        private readonly Mock<IRequestValidator<DeleteFavoriteCommand>> _validatorMock;

        private readonly ICoreRequestHandler<bool, DeleteFavoriteCommand> _uut;
        public DeleteFavoriteHandlerTests()
        {
            _weatherCommandsRepositoryMock = new();
            _validatorMock = new();

            _uut = new DeleteFavoriteHandler(_weatherCommandsRepositoryMock.Object, _validatorMock.Object);
        }

        [Fact]
        public async Task InvalidRequest()
        {
            //Arrange
            var deleteFavoriteCommand = new DeleteFavoriteCommand { Id = 5 };

            _validatorMock.Setup(x => x.Validate(It.IsAny<DeleteFavoriteCommand>())).Returns(new RequestValidationResult { IsValid = false});

            //Act
            var result = await _uut.HandleAsync(deleteFavoriteCommand, CancellationToken.None);

            //Assert
            Assert.Equal(HandlerStatusCode.ValidationError, result.StatusCode);
            Assert.Single(result.Errors);
            _validatorMock.Verify(x => x.Validate(It.Is<DeleteFavoriteCommand>(y => y.Equals(deleteFavoriteCommand))), Times.Once);
        }

        [Fact]
        public async Task DeleteFavoriteLocationSafeAsync_Failed()
        {
            //Arrange
            var deleteFavoriteCommand = new DeleteFavoriteCommand { Id = 5 };
            
            _validatorMock.Setup(x => x.Validate(It.IsAny<DeleteFavoriteCommand>())).Returns(new RequestValidationResult { IsValid = true });
            _weatherCommandsRepositoryMock.Setup(x => x.DeleteFavoriteLocationSafeAsync(deleteFavoriteCommand, CancellationToken.None))
                .ReturnsAsync(Result.Fail(string.Empty));

            //Act
            var result = await _uut.HandleAsync(deleteFavoriteCommand, CancellationToken.None);

            //Assert
            Assert.Equal(HandlerStatusCode.InternalError, result.StatusCode);
            Assert.Single(result.Errors);
            _validatorMock.Verify(x => x.Validate(deleteFavoriteCommand), Times.Once);
            _weatherCommandsRepositoryMock.Verify(x => x.DeleteFavoriteLocationSafeAsync(deleteFavoriteCommand, CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task DeleteFavoriteLocationSafeAsync_Success()
        {
            //Arrange
            var deleteFavoriteCommand = new DeleteFavoriteCommand { Id = 5 };

            _validatorMock.Setup(x => x.Validate(It.IsAny<DeleteFavoriteCommand>())).Returns(new RequestValidationResult { IsValid = true });
            _weatherCommandsRepositoryMock.Setup(x => x.DeleteFavoriteLocationSafeAsync(deleteFavoriteCommand, CancellationToken.None))
                .ReturnsAsync(Result.Ok());

            //Act
            var result = await _uut.HandleAsync(deleteFavoriteCommand, CancellationToken.None);

            //Assert
            Assert.Equal(HandlerStatusCode.Success, result.StatusCode);
            Assert.Empty(result.Errors);
            _validatorMock.Verify(x => x.Validate(deleteFavoriteCommand), Times.Once);
            _weatherCommandsRepositoryMock.Verify(x => x.DeleteFavoriteLocationSafeAsync(deleteFavoriteCommand, CancellationToken.None), Times.Once);
        }
    }
}
