using Microsoft.Extensions.Options;
using Moq;
using System.Xml.Linq;
using Validot;
using Validot.Results;
using Wheaterbit.Client.Options;

namespace Wheaterbit.Client.UnitTests.Extensions
{
    internal static class OptionsValidationExtensions
    {
        internal static Mock<IValidator<WeatherbitOptions>> Setup(this Mock<IValidator<WeatherbitOptions>> optionsValidatorMock, bool anyErrors)
        {
            var validationResultMock = new Mock<IValidationResult>();
            validationResultMock.SetupGet(x => x.AnyErrors).Returns(anyErrors);

            optionsValidatorMock
            .Setup(x => x.Validate(It.IsAny<WeatherbitOptions>(), It.IsAny<bool>()))
            .Returns(validationResultMock.Object);

            return optionsValidatorMock;
        }
    }
}
