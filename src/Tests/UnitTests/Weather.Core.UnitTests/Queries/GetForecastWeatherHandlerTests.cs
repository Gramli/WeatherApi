using Validot.Results;
using Weather.Core.Abstractions;
using Weather.Core.Queries;
using Weather.Core.Resources;
using Weather.Domain.Dtos;
using Weather.Domain.Logging;
using Weather.Domain.Queries;
using Weather.UnitTests.Common.Extensions;

namespace Weather.Core.UnitTests.Queries
{
    public class GetForecastWeatherHandlerTests
    {
        private readonly Mock<IValidator<GetForecastWeatherQuery>> _getForecastWeatherQueryValidatorMock;
        private readonly Mock<IValidator<ForecastWeatherDto>> _forecastWeatherValidatorMock;
        private readonly Mock<IWeatherService> _weatherServiceMock;
        private readonly Mock<ILogger<IGetForecastWeatherHandler>> _loggerMock;

        private readonly IGetForecastWeatherHandler _uut;
        public GetForecastWeatherHandlerTests()
        {
            _getForecastWeatherQueryValidatorMock = new Mock<IValidator<GetForecastWeatherQuery>>();
            _forecastWeatherValidatorMock = new Mock<IValidator<ForecastWeatherDto>>();
            _weatherServiceMock = new Mock<IWeatherService>();
            _loggerMock = new Mock<ILogger<IGetForecastWeatherHandler>>();

            _uut = new GetForecastWeatherHandler(_getForecastWeatherQueryValidatorMock.Object, _weatherServiceMock.Object, _forecastWeatherValidatorMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task InvalidLocation()
        {
            //Arrange
            var getForecastWeatherQuery = new GetForecastWeatherQuery(1, 1);

            _getForecastWeatherQueryValidatorMock.Setup(x => x.IsValid(It.IsAny<GetForecastWeatherQuery>())).Returns(false);

            //Act
            var result = await _uut.HandleAsync(getForecastWeatherQuery, CancellationToken.None);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Single(result.Errors);
            Assert.Null(result.Data);
            _getForecastWeatherQueryValidatorMock.Verify(x => x.IsValid(It.Is<GetForecastWeatherQuery>(y => y.Equals(getForecastWeatherQuery))), Times.Once);
        }

        [Fact]
        public async Task GetForecastWeather_Failed()
        {
            //Arrange
            var errorMessage = "error";
            var getForecastWeatherQuery = new GetForecastWeatherQuery(1, 1);

            _getForecastWeatherQueryValidatorMock.Setup(x => x.IsValid(It.IsAny<GetForecastWeatherQuery>())).Returns(true);
            _weatherServiceMock.Setup(x => x.GetForecastWeather(It.IsAny<LocationDto>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Fail(errorMessage));
            //Act
            var result = await _uut.HandleAsync(getForecastWeatherQuery, CancellationToken.None);

            //Assert
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Single(result.Errors);
            Assert.Equal(ErrorMessages.ExternalApiError, result.Errors.Single());
            Assert.Null(result.Data);
            _getForecastWeatherQueryValidatorMock.Verify(x => x.IsValid(It.Is<GetForecastWeatherQuery>(y => y.Equals(getForecastWeatherQuery))), Times.Once);
            _weatherServiceMock.Verify(x => x.GetForecastWeather(It.Is<LocationDto>(y => y.Equals(getForecastWeatherQuery.Location)), It.IsAny<CancellationToken>()), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Error, LogEvents.ForecastWeathersGet, errorMessage, Times.Once());
        }

        [Fact]
        public async Task GetForecastWeather_ValidationFailed()
        {
            //Arrange
            var getForecastWeatherQuery = new GetForecastWeatherQuery(1, 1);
            var forecastWeather = new ForecastWeatherDto();

            var validationResutlMock = new Mock<IValidationResult>();
            validationResutlMock.SetupGet(x => x.AnyErrors).Returns(true);
            _getForecastWeatherQueryValidatorMock.Setup(x => x.IsValid(It.IsAny<GetForecastWeatherQuery>())).Returns(true);
            _weatherServiceMock.Setup(x => x.GetForecastWeather(It.IsAny<LocationDto>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Ok(forecastWeather));
            _forecastWeatherValidatorMock.Setup(x => x.Validate(It.IsAny<ForecastWeatherDto>(), It.IsAny<bool>())).Returns(validationResutlMock.Object);

            //Act
            var result = await _uut.HandleAsync(getForecastWeatherQuery, CancellationToken.None);

            //Assert
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Single(result.Errors);
            Assert.Null(result.Data);
            _getForecastWeatherQueryValidatorMock.Verify(x => x.IsValid(It.Is<GetForecastWeatherQuery>(y => y.Equals(getForecastWeatherQuery))), Times.Once);
            _weatherServiceMock.Verify(x => x.GetForecastWeather(It.Is<LocationDto>(y => y.Equals(getForecastWeatherQuery.Location)), It.IsAny<CancellationToken>()), Times.Once);
            _forecastWeatherValidatorMock.Verify(x => x.Validate(It.Is<ForecastWeatherDto>(y => y.Equals(forecastWeather)), It.Is<bool>(y => !y)), Times.Once);
            validationResutlMock.VerifyGet(x => x.AnyErrors, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Error, LogEvents.ForecastWeathersValidation, Times.Once());
        }

        [Fact]
        public async Task Success()
        {
            //Arrange
            var getForecastWeatherQuery = new GetForecastWeatherQuery(1, 1);
            var forecastWeather = new ForecastWeatherDto();

            var validationResutlMock = new Mock<IValidationResult>();
            validationResutlMock.SetupGet(x => x.AnyErrors).Returns(false);
            _getForecastWeatherQueryValidatorMock.Setup(x => x.IsValid(It.IsAny<GetForecastWeatherQuery>())).Returns(true);
            _weatherServiceMock.Setup(x => x.GetForecastWeather(It.IsAny<LocationDto>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Ok(forecastWeather));
            _forecastWeatherValidatorMock.Setup(x => x.Validate(It.IsAny<ForecastWeatherDto>(), It.IsAny<bool>())).Returns(validationResutlMock.Object);

            //Act
            var result = await _uut.HandleAsync(getForecastWeatherQuery, CancellationToken.None);

            //Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Empty(result.Errors);
            Assert.NotNull(result.Data);
            Assert.Equal(forecastWeather, result.Data);
            _getForecastWeatherQueryValidatorMock.Verify(x => x.IsValid(It.Is<GetForecastWeatherQuery>(y => y.Equals(getForecastWeatherQuery))), Times.Once);
            _weatherServiceMock.Verify(x => x.GetForecastWeather(It.Is<LocationDto>(y => y.Equals(getForecastWeatherQuery.Location)), It.IsAny<CancellationToken>()), Times.Once);
            _forecastWeatherValidatorMock.Verify(x => x.Validate(It.Is<ForecastWeatherDto>(y => y.Equals(forecastWeather)), It.Is<bool>(y => !y)), Times.Once);
            validationResutlMock.VerifyGet(x => x.AnyErrors, Times.Once);
        }
    }
}
