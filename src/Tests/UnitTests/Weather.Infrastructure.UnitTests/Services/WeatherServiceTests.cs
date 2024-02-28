using Weather.Core.Abstractions;
using Weather.Domain.Dtos;
using Weather.Infrastructure.Resources;
using Weather.Infrastructure.Services;
using Wheaterbit.Client.Abstractions;
using Wheaterbit.Client.Dtos;

namespace Weather.Infrastructure.UnitTests.Services
{
    public class WeatherServiceTests
    {
        private readonly Mock<IWeatherbitHttpClient> _weatherbiClientMock;
        private readonly Mock<IMapper> _mapperMock;

        private readonly IWeatherService _uut;
        public WeatherServiceTests()
        {
            _weatherbiClientMock = new Mock<IWeatherbitHttpClient>();
            _mapperMock = new Mock<IMapper>();

            _uut = new WeatherService(_weatherbiClientMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetCurrentWeather_Failed()
        {
            //Arrange
            var location = new LocationDto { Latitude = 15, Longitude = 25 };
            var failedMessage = "message";

            _weatherbiClientMock.Setup(x=>x.GetCurrentWeather(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Fail(failedMessage));
            //Act
            var result = await _uut.GetCurrentWeather(location, CancellationToken.None);
            //Assert
            Assert.True(result.IsFailed);
            Assert.Single(result.Errors);
            Assert.Equal(failedMessage, result.Errors.First().Message);
            _weatherbiClientMock.Verify(x => x.GetCurrentWeather(It.Is<double>(y=>y.Equals(location.Latitude)), It.Is<double>(y => y.Equals(location.Longitude)), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetCurrentWeather_NullData()
        {
            //Arrange
            var location = new LocationDto { Latitude = 15, Longitude = 25 };

            _weatherbiClientMock.Setup(x => x.GetCurrentWeather(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Ok((CurrentWeatherDataDto)null));
            //Act
            var result = await _uut.GetCurrentWeather(location, CancellationToken.None);
            //Assert
            Assert.True(result.IsFailed);
            Assert.Single(result.Errors);
            Assert.Equal(ErrorMessages.ExternalClientGetDataFailed_EmptyOrNull, result.Errors.First().Message);
            _weatherbiClientMock.Verify(x => x.GetCurrentWeather(It.Is<double>(y => y.Equals(location.Latitude)), It.Is<double>(y => y.Equals(location.Longitude)), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetCurrentWeather_EmptyData()
        {
            //Arrange
            var location = new LocationDto { Latitude = 15, Longitude = 25 };

            _weatherbiClientMock.Setup(x => x.GetCurrentWeather(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Ok(new CurrentWeatherDataDto()));
            //Act
            var result = await _uut.GetCurrentWeather(location, CancellationToken.None);
            //Assert
            Assert.True(result.IsFailed);
            Assert.Single(result.Errors);
            Assert.Contains(ErrorMessages.ExternalClientGetDataFailed_EmptyOrNull, result.Errors.First().Message);
            _weatherbiClientMock.Verify(x => x.GetCurrentWeather(It.Is<double>(y => y.Equals(location.Latitude)), It.Is<double>(y => y.Equals(location.Longitude)), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetCurrentWeather_ManyData()
        {
            //Arrange
            var location = new LocationDto { Latitude = 15, Longitude = 25 };
            var data = new List<Wheaterbit.Client.Dtos.CurrentWeatherDto>
            {
                new Wheaterbit.Client.Dtos.CurrentWeatherDto(),
                new Wheaterbit.Client.Dtos.CurrentWeatherDto()
            };

            _weatherbiClientMock.Setup(x => x.GetCurrentWeather(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Ok(new CurrentWeatherDataDto
            { 
                Data = data
            }));
            //Act
            var result = await _uut.GetCurrentWeather(location, CancellationToken.None);
            //Assert
            Assert.True(result.IsFailed);
            Assert.Single(result.Errors);
            Assert.Contains(string.Format(ErrorMessages.ExternalClientGetDataFailed_CorruptedData_InvalidCount, 2), result.Errors.First().Message);
            _weatherbiClientMock.Verify(x => x.GetCurrentWeather(It.Is<double>(y => y.Equals(location.Latitude)), It.Is<double>(y => y.Equals(location.Longitude)), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetCurrentWeather_Success()
        {
            //Arrange
            var location = new LocationDto { Latitude = 15, Longitude = 25 };
            var data = new List<Wheaterbit.Client.Dtos.CurrentWeatherDto>
            {
                new Wheaterbit.Client.Dtos.CurrentWeatherDto(),
            };

            var mapResult = new Domain.Dtos.CurrentWeatherDto();

            _weatherbiClientMock.Setup(x => x.GetCurrentWeather(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Ok(new CurrentWeatherDataDto
            {
                Data = data
            }));

            _mapperMock.Setup(x => x.Map<Domain.Dtos.CurrentWeatherDto>(It.IsAny<Wheaterbit.Client.Dtos.CurrentWeatherDto>())).Returns(mapResult);

            //Act
            var result = await _uut.GetCurrentWeather(location, CancellationToken.None);

            //Assert
            Assert.True(result.IsSuccess);
            Assert.Empty(result.Errors);
            _weatherbiClientMock.Verify(x => x.GetCurrentWeather(It.Is<double>(y => y.Equals(location.Latitude)), It.Is<double>(y => y.Equals(location.Longitude)), It.IsAny<CancellationToken>()), Times.Once);
            Assert.Equivalent(mapResult, result.Value);
            _mapperMock.Verify(x => x.Map<Domain.Dtos.CurrentWeatherDto>(It.IsAny<Wheaterbit.Client.Dtos.CurrentWeatherDto>()), Times.Once);
        }

        [Fact]
        public async Task GetForecastWeather_Failed()
        {
            //Arrange
            var location = new LocationDto { Latitude = 15, Longitude = 25 };
            var failedMessage = "message";

            _weatherbiClientMock.Setup(x => x.GetSixteenDayForecast(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Fail(failedMessage));
            //Act
            var result = await _uut.GetForecastWeather(location, CancellationToken.None);
            //Assert
            Assert.True(result.IsFailed);
            Assert.Single(result.Errors);
            Assert.Equal(failedMessage, result.Errors.First().Message);
            _weatherbiClientMock.Verify(x => x.GetSixteenDayForecast(It.Is<double>(y => y.Equals(location.Latitude)), It.Is<double>(y => y.Equals(location.Longitude)), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetForecastWeather_NullData()
        {
            //Arrange
            var location = new LocationDto { Latitude = 15, Longitude = 25 };

            _weatherbiClientMock.Setup(x => x.GetSixteenDayForecast(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Ok((Wheaterbit.Client.Dtos.ForecastWeatherDto)null));
            //Act
            var result = await _uut.GetForecastWeather(location, CancellationToken.None);
            //Assert
            Assert.True(result.IsFailed);
            Assert.Single(result.Errors);
            Assert.Equal(ErrorMessages.ExternalClientGetDataFailed_EmptyOrNull, result.Errors.First().Message);
            _weatherbiClientMock.Verify(x => x.GetSixteenDayForecast(It.Is<double>(y => y.Equals(location.Latitude)), It.Is<double>(y => y.Equals(location.Longitude)), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetForecastWeather_EmptyData()
        {
            //Arrange
            var location = new LocationDto { Latitude = 15, Longitude = 25 };

            _weatherbiClientMock.Setup(x => x.GetSixteenDayForecast(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Ok(new Wheaterbit.Client.Dtos.ForecastWeatherDto()));
            //Act
            var result = await _uut.GetForecastWeather(location, CancellationToken.None);
            //Assert
            Assert.True(result.IsFailed);
            Assert.Single(result.Errors);
            Assert.Contains(ErrorMessages.ExternalClientGetDataFailed_EmptyOrNull, result.Errors.First().Message);
            _weatherbiClientMock.Verify(x => x.GetSixteenDayForecast(It.Is<double>(y => y.Equals(location.Latitude)), It.Is<double>(y => y.Equals(location.Longitude)), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetForecastWeather_Success()
        {
            //Arrange
            var location = new LocationDto { Latitude = 15, Longitude = 25 };
            var data = new Wheaterbit.Client.Dtos.ForecastWeatherDto
            {
                Data = new List<Wheaterbit.Client.Dtos.ForecastTemperatureDto>
                {
                    new Wheaterbit.Client.Dtos.ForecastTemperatureDto()
                }
            };

            var mapResult = new Domain.Dtos.ForecastWeatherDto();

            _weatherbiClientMock.Setup(x => x.GetSixteenDayForecast(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Ok(data));

            _mapperMock.Setup(x => x.Map<Domain.Dtos.ForecastWeatherDto>(It.IsAny<Wheaterbit.Client.Dtos.ForecastWeatherDto>())).Returns(mapResult);

            //Act
            var result = await _uut.GetForecastWeather(location, CancellationToken.None);

            //Assert
            Assert.True(result.IsSuccess);
            Assert.Empty(result.Errors);
            _weatherbiClientMock.Verify(x => x.GetSixteenDayForecast(It.Is<double>(y => y.Equals(location.Latitude)), It.Is<double>(y => y.Equals(location.Longitude)), It.IsAny<CancellationToken>()), Times.Once);
            Assert.Equivalent(mapResult, result.Value);
            _mapperMock.Verify(x => x.Map<Domain.Dtos.ForecastWeatherDto>(It.IsAny<Wheaterbit.Client.Dtos.ForecastWeatherDto>()), Times.Once);
        }
    }
}
