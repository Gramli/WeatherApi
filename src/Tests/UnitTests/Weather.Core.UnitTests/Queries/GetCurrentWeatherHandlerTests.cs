using Weather.Core.Abstractions;
using Weather.Core.HandlerModel;
using Weather.Core.Queries;
using Weather.Core.Resources;
using Weather.Domain.Dtos;
using Weather.Domain.Logging;
using Weather.Domain.Queries;
using Weather.UnitTests.Common.Extensions;

namespace Weather.Core.UnitTests.Queries
{
    public class GetCurrentWeatherHandlerTests
    {
        private readonly Mock<IRequestValidator<GetCurrentWeatherQuery>> _getCurrentWeatherQueryValidatorMock;
        private readonly Mock<IRequestValidator<CurrentWeatherDto>> _currentWeatherValidatorMock;
        private readonly Mock<IWeatherService> _weatherServiceMock;
        private readonly Mock<ILogger<GetCurrentWeatherHandler>> _loggerMock;

        private readonly IStatusRequestHandler<CurrentWeatherDto, GetCurrentWeatherQuery> _uut;
        public GetCurrentWeatherHandlerTests()
        {
            _getCurrentWeatherQueryValidatorMock = new();
            _currentWeatherValidatorMock = new();
            _weatherServiceMock = new();
            _loggerMock = new();

            _uut = new GetCurrentWeatherHandler(_getCurrentWeatherQueryValidatorMock.Object, _currentWeatherValidatorMock.Object, _weatherServiceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task InvalidLocation()
        {
            //Arrange
            var getCurrentWeatherQuery = new GetCurrentWeatherQuery(1,1);

            _getCurrentWeatherQueryValidatorMock.Setup(x => x.Validate(It.IsAny<GetCurrentWeatherQuery>())).Returns(new RequestValidationResult { IsValid = false});

            //Act
            var result = await _uut.HandleAsync(getCurrentWeatherQuery, CancellationToken.None);

            //Assert
            Assert.Equal(HandlerStatusCode.ValidationError, result.StatusCode);
            Assert.Single(result.Errors);
            Assert.Null(result.Data);
            _getCurrentWeatherQueryValidatorMock.Verify(x => x.Validate(It.Is<GetCurrentWeatherQuery>(y => y.Equals(getCurrentWeatherQuery))), Times.Once);
        }

        [Fact]
        public async Task GetCurrentWeather_Failed()
        {
            //Arrange
            var errorMessage = "error";
            var getCurrentWeatherQuery = new GetCurrentWeatherQuery(1, 1);

            _getCurrentWeatherQueryValidatorMock.Setup(x => x.Validate(It.IsAny<GetCurrentWeatherQuery>())).Returns(new RequestValidationResult { IsValid = true });
            _weatherServiceMock.Setup(x=>x.GetCurrentWeather(It.IsAny<LocationDto>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Fail(errorMessage));
            //Act
            var result = await _uut.HandleAsync(getCurrentWeatherQuery, CancellationToken.None);

            //Assert
            Assert.Equal(HandlerStatusCode.InternalError, result.StatusCode);
            Assert.Single(result.Errors);
            Assert.Equal(ErrorMessages.ExternalApiError, result.Errors.Single());
            Assert.Null(result.Data);
            _getCurrentWeatherQueryValidatorMock.Verify(x => x.Validate(It.Is<GetCurrentWeatherQuery>(y => y.Equals(getCurrentWeatherQuery))), Times.Once);
            _weatherServiceMock.Verify(x => x.GetCurrentWeather(It.Is<LocationDto>(y => y.Equals(getCurrentWeatherQuery.Location)), It.IsAny<CancellationToken>()), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Error, LogEvents.CurrentWeathersGet, errorMessage, Times.Once());
        }

        [Fact]
        public async Task CurrentWeather_ValidationFailed()
        {
            //Arrange
            var getCurrentWeatherQuery = new GetCurrentWeatherQuery(1, 1);
            var currentWeather = new CurrentWeatherDto();

            _getCurrentWeatherQueryValidatorMock.Setup(x => x.Validate(It.IsAny<GetCurrentWeatherQuery>())).Returns(new RequestValidationResult { IsValid = true });
            _weatherServiceMock.Setup(x => x.GetCurrentWeather(It.IsAny<LocationDto>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Ok(currentWeather));
            _currentWeatherValidatorMock.Setup(x => x.Validate(It.IsAny<CurrentWeatherDto>())).Returns(new RequestValidationResult { IsValid = false});
            
            //Act
            var result = await _uut.HandleAsync(getCurrentWeatherQuery, CancellationToken.None);

            //Assert
            Assert.Equal(HandlerStatusCode.InternalError, result.StatusCode);
            Assert.Single(result.Errors);
            Assert.Null(result.Data);
            _getCurrentWeatherQueryValidatorMock.Verify(x => x.Validate(It.Is<GetCurrentWeatherQuery>(y => y.Equals(getCurrentWeatherQuery))), Times.Once);
            _weatherServiceMock.Verify(x => x.GetCurrentWeather(It.Is<LocationDto>(y => y.Equals(getCurrentWeatherQuery.Location)), It.IsAny<CancellationToken>()), Times.Once);
            _currentWeatherValidatorMock.Verify(x => x.Validate(It.Is<CurrentWeatherDto>(y=>y.Equals(currentWeather))), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Error, LogEvents.CurrentWeathersValidation, Times.Once());
        }

        [Fact]
        public async Task Success()
        {
            //Arrange
            var getCurrentWeatherQuery = new GetCurrentWeatherQuery(1, 1);
            var currentWeather = new CurrentWeatherDto();

            _getCurrentWeatherQueryValidatorMock.Setup(x => x.Validate(It.IsAny<GetCurrentWeatherQuery>())).Returns(new RequestValidationResult { IsValid = true });
            _weatherServiceMock.Setup(x => x.GetCurrentWeather(It.IsAny<LocationDto>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Ok(currentWeather));
            _currentWeatherValidatorMock.Setup(x => x.Validate(It.IsAny<CurrentWeatherDto>())).Returns(new RequestValidationResult { IsValid = true});

            //Act
            var result = await _uut.HandleAsync(getCurrentWeatherQuery, CancellationToken.None);

            //Assert
            Assert.Equal(HandlerStatusCode.Success, result.StatusCode);
            Assert.Empty(result.Errors);
            Assert.NotNull(result.Data);
            Assert.Equal(currentWeather, result.Data);
            _getCurrentWeatherQueryValidatorMock.Verify(x => x.Validate(It.Is<GetCurrentWeatherQuery>(y => y.Equals(getCurrentWeatherQuery))), Times.Once);
            _weatherServiceMock.Verify(x => x.GetCurrentWeather(It.Is<LocationDto>(y => y.Equals(getCurrentWeatherQuery.Location)), It.IsAny<CancellationToken>()), Times.Once);
            _currentWeatherValidatorMock.Verify(x => x.Validate(It.Is<CurrentWeatherDto>(y => y.Equals(currentWeather))), Times.Once);
        }
    }
}
