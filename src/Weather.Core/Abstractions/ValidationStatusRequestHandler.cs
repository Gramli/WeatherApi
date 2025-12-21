using Weather.Core.HandlerModel;

namespace Weather.Core.Abstractions
{
    public abstract class ValidationCoreRequestHandler<TResponse, TRequest> : IStatusRequestHandler<TResponse, TRequest>
    {
        protected virtual string BadRequestMessage { get { return "Invalid request"; } }

        protected readonly IRequestValidator<TRequest> _validator;

        protected ValidationCoreRequestHandler(IRequestValidator<TRequest> validator)
        {
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }
        public async Task<HandlerResponse<TResponse>> HandleAsync(TRequest request, CancellationToken cancellationToken)
        {

            var validationResult = _validator.Validate(request);
            if (!validationResult.IsValid)
            {
                return CreateInvalidResponse(request, validationResult);
            }

            return await HandleValidRequestAsync(request, cancellationToken);
        }

        protected abstract Task<HandlerResponse<TResponse>> HandleValidRequestAsync(TRequest request, CancellationToken cancellationToken);

        protected virtual HandlerResponse<TResponse> CreateInvalidResponse(TRequest request, RequestValidationResult validationResult)
            => HandlerResponses.AsValidationError<TResponse>(BadRequestMessage);
    }
}
