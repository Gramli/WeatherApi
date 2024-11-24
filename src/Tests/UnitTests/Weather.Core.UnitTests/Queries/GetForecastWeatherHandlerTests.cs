using SmallApiToolkit.Core.RequestHandlers;
using SmallApiToolkit.Core.Validation;
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
        private readonly Mock<IRequestValidator<GetForecastWeatherQuery>> _getForecastWeatherQueryValidatorMock;
        private readonly Mock<IRequestValidator<ForecastWeatherDto>> _forecastWeatherValidatorMock;
        private readonly Mock<IWeatherService> _weatherServiceMock;
        private readonly Mock<ILogger<GetForecastWeatherHandler>> _loggerMock;

        private readonly IHttpRequestHandler<ForecastWeatherDto, GetForecastWeatherQuery> _uut;
        public GetForecastWeatherHandlerTests()
        {
            _getForecastWeatherQueryValidatorMock = new();
            _forecastWeatherValidatorMock = new();
            _weatherServiceMock = new();
            _loggerMock = new();

            _uut = new GetForecastWeatherHandler(
                _getForecastWeatherQueryValidatorMock.Object, 
                _weatherServiceMock.Object, 
                _forecastWeatherValidatorMock.Object, 
                _loggerMock.Object);
        }

        [Fact]
        public async Task InvalidLocation()
        {
            //Arrange
            var getForecastWeatherQuery = new GetForecastWeatherQuery(1, 1);

            _getForecastWeatherQueryValidatorMock.Setup(x => x.Validate(It.IsAny<GetForecastWeatherQuery>())).Returns(new RequestValidationResult { IsValid = false });

            //Act
            var result = await _uut.HandleAsync(getForecastWeatherQuery, CancellationToken.None);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Single(result.Errors);
            Assert.Null(result.Data);
            _getForecastWeatherQueryValidatorMock.Verify(x => x.Validate(It.Is<GetForecastWeatherQuery>(y => y.Equals(getForecastWeatherQuery))), Times.Once);
        }

        [Fact]
        public async Task GetForecastWeather_Failed()
        {
            //Arrange
            var errorMessage = "error";
            var getForecastWeatherQuery = new GetForecastWeatherQuery(1, 1);

            _getForecastWeatherQueryValidatorMock.Setup(x => x.Validate(It.IsAny<GetForecastWeatherQuery>())).Returns(new RequestValidationResult { IsValid = true });
            _weatherServiceMock.Setup(x => x.GetSixteenDayForecastWeather(It.IsAny<LocationDto>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Fail(errorMessage));
            //Act
            var result = await _uut.HandleAsync(getForecastWeatherQuery, CancellationToken.None);

            //Assert
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Single(result.Errors);
            Assert.Equal(ErrorMessages.ExternalApiError, result.Errors.Single());
            Assert.Null(result.Data);
            _getForecastWeatherQueryValidatorMock.Verify(x => x.Validate(It.Is<GetForecastWeatherQuery>(y => y.Equals(getForecastWeatherQuery))), Times.Once);
            _weatherServiceMock.Verify(x => x.GetSixteenDayForecastWeather(It.Is<LocationDto>(y => y.Equals(getForecastWeatherQuery.Location)), It.IsAny<CancellationToken>()), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Error, LogEvents.ForecastWeathersGet, errorMessage, Times.Once());
        }

        [Fact]
        public async Task GetForecastWeather_ValidationFailed()
        {
            //Arrange
            var getForecastWeatherQuery = new GetForecastWeatherQuery(1, 1);
            var forecastWeather = new ForecastWeatherDto();

            _getForecastWeatherQueryValidatorMock.Setup(x => x.Validate(It.IsAny<GetForecastWeatherQuery>())).Returns(new RequestValidationResult { IsValid = true });
            _weatherServiceMock.Setup(x => x.GetSixteenDayForecastWeather(It.IsAny<LocationDto>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Ok(forecastWeather));
            _forecastWeatherValidatorMock.Setup(x => x.Validate(It.IsAny<ForecastWeatherDto>())).Returns(new RequestValidationResult { IsValid = false });

            //Act
            var result = await _uut.HandleAsync(getForecastWeatherQuery, CancellationToken.None);

            //Assert
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Single(result.Errors);
            Assert.Null(result.Data);
            _getForecastWeatherQueryValidatorMock.Verify(x => x.Validate(It.Is<GetForecastWeatherQuery>(y => y.Equals(getForecastWeatherQuery))), Times.Once);
            _weatherServiceMock.Verify(x => x.GetSixteenDayForecastWeather(It.Is<LocationDto>(y => y.Equals(getForecastWeatherQuery.Location)), It.IsAny<CancellationToken>()), Times.Once);
            _forecastWeatherValidatorMock.Verify(x => x.Validate(It.Is<ForecastWeatherDto>(y => y.Equals(forecastWeather))), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Error, LogEvents.ForecastWeathersValidation, Times.Once());
        }

        [Fact]
        public async Task Success()
        {
            //Arrange
            var getForecastWeatherQuery = new GetForecastWeatherQuery(1, 1);
            var forecastWeather = new ForecastWeatherDto();

            _getForecastWeatherQueryValidatorMock.Setup(x => x.Validate(It.IsAny<GetForecastWeatherQuery>())).Returns(new RequestValidationResult { IsValid = true });
            _weatherServiceMock.Setup(x => x.GetSixteenDayForecastWeather(It.IsAny<LocationDto>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Ok(forecastWeather));
            _forecastWeatherValidatorMock.Setup(x => x.Validate(It.IsAny<ForecastWeatherDto>())).Returns(new RequestValidationResult { IsValid = true });

            //Act
            var result = await _uut.HandleAsync(getForecastWeatherQuery, CancellationToken.None);

            //Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Empty(result.Errors);
            Assert.NotNull(result.Data);
            Assert.Equal(forecastWeather, result.Data);
            _getForecastWeatherQueryValidatorMock.Verify(x => x.Validate(It.Is<GetForecastWeatherQuery>(y => y.Equals(getForecastWeatherQuery))), Times.Once);
            _weatherServiceMock.Verify(x => x.GetSixteenDayForecastWeather(It.Is<LocationDto>(y => y.Equals(getForecastWeatherQuery.Location)), It.IsAny<CancellationToken>()), Times.Once);
            _forecastWeatherValidatorMock.Verify(x => x.Validate(It.Is<ForecastWeatherDto>(y => y.Equals(forecastWeather))), Times.Once);
        }
    }
}
