using Newtonsoft.Json;
using Wheaterbit.Client.Abstractions;

namespace Wheaterbit.Client.Factories
{
    internal sealed class JsonSerializerSettingsFactory : IJsonSerializerSettingsFactory
    {
        public JsonSerializerSettings Create()
        {
            return new JsonSerializerSettings
            {
                DateFormatString = "yyyy-MM-dd hh:mm"
            };
        }
    }
}
