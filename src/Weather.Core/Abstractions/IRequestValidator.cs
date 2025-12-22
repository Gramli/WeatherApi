using Weather.Core.HandlerModel;

namespace Weather.Core.Abstractions
{
    public interface IRequestValidator<in TRequest>
    {
        RequestValidationResult Validate(TRequest request);
    }
}
