using Weather.Domain.Extensions;

namespace Weather.Domain.UnitTests.Extensions
{
    public class EnumerableExtensionsTests
    {
        [Fact]
        public void HasAny_NullValues()
        {
            //Arrange
            var data = (List<int>)null;
            //Act
            var result = data.HasAny();
            //Assert
            Assert.False(result);
        }

        [Fact]
        public void HasAny_EmptyValues()
        {
            //Arrange
            var data = new List<int>();
            //Act
            var result = data.HasAny();
            //Assert
            Assert.False(result);
        }

        [Fact]
        public void HasAny_Success()
        {
            //Arrange
            var data = new List<int> { 1 };
            //Act
            var result = data.HasAny();
            //Assert
            Assert.True(result);
        }
    }
}
