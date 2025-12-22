using Ardalis.GuardClauses;
using Weather.Core.HandlerModel;

namespace Weather.Core.Abstractions
{
    public abstract class ValidationStatusRequestHandler<TResponse, TRequest> : IStatusRequestHandler<TResponse, TRequest>
    {
        protected virtual string BadRequestMessage { get { return "Invalid request"; } }

        protected readonly IRequestValidator<TRequest> _validator;

        protected ValidationStatusRequestHandler(IRequestValidator<TRequest> validator)
        {
            _validator = Guard.Against.Null(validator);
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
