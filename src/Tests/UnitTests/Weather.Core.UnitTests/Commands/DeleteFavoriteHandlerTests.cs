using Weather.Core.Abstractions;
using Weather.Core.Commands;
using Weather.Domain.Commands;

namespace Weather.Core.UnitTests.Commands
{
    public class DeleteFavoriteHandlerTests
    {
        private readonly Mock<IWeatherCommandsRepository> _weatherCommandsRepositoryMock;
        private readonly Mock<IValidator<DeleteFavoriteCommand>> _validatorMock;

        private readonly IDeleteFavoriteHandler _uut;
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

            _validatorMock.Setup(x => x.IsValid(It.IsAny<DeleteFavoriteCommand>())).Returns(false);

            //Act
            var result = await _uut.HandleAsync(deleteFavoriteCommand, CancellationToken.None);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Single(result.Errors);
            _validatorMock.Verify(x => x.IsValid(It.Is<DeleteFavoriteCommand>(y => y.Equals(deleteFavoriteCommand))), Times.Once);
        }

        [Fact]
        public async Task DeleteFavoriteLocationSafeAsync_Failed()
        {
            //Arrange
            var deleteFavoriteCommand = new DeleteFavoriteCommand { Id = 5 };

            _validatorMock.Setup(x => x.IsValid(deleteFavoriteCommand)).Returns(true);
            _weatherCommandsRepositoryMock.Setup(x => x.DeleteFavoriteLocationSafeAsync(deleteFavoriteCommand, CancellationToken.None))
                .ReturnsAsync(Result.Fail(string.Empty));

            //Act
            var result = await _uut.HandleAsync(deleteFavoriteCommand, CancellationToken.None);

            //Assert
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Single(result.Errors);
            _validatorMock.Verify(x => x.IsValid(deleteFavoriteCommand), Times.Once);
            _weatherCommandsRepositoryMock.Verify(x => x.DeleteFavoriteLocationSafeAsync(deleteFavoriteCommand, CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task DeleteFavoriteLocationSafeAsync_Success()
        {
            //Arrange
            var deleteFavoriteCommand = new DeleteFavoriteCommand { Id = 5 };

            _validatorMock.Setup(x => x.IsValid(deleteFavoriteCommand)).Returns(true);
            _weatherCommandsRepositoryMock.Setup(x => x.DeleteFavoriteLocationSafeAsync(deleteFavoriteCommand, CancellationToken.None))
                .ReturnsAsync(Result.Ok());

            //Act
            var result = await _uut.HandleAsync(deleteFavoriteCommand, CancellationToken.None);

            //Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Empty(result.Errors);
            _validatorMock.Verify(x => x.IsValid(deleteFavoriteCommand), Times.Once);
            _weatherCommandsRepositoryMock.Verify(x => x.DeleteFavoriteLocationSafeAsync(deleteFavoriteCommand, CancellationToken.None), Times.Once);
        }
    }
}
