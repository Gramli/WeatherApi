using FluentResults;

namespace Weather.Domain.Extensions
{
    public static class FluentResultExtensions
    {
        public static IEnumerable<string> ToErrorMessages(this IList<IError> errors)
        {
            return errors.Select(error => error.Message);
        }
    }
}
