using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Text;
using Weather.Domain.Commands;
using Weather.Domain.Dtos;
using Weather.Domain.Http;

namespace Weather.API.SystemTests
{
    public class WeatherSystemTests
    {
        private readonly long latitude = 1;
        private readonly long longtitude = 1;
        private readonly string cityName = "Mumford";

        private readonly HttpClient _httpClient;

        public WeatherSystemTests()
        {
            var application = new WebApplicationFactory<Program>();
            _httpClient = application.CreateClient();
        }

        [Fact]
        public async Task GetCurrentWeather()
        {
            //Arrange
            //Act
            var response = await _httpClient.GetAsync($"weather/v1/current?latitude={latitude}&longtitude={longtitude}");

            //Assert
            response.EnsureSuccessStatusCode();
            var stringResult = await response.Content.ReadAsStringAsync();
            var resultDto = JsonConvert.DeserializeObject<DataResponse<CurrentWeatherDto>>(stringResult);
            Assert.NotNull(resultDto?.Data);
            Assert.Equal(cityName, resultDto.Data.CityName);
        }

        [Fact]
        public async Task GetForecastWeather()
        {
            //Arrange
            //Act
            var response = await _httpClient.GetAsync("weather/v1/forecast?latitude=1&longtitude=1");

            //Assert
            response.EnsureSuccessStatusCode();
            var stringResult = await response.Content.ReadAsStringAsync();
            var resultDto = JsonConvert.DeserializeObject<DataResponse<ForecastWeatherDto>>(stringResult);
            Assert.NotNull(resultDto?.Data);
            Assert.Equal(cityName, resultDto.Data.CityName);
        }

        [Fact]
        public async Task PostWeatherFavorites()
        {
            //Act
            var response = await AddFavorite();

            //Assert
            response.EnsureSuccessStatusCode();
            var stringResult = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<DataResponse<bool>>(stringResult);
            Assert.True(result?.Data);
        }

        [Fact]
        public async Task GetWeatherFavorites()
        {
            //Arrange
            var addResponse = await AddFavorite();

            addResponse.EnsureSuccessStatusCode();
            //Act
            var response = await _httpClient.GetAsync("weather/v1/favorites");

            //Assert
            response.EnsureSuccessStatusCode();
            var stringResult = await response.Content.ReadAsStringAsync();
            var resultDto = JsonConvert.DeserializeObject<DataResponse<FavoritesWeatherDto>>(stringResult);
            Assert.NotNull(resultDto?.Data);
            Assert.Equal(cityName, resultDto.Data.FavoriteWeathers.First().CityName);
        }

        private async Task<HttpResponseMessage> AddFavorite()
        {
            //Arrange
            var body = JsonConvert.SerializeObject(new AddFavoriteCommand
            {
                Location = new LocationDto
                {
                    Latitude = latitude,
                    Longitude = longtitude,
                }
            });
            var content = new StringContent(body, Encoding.UTF8, "application/json");

            //Act
            return await _httpClient.PostAsync("weather/v1/favorite", content);
        }
    }
}
