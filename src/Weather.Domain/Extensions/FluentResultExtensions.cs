using FluentResults;

namespace Weather.Domain.Extensions
{
    public static class FluentResultExtensions
    {
        public static IEnumerable<string> ToErrorMessages(this IList<IError> errors)
        {
            if(!errors.HasAny())
            {
                return Array.Empty<string>();
            }

            return errors.Select(error => error.Message);
        }

        public static string JoinToMessage(this IList<IError> errors)
        {
            if (!errors.HasAny())
            {
                return string.Empty;
            }

            return string.Join(',', errors.ToErrorMessages());
        }

        public static T UnwrapOrDefault<T>(this IResult<T> result, Action<IList<IError>> isFailedAction)
        {
            if (result.IsFailed) 
            {
                isFailedAction(result.Errors);
                return result.ValueOrDefault;
            }

            return result.Value;
        }

        public static T UnwrapOrDefault<T>(this IResult<T> result, IResultBase failedError)
        {
            if (result.IsFailed)
            {
                failedError.Errors.AddRange(result.Errors);
                return result.ValueOrDefault;
            }

            return result.Value;
        }

        public static async Task<T> UnwrapOrDefaultAsync<T>(this Task<IResult<T>> resultAsync, IResultBase failedError)
        {
            var result = await resultAsync;
            if (result.IsFailed)
            {
                failedError.Errors.AddRange(result.Errors);
                return result.ValueOrDefault;
            }

            return result.Value;
        }
    }
}
