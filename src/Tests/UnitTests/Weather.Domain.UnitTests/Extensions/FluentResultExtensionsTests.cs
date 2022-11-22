using FluentResults;
using Weather.Domain.Extensions;

namespace Weather.Domain.UnitTests.Extensions
{
    public class FluentResultExtensionsTests
    {
        [Fact]
        public void ToErrorMessages_NullValues()
        {
            //Arrange
            var data = (IList<IError>)null;
            //Act
            var result = data.ToErrorMessages();
            //Assert
            Assert.Empty(result);
        }

        [Fact]
        public void ToErrorMessages_EmptyValues()
        {
            //Arrange
            var data = new List<IError>();
            //Act
            var result = data.ToErrorMessages();
            //Assert
            Assert.Empty(result);
        }

        [Fact]
        public void ToErrorMessages_Success()
        {
            //Arrange
            var message = "message";
            var data = new List<IError> { new TestError { Message = message } };
            //Act
            var result = data.ToErrorMessages();
            //Assert
            Assert.Single(result);
            Assert.Equal(message, result.Single());
        }

        [Fact]
        public void JoinToMessage_NullValues()
        {
            //Arrange
            var data = (IList<IError>)null;
            //Act
            var result = data.JoinToMessage();
            //Assert
            Assert.Empty(result);
        }

        [Fact]
        public void JoinToMessage_EmptyValues()
        {
            //Arrange
            var data = new List<IError>();
            //Act
            var result = data.JoinToMessage();
            //Assert
            Assert.Empty(result);
        }

        [Fact]
        public void JoinToMessage_Success()
        {
            //Arrange
            var message = "message";
            var data = new List<IError> { new TestError { Message = message } };
            //Act
            var result = data.JoinToMessage();
            //Assert
            Assert.Equal(message, result);
        }
    }
}
