namespace Weather.Core.HandlerModel
{
    public static class HandlerResponses
    {
        public static HandlerResponse<T> AsValidationError<T>(IEnumerable<string> errorMessages)
        {
            return AsResponse<T>(HandlerStatusCode.ValidationError, errorMessages);
        }

        public static HandlerResponse<T> AsValidationError<T>(string errorMessages)
        {
            return AsResponse<T>(HandlerStatusCode.ValidationError, errorMessages);
        }

        public static HandlerResponse<T> AsInternalError<T>(IEnumerable<string> errorMessages)
        {
            return AsResponse<T>(HandlerStatusCode.InternalError, errorMessages);
        }

        public static HandlerResponse<T> AsInternalError<T>(string errorMessages)
        {
            return AsResponse<T>(HandlerStatusCode.InternalError, errorMessages);
        }

        public static HandlerResponse<T> AsSuccessWithEmptyResult<T>()
            => new()
            {
                StatusCode = HandlerStatusCode.SuccessWithEmptyResult,
            };

        public static HandlerResponse<T> AsSuccess<T>(T data)
            => new()
            {
                Data = data,
                StatusCode = HandlerStatusCode.Success,
            };

        public static HandlerResponse<T> AsSuccess<T>(T data, IEnumerable<string> errorMessages)
            => new()
            {
                Data = data,
                StatusCode = HandlerStatusCode.Success,
                Errors = errorMessages
            };

        private static HandlerResponse<T> AsResponse<T>(HandlerStatusCode handlerStatusCode, IEnumerable<string> errorMessages)
        {
            return new HandlerResponse<T>
            {
                StatusCode = handlerStatusCode,
                Errors = errorMessages
            };
        }

        private static HandlerResponse<T> AsResponse<T>(HandlerStatusCode handlerStatusCode, string errorMessage)
        {
            return new HandlerResponse<T>
            {
                StatusCode = handlerStatusCode,
                Errors = [errorMessage]
            };
        }
    }
}
