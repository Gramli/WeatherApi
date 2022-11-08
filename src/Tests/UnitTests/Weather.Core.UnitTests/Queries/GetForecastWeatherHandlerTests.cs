using Microsoft.Extensions.Logging;
using Validot;
using Weather.Core.Abstractions;
using Weather.Core.Queries;
using Weather.Domain.Dtos;

namespace Weather.Core.UnitTests.Queries
{
    public class GetForecastWeatherHandlerTests
    {
        private readonly Mock<IValidator<LocationDto>> _locationValidatorMock;
        private readonly Mock<IValidator<ForecastWeatherDto>> _forecastWeatherValidatorMock;
        private readonly Mock<IWeatherService> _weatherServiceMock;
        private readonly Mock<ILogger<IGetCurrentWeatherHandler>> _loggerMock;

        private readonly IGetForecastWeatherHandler _uut;
        public GetForecastWeatherHandlerTests()
        {
            _locationValidatorMock = new Mock<IValidator<LocationDto>>();
            _forecastWeatherValidatorMock = new Mock<IValidator<ForecastWeatherDto>>();
            _weatherServiceMock = new Mock<IWeatherService>();
            _loggerMock = new Mock<ILogger<IGetCurrentWeatherHandler>>();

            _uut = new GetForecastWeatherHandler(_locationValidatorMock.Object, _weatherServiceMock.Object, _forecastWeatherValidatorMock.Object, _loggerMock.Object);
        }
    }
}
