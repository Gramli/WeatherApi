﻿using FluentResults;
using Microsoft.Extensions.Logging;
using System.Net;
using Validot;
using Weather.Core.Abstractions;
using Weather.Core.Queries;
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
        public async Task GetFavorites_Failed()
        {
            //Arrange
            var failMessage = "Some fail message";

            _weatherRepositoryMock.Setup(x => x.GetFavorites(It.IsAny<CancellationToken>())).ReturnsAsync(Result.Fail(failMessage));

            //Act
            var result = await _uut.HandleAsync(EmptyRequest.Instance, CancellationToken.None);

            //Assert
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Single(result.Errors);
            Assert.Equal(failMessage, result.Errors.Single());
            _weatherRepositoryMock.Verify(x => x.GetFavorites(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetFavorites_Empty()
        {
            //Arrange
            _weatherRepositoryMock.Setup(x => x.GetFavorites(It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Ok<IEnumerable<LocationDto>>(new List<LocationDto>()));

            //Act
            var result = await _uut.HandleAsync(EmptyRequest.Instance, CancellationToken.None);

            //Assert
            Assert.Equal(HttpStatusCode.NoContent, result.StatusCode);
            Assert.Empty(result.Errors);
            _weatherRepositoryMock.Verify(x => x.GetFavorites(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Result_Empty()
        {
            //Arrange
            var failMessage = "Some fail message";
            var locationDto = new LocationDto() { Latitude = 1, Longitude = 1 };

            _weatherRepositoryMock.Setup(x => x.GetFavorites(It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Ok<IEnumerable<LocationDto>>(new List<LocationDto>() 
                {
                    locationDto
                }));

            _weatherServiceMock.Setup(x => x.GetCurrentWeather(It.IsAny<LocationDto>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Fail(failMessage));
            //Act
            var result = await _uut.HandleAsync(EmptyRequest.Instance, CancellationToken.None);

            //Assert
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Single(result.Errors);
            _weatherRepositoryMock.Verify(x => x.GetFavorites(It.IsAny<CancellationToken>()), Times.Once);
            _weatherServiceMock.Verify(x => x.GetCurrentWeather(It.Is<LocationDto>(y=>y.Equals(locationDto)), It.IsAny<CancellationToken>()), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Warning, LogEvents.FavoriteWeathersGeneral, failMessage, Times.Once());
        }

        [Fact]
        public async Task One_Of_GetCurrentWeather_Failed()
        {
            //Arrange
            var failMessage = "Some fail message";
            var locationDto = new LocationDto() { Latitude = 1, Longitude = 1 };

            _weatherRepositoryMock.Setup(x => x.GetFavorites(It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Ok<IEnumerable<LocationDto>>(new List<LocationDto>()
                {
                    locationDto,
                    new LocationDto() {},
                }));

            var currentWeather = new CurrentWeatherDto();

            _weatherServiceMock.Setup(x => x.GetCurrentWeather(It.Is<LocationDto>(y=> y.Equals(locationDto)), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Fail(failMessage));
            _weatherServiceMock.Setup(x => x.GetCurrentWeather(It.Is<LocationDto>(y => !y.Equals(locationDto)), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Ok(currentWeather));
            //Act
            var result = await _uut.HandleAsync(EmptyRequest.Instance, CancellationToken.None);

            //Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Single(result.Errors);
            Assert.NotNull(result.Data);
            Assert.Single(result.Data.FavoriteWeathers);
            Assert.Equal(currentWeather, result.Data.FavoriteWeathers.Single());
            _weatherRepositoryMock.Verify(x => x.GetFavorites(It.IsAny<CancellationToken>()), Times.Once);
            _weatherServiceMock.Verify(x => x.GetCurrentWeather(It.IsAny<LocationDto>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
            _loggerMock.VerifyLog(LogLevel.Warning, LogEvents.FavoriteWeathersGeneral, failMessage, Times.Once());
        }

        [Fact]
        public async Task Success()
        {
            //Arrange
            var locationDto = new LocationDto() { Latitude = 1, Longitude = 1 };

            _weatherRepositoryMock.Setup(x => x.GetFavorites(It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Ok<IEnumerable<LocationDto>>(new List<LocationDto>()
                {
                    locationDto,
                }));

            var currentWeather = new CurrentWeatherDto();

            _weatherServiceMock.Setup(x => x.GetCurrentWeather(It.IsAny<LocationDto>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Ok(currentWeather));
            //Act
            var result = await _uut.HandleAsync(EmptyRequest.Instance, CancellationToken.None);

            //Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Empty(result.Errors);
            Assert.NotNull(result.Data);
            Assert.Single(result.Data.FavoriteWeathers);
            Assert.Equal(currentWeather, result.Data.FavoriteWeathers.Single());
            _weatherRepositoryMock.Verify(x => x.GetFavorites(It.IsAny<CancellationToken>()), Times.Once);
            _weatherServiceMock.Verify(x => x.GetCurrentWeather(It.Is<LocationDto>(y=>y.Equals(locationDto)), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}