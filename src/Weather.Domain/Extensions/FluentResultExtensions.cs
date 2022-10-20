using FluentResults;

namespace Weather.Domain.Extensions
{
    public static class FluentResultExtensions
    {
        public static IEnumerable<string> ToErrorMessages(this IList<IError> errors)
        {
            if(errors.HasAny())
            {
                return Array.Empty<string>();
            }

            return errors.Select(error => error.Message);
        }
    }
}
