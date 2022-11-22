using FluentResults;

namespace Weather.Domain.UnitTests.Extensions
{
    internal class TestError : IError
    {
        public List<IError> Reasons => throw new NotImplementedException();

        public string Message {get;set;}

        public Dictionary<string, object> Metadata => throw new NotImplementedException();
    }
}
