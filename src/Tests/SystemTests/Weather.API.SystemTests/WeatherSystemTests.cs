using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Text;
using Weather.Domain.Dtos;
using Weather.Domain.Dtos.Commands;

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
            var response = await _httpClient.GetAsync($"weather/current?latitude={latitude}&longtitude={longtitude}");

            //Assert
            response.EnsureSuccessStatusCode();
            var stringResult = await response.Content.ReadAsStringAsync();
            var resultDto = JsonConvert.DeserializeObject<CurrentWeatherDto>(stringResult);
            Assert.NotNull(resultDto);
            Assert.Equal(cityName, resultDto.CityName);
        }

        [Fact]
        public async Task GetForecastWeather()
        {
            //Arrange
            //Act
            var response = await _httpClient.GetAsync("weather/forecast?latitude=1&longtitude=1");

            //Assert
            response.EnsureSuccessStatusCode();
            var stringResult = await response.Content.ReadAsStringAsync();
            var resultDto = JsonConvert.DeserializeObject<ForecastWeatherDto>(stringResult);
            Assert.NotNull(resultDto);
            Assert.Equal(cityName, resultDto.CityName);
        }

        [Fact]
        public async Task PostWeatherFavorites()
        {
            //Act
            var response = await AddFavorite();

            //Assert
            response.EnsureSuccessStatusCode();
            var stringResult = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<bool>(stringResult);
            Assert.True(result);
        }

        [Fact]
        public async Task GetWeatherFavorites()
        {
            //Arrange
            var addResponse = await AddFavorite();

            addResponse.EnsureSuccessStatusCode();
            //Act
            var response = await _httpClient.GetAsync("weather/favorites");

            //Assert
            response.EnsureSuccessStatusCode();
            var stringResult = await response.Content.ReadAsStringAsync();
            var resultDto = JsonConvert.DeserializeObject<FavoritesWeatherDto>(stringResult);
            Assert.NotNull(resultDto);
            Assert.Equal(cityName, resultDto.FavoriteWeathers.First().CityName);
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
            return await _httpClient.PostAsync("weather/favorite", content);
        }
    }
}
