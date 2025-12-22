namespace Weather.Core.HandlerModel
{
    public class RequestValidationResult
    {
        public required bool IsValid { get; init; }

        public string[] ErrorMessages { get; init; } = [];

        public override string ToString()
            => string.Join(",", ErrorMessages);
    }
}
