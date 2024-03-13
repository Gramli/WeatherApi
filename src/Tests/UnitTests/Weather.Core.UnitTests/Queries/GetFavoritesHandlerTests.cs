using Weather.Core.Abstractions;
using Weather.Core.Queries;
using Weather.Core.Resources;
using Weather.Domain.BusinessEntities;
using Weather.Domain.Dtos;
using Weather.Domain.Http;
using Weather.Domain.Logging;
using Weather.UnitTests.Common.Extensions;

namespace Weather.Core.UnitTests.Queries
{
    public class GetFavoritesHandlerTests
    {
        private readonly Mock<IWeatherQueriesRepository> _weatherRepositoryMock;
        private readonly Mock<IWeatherService> _weatherServiceMock;
        private readonly Mock<ILogger<IGetFavoritesHandler>> _loggerMock;
        private readonly Mock<IValidator<LocationDto>> _locationValidatorMock;
        private readonly Mock<IValidator<CurrentWeatherDto>> _currentWeatherValidatorMock;

        private readonly IGetFavoritesHandler _uut;
        public GetFavoritesHandlerTests() 
        { 
            _weatherRepositoryMock = new Mock<IWeatherQueriesRepository>();
            _weatherServiceMock = new Mock<IWeatherService>();
            _loggerMock = new Mock<ILogger<IGetFavoritesHandler>>();
            _locationValidatorMock = new Mock<IValidator<LocationDto>>();
            _currentWeatherValidatorMock = new Mock<IValidator<CurrentWeatherDto>>();

            _uut = new GetFavoritesHandler(_weatherRepositoryMock.Object, 
                _weatherServiceMock.Object, 
                _locationValidatorMock.Object, 
                _currentWeatherValidatorMock.Object,
                _loggerMock.Object);
        }

        [Fact]
        public async Task GetFavorites_Empty()
        {
            //Arrange
            _weatherRepositoryMock.Setup(x => x.GetFavorites(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<FavoriteLocation>());

            //Act
            var result = await _uut.HandleAsync(EmptyRequest.Instance, CancellationToken.None);

            //Assert
            Assert.Equal(HttpStatusCode.NoContent, result.StatusCode);
            Assert.Empty(result.Errors);
            _weatherRepositoryMock.Verify(x => x.GetFavorites(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task InvalidLocation()
        {
            //Arrange
            var locationDto = new FavoriteLocation { Id =0, Latitude = 1, Longitude = 1 };

            _weatherRepositoryMock.Setup(x => x.GetFavorites(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<FavoriteLocation>
                {
                    locationDto,
                });

            _locationValidatorMock.Setup(x => x.IsValid(It.IsAny<LocationDto>())).Returns(false);

            //Act
            var result = await _uut.HandleAsync(EmptyRequest.Instance, CancellationToken.None);

            //Assert
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Single(result.Errors);
            Assert.Null(result.Data);
            _weatherRepositoryMock.Verify(x => x.GetFavorites(It.IsAny<CancellationToken>()), Times.Once);
            _weatherServiceMock.Verify(x => x.GetCurrentWeather(It.Is<LocationDto>(y => y.Equals(locationDto)), It.IsAny<CancellationToken>()), Times.Never);
            _locationValidatorMock.Verify(x => x.IsValid(It.Is<LocationDto>(y => y.Equals(locationDto))), Times.Once);
        }

        [Fact]
        public async Task EmptyResult_GetCurrentWeather_Fail()
        {
            //Arrange
            var failMessage = "Some fail message";
            var locationDto = new FavoriteLocation { Id = 0, Latitude = 1, Longitude = 1 };

            _weatherRepositoryMock.Setup(x => x.GetFavorites(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<FavoriteLocation>
                {
                    locationDto,
                });

            _locationValidatorMock.Setup(x=>x.IsValid(It.IsAny<LocationDto>())).Returns(true);

            _weatherServiceMock.Setup(x => x.GetCurrentWeather(It.IsAny<LocationDto>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Fail(failMessage));
            //Act
            var result = await _uut.HandleAsync(EmptyRequest.Instance, CancellationToken.None);

            //Assert
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Single(result.Errors);
            _weatherRepositoryMock.Verify(x => x.GetFavorites(It.IsAny<CancellationToken>()), Times.Once);
            _weatherServiceMock.Verify(x => x.GetCurrentWeather(It.Is<LocationDto>(y=>y.Equals(locationDto)), It.IsAny<CancellationToken>()), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Warning, LogEvents.FavoriteWeathersGeneral, failMessage, Times.Once());
            _locationValidatorMock.Verify(x => x.IsValid(It.Is<LocationDto>(y=>y.Equals(locationDto))), Times.Once);
        }

        [Fact]
        public async Task One_Of_GetCurrentWeather_Failed()
        {
            //Arrange
            var failMessage = "Some fail message";
            var locationDto = new FavoriteLocation { Id = 0, Latitude = 1, Longitude = 1 };

            _weatherRepositoryMock.Setup(x => x.GetFavorites(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<FavoriteLocation>
                {
                    locationDto,
                    new FavoriteLocation(),
                });

            _locationValidatorMock.Setup(x => x.IsValid(It.IsAny<LocationDto>())).Returns(true);

            var currentWeather = new CurrentWeatherDto();

            _currentWeatherValidatorMock.Setup(x => x.IsValid(It.IsAny<CurrentWeatherDto>())).Returns(true);

            _weatherServiceMock.Setup(x => x.GetCurrentWeather(It.Is<FavoriteLocation>(y=> y.Equals(locationDto)), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Fail(failMessage));
            _weatherServiceMock.Setup(x => x.GetCurrentWeather(It.Is<FavoriteLocation>(y => !y.Equals(locationDto)), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Ok(currentWeather));
            //Act
            var result = await _uut.HandleAsync(EmptyRequest.Instance, CancellationToken.None);

            //Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Single(result.Errors);
            Assert.NotNull(result.Data);
            Assert.Single(result.Data.FavoriteWeathers);
            Assert.Equal(currentWeather.CityName, result.Data.FavoriteWeathers.Single().CityName);
            _weatherRepositoryMock.Verify(x => x.GetFavorites(It.IsAny<CancellationToken>()), Times.Once);
            _weatherServiceMock.Verify(x => x.GetCurrentWeather(It.IsAny<LocationDto>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
            _loggerMock.VerifyLog(LogLevel.Warning, LogEvents.FavoriteWeathersGeneral, failMessage, Times.Once());
            _locationValidatorMock.Verify(x => x.IsValid(It.Is<LocationDto>(y => y.Equals(locationDto))), Times.Once);
            _currentWeatherValidatorMock.Verify(x => x.IsValid(It.Is<CurrentWeatherDto>(y => y.Equals(currentWeather))), Times.Once);
        }

        [Fact]
        public async Task GetCurrentWeather_Validation_Fail()
        {
            //Arrange
            var locationDto = new FavoriteLocation { Id = 0, Latitude = 1, Longitude = 1 };

            _weatherRepositoryMock.Setup(x => x.GetFavorites(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<FavoriteLocation>
                {
                    locationDto,
                });

            _locationValidatorMock.Setup(x => x.IsValid(It.IsAny<LocationDto>())).Returns(true);
            _currentWeatherValidatorMock.Setup(x => x.IsValid(It.IsAny<CurrentWeatherDto>())).Returns(false);
            var currentWeather = new CurrentWeatherDto();

            _weatherServiceMock.Setup(x => x.GetCurrentWeather(It.IsAny<LocationDto>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Ok(currentWeather));
            //Act
            var result = await _uut.HandleAsync(EmptyRequest.Instance, CancellationToken.None);

            //Assert
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Single(result.Errors);
            Assert.Null(result.Data);
            _weatherRepositoryMock.Verify(x => x.GetFavorites(It.IsAny<CancellationToken>()), Times.Once);
            _weatherServiceMock.Verify(x => x.GetCurrentWeather(It.Is<LocationDto>(y => y.Equals(locationDto)), It.IsAny<CancellationToken>()), Times.Once);
            _locationValidatorMock.Verify(x => x.IsValid(It.Is<LocationDto>(y => y.Equals(locationDto))), Times.Once);
            _currentWeatherValidatorMock.Verify(x => x.IsValid(It.Is<CurrentWeatherDto>(y => y.Equals(currentWeather))), Times.Once);
        }

        [Fact]
        public async Task Success()
        {
            //Arrange
            var locationDto = new FavoriteLocation {  Latitude = 1, Longitude = 1 };

            _weatherRepositoryMock.Setup(x => x.GetFavorites(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<FavoriteLocation>
                {
                    locationDto,
                });

            _locationValidatorMock.Setup(x => x.IsValid(It.IsAny<LocationDto>())).Returns(true);
            _currentWeatherValidatorMock.Setup(x=>x.IsValid(It.IsAny<CurrentWeatherDto>())).Returns(true);
            var currentWeather = new CurrentWeatherDto();

            _weatherServiceMock.Setup(x => x.GetCurrentWeather(It.IsAny<LocationDto>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Ok(currentWeather));
            //Act
            var result = await _uut.HandleAsync(EmptyRequest.Instance, CancellationToken.None);

            //Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Empty(result.Errors);
            Assert.NotNull(result.Data);
            Assert.Single(result.Data.FavoriteWeathers);
            Assert.Equal(currentWeather.CityName, result.Data.FavoriteWeathers.Single().CityName);
            _weatherRepositoryMock.Verify(x => x.GetFavorites(It.IsAny<CancellationToken>()), Times.Once);
            _weatherServiceMock.Verify(x => x.GetCurrentWeather(It.Is<LocationDto>(y=>y.Equals(locationDto)), It.IsAny<CancellationToken>()), Times.Once);
            _locationValidatorMock.Verify(x => x.IsValid(It.Is<LocationDto>(y => y.Equals(locationDto))), Times.Once);
            _currentWeatherValidatorMock.Verify(x => x.IsValid(It.Is<CurrentWeatherDto>(y=>y.Equals(currentWeather))), Times.Once);
        }
    }
}
