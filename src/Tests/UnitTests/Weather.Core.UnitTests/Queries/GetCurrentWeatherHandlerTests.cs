using Microsoft.Extensions.Logging;
using Validot;
using Weather.Core.Abstractions;
using Weather.Core.Queries;
using Weather.Domain.Dtos;

namespace Weather.Core.UnitTests.Queries
{
    public class GetCurrentWeatherHandlerTests
    {
        private readonly Mock<IValidator<LocationDto>> _locationValidatorMock;
        private readonly Mock<IValidator<CurrentWeatherDto>> _currentWeatherValidatorMock;
        private readonly Mock<IWeatherService> _weatherServiceMock;
        private readonly Mock<ILogger<IGetCurrentWeatherHandler>> _loggerMock;

        private readonly IGetCurrentWeatherHandler _uut;
        public GetCurrentWeatherHandlerTests()
        {
            _locationValidatorMock = new Mock<IValidator<LocationDto>>();
            _currentWeatherValidatorMock = new Mock<IValidator<CurrentWeatherDto>>();
            _weatherServiceMock = new Mock<IWeatherService>();
            _loggerMock = new Mock<ILogger<IGetCurrentWeatherHandler>>();

            _uut = new GetCurrentWeatherHandler(_locationValidatorMock.Object, _currentWeatherValidatorMock.Object, _weatherServiceMock.Object, _loggerMock.Object);
        }
    }
}
