using Moq;
using Validot;
using Wheaterbit.Client.Abstractions;
using Wheaterbit.Client.UnitTests.DataGenerator;
using Wheaterbit.Client.UnitTests.Extensions;

namespace Wheaterbit.Client.UnitTests
{
    public class WeatherbitHttpClientTests
    {
        private Mock<IHttpClientFactory>? _httpClientFactoryMock;
        private Mock<IJsonSerializerSettingsFactory>? _jsonSerializerSettingsFactoryMock;
        private Mock<IValidator<Options.WeatherbitOptions>>? _optionsValidatorMock;

        private IWeatherbitHttpClient? _uut;

        [Fact]
        public void InvalidOptions()
        {
            //Arrange
            var _customHttpMessageHandlerMock = new Mock<ICusomHttpMessageHandler>();
            _httpClientFactoryMock = new Mock<IHttpClientFactory>().Setup(_customHttpMessageHandlerMock);
            _jsonSerializerSettingsFactoryMock = new Mock<IJsonSerializerSettingsFactory>();
            _optionsValidatorMock = new Mock<IValidator<Options.WeatherbitOptions>>()
                .Setup(true);

            //Act & Assert
            Assert.Throws<ArgumentException>(() => new WeatherbitHttpClient(WeatherbitOptionsGenerator.CreateValidOptions(), _httpClientFactoryMock.Object, _optionsValidatorMock.Object, _jsonSerializerSettingsFactoryMock.Object));
        }

        [Fact]
        public async Task GetSixteenDayForecast_SendAsync_NotSuccessStatusCode()
        {
            //Arrange
            var _customHttpMessageHandlerMock = new Mock<ICusomHttpMessageHandler>();
            _httpClientFactoryMock = new Mock<IHttpClientFactory>().Setup(_customHttpMessageHandlerMock);
            _jsonSerializerSettingsFactoryMock = new Mock<IJsonSerializerSettingsFactory>();
            _optionsValidatorMock = new Mock<IValidator<Options.WeatherbitOptions>>()
                .Setup(false);

            _uut = new WeatherbitHttpClient(
                WeatherbitOptionsGenerator.CreateValidOptions(),
                _httpClientFactoryMock.Object,
                _optionsValidatorMock.Object,
                _jsonSerializerSettingsFactoryMock.Object);

            _uut = new WeatherbitHttpClient(WeatherbitOptionsGenerator.CreateValidOptions(), _httpClientFactoryMock.Object, _optionsValidatorMock.Object, _jsonSerializerSettingsFactoryMock.Object);

            _customHttpMessageHandlerMock
                .Setup(x => x.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.InternalServerError });
            //Act
            var result = await _uut.GetSixteenDayForecast(0,0,CancellationToken.None);

            //Assert
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task GetSixteenDayForecast_InvalidUri()
        {
            //Arrange
            var _customHttpMessageHandlerMock = new Mock<ICusomHttpMessageHandler>();
            _httpClientFactoryMock = new Mock<IHttpClientFactory>().Setup(_customHttpMessageHandlerMock);
            _jsonSerializerSettingsFactoryMock = new Mock<IJsonSerializerSettingsFactory>();
            _optionsValidatorMock = new Mock<IValidator<Options.WeatherbitOptions>>()
                .Setup(false);

            _uut = new WeatherbitHttpClient(
                WeatherbitOptionsGenerator.CreateInvalidOptions(),
                _httpClientFactoryMock.Object,
                _optionsValidatorMock.Object,
                _jsonSerializerSettingsFactoryMock.Object);

            //Act & Assert
            await Assert.ThrowsAsync<UriFormatException>(() => _uut.GetSixteenDayForecast(0, 0, CancellationToken.None));
        }


    }
}
