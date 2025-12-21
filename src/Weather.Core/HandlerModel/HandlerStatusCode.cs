namespace Weather.Core.HandlerModel
{
    public enum HandlerStatusCode
    {
        Success = 0,
        SuccessWithEmptyResult = 1,
        ValidationError = 2,
        InternalError = 4
    }
}
