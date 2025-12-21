using Weather.Core.HandlerModel;

namespace Weather.Core.Abstractions
{
    public interface IRequestValidator<TRequest>
    {
        RequestValidationResult Validate(TRequest request);
    }
}
