using System.Text.Json.Serialization;

namespace Weather.Core.HandlerModel
{
    public class HandlerResponse<T> : DataResponse<T>
    {
        [JsonIgnore]
        public HandlerStatusCode StatusCode { get; init; }
    }
}
