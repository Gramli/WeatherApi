using Newtonsoft.Json;

namespace Wheaterbit.Client.Abstractions
{
    internal interface IJsonSerializerSettingsFactory
    {
        JsonSerializerSettings Create();
    }
}
