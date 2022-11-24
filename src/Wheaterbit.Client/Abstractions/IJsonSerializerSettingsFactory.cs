using Newtonsoft.Json;

namespace Wheaterbit.Client.Abstractions
{
    public interface IJsonSerializerSettingsFactory
    {
        JsonSerializerSettings Create();
    }
}
