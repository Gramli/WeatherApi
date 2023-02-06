using Validot;
using Wheaterbit.Client.Abstractions;
using Wheaterbit.Client.UnitTests.DataGenerator;
using Wheaterbit.Client.UnitTests.Extensions;
using Wheaterbit.Client.UnitTests.Extensions.Http;

namespace Wheaterbit.Client.UnitTests
{
    public class WeatherbitHttpClientTests
    {
        [Fact]
        public void InvalidOptions()
        {
            //Arrange
            var _customHttpMessageHandlerMock = new Mock<ICusomHttpMessageHandler>();
            var _httpClientFactoryMock = new Mock<IHttpClientFactory>().Setup(_customHttpMessageHandlerMock);
            var _jsonSerializerSettingsFactoryMock = new Mock<IJsonSerializerSettingsFactory>();
            var _optionsValidatorMock = new Mock<IValidator<Options.WeatherbitOptions>>()
                .Setup(true);

            //Act & Assert
            Assert.Throws<ArgumentException>(() => new WeatherbitHttpClient(WeatherbitOptionsGenerator.CreateValidOptions(), _httpClientFactoryMock.Object, _optionsValidatorMock.Object, _jsonSerializerSettingsFactoryMock.Object));
        }

        [Fact]
        public async Task GetSixteenDayForecast_SendAsync_NotSuccessStatusCode()
        {
            //Arrange
            var _customHttpMessageHandlerMock = new Mock<ICusomHttpMessageHandler>();
            var _httpClientFactoryMock = new Mock<IHttpClientFactory>().Setup(_customHttpMessageHandlerMock);
            var _jsonSerializerSettingsFactoryMock = new Mock<IJsonSerializerSettingsFactory>();
            var _optionsValidatorMock = new Mock<IValidator<Options.WeatherbitOptions>>()
                .Setup(false);

            var _uut = new WeatherbitHttpClient(
                WeatherbitOptionsGenerator.CreateValidOptions(),
                _httpClientFactoryMock.Object,
                _optionsValidatorMock.Object,
                _jsonSerializerSettingsFactoryMock.Object);

            _customHttpMessageHandlerMock
                .Setup(x => x.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.InternalServerError });
            //Act
            var result = await _uut.GetSixteenDayForecast(0,0,CancellationToken.None);

            //Assert
            Assert.False(result.IsSuccess);
            Assert.Single(result.Errors);
            Assert.Equal("Failed response to SendAsync", result.Errors.Single().Message);
        }

        [Fact]
        public async Task GetSixteenDayForecast_InvalidUri()
        {
            //Arrange
            var _customHttpMessageHandlerMock = new Mock<ICusomHttpMessageHandler>();
            var _httpClientFactoryMock = new Mock<IHttpClientFactory>().Setup(_customHttpMessageHandlerMock);
            var _jsonSerializerSettingsFactoryMock = new Mock<IJsonSerializerSettingsFactory>();
            var _optionsValidatorMock = new Mock<IValidator<Options.WeatherbitOptions>>()
                .Setup(false);

            var _uut = new WeatherbitHttpClient(
                WeatherbitOptionsGenerator.CreateInvalidOptions(),
                _httpClientFactoryMock.Object,
                _optionsValidatorMock.Object,
                _jsonSerializerSettingsFactoryMock.Object);

            //Act & Assert
            await Assert.ThrowsAsync<UriFormatException>(() => _uut.GetSixteenDayForecast(0, 0, CancellationToken.None));
        }

        [Fact]
        public async Task GetSixteenDayForecast_SendAsync_ThrowException()
        {
            //Arrange
            var _customHttpMessageHandlerMock = new Mock<ICusomHttpMessageHandler>();
            var _httpClientFactoryMock = new Mock<IHttpClientFactory>().Setup(_customHttpMessageHandlerMock);
            var _jsonSerializerSettingsFactoryMock = new Mock<IJsonSerializerSettingsFactory>();
            var _optionsValidatorMock = new Mock<IValidator<Options.WeatherbitOptions>>()
                .Setup(false);

            var sendAsyncException = new ArgumentOutOfRangeException("Exception message");

            _customHttpMessageHandlerMock
                .Setup(x => x.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(sendAsyncException);

            var _uut = new WeatherbitHttpClient(
                WeatherbitOptionsGenerator.CreateValidOptions(),
                _httpClientFactoryMock.Object,
                _optionsValidatorMock.Object,
                _jsonSerializerSettingsFactoryMock.Object);
            //Act
            var result = await _uut.GetSixteenDayForecast(0, 0, CancellationToken.None);

            //Assert
            Assert.False(result.IsSuccess);
            Assert.Single(result.Errors);
            Assert.Equal(sendAsyncException.Message, result.Errors.Single().Message);
        }

        [Fact]
        public async Task GetSixteenDayForecast_SendAsync_DeserializeObject_Failed()
        {
            //Arrange
            var _customHttpMessageHandlerMock = new Mock<ICusomHttpMessageHandler>();
            var _httpClientFactoryMock = new Mock<IHttpClientFactory>().Setup(_customHttpMessageHandlerMock);
            var _jsonSerializerSettingsFactoryMock = new Mock<IJsonSerializerSettingsFactory>();
            var _optionsValidatorMock = new Mock<IValidator<Options.WeatherbitOptions>>()
                .Setup(false);

            var _uut = new WeatherbitHttpClient(
                WeatherbitOptionsGenerator.CreateValidOptions(),
                _httpClientFactoryMock.Object,
                _optionsValidatorMock.Object,
                _jsonSerializerSettingsFactoryMock.Object);

            _customHttpMessageHandlerMock
                .Setup(x => x.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.OK, Content = new StringContent("") });
            //Act
            var result = await _uut.GetSixteenDayForecast(0, 0, CancellationToken.None);

            //Assert
            Assert.False(result.IsSuccess);
            Assert.Single(result.Errors);
            Assert.Equal("Failed to deserialize response.", result.Errors.Single().Message);
        }


    }
}
