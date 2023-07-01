using System.Reflection;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DataMedic.Contracts.Sensors;

/// <summary>
/// JSON Converter for Sensor Detail
/// </summary>
public class CreateSensorDetailConverter : JsonConverter<ICreateSensorDetailRequest>
{
    /// <inheritdoc/>
    public override ICreateSensorDetailRequest? ReadJson(
        JsonReader reader,
        Type objectType,
        ICreateSensorDetailRequest? existingValue,
        bool hasExistingValue,
        JsonSerializer serializer
    )
    {
        var json = JToken.ReadFrom(reader);
        var typeToken = json.SelectToken("type") ?? json.SelectToken("Type");
        if (typeToken is null)
        {
            return null;
        }

        var type = ResolveISensorDetailTypeRuntime((int)typeToken);
        return DeserializeSensorDetailRuntime(json, type);
    }

    /// <inheritdoc/>

    public override void WriteJson(
        JsonWriter writer,
        ICreateSensorDetailRequest? value,
        JsonSerializer serializer
    )
    {
        if (value is null)
        {
            serializer.Serialize(writer, value);
        }
        else
        {
            var type = ResolveISensorDetailTypeRuntime(value.Type);
            serializer.Serialize(writer, value, type);
        }
    }

    private static Type ResolveISensorDetailTypeRuntime(int type) =>
        type switch
        {
            0 => typeof(CreateDockerSensorDetailRequest),
            1 => typeof(CreateKafkaSensorDetailRequest),
            2 => typeof(CreateMqttSensorDetailRequest),
            3 => typeof(CreateNodeRedSensorDetailRequest),
            4 => typeof(CreatePingSensorDetailRequest),
            _ => throw new ArgumentOutOfRangeException(nameof(type))
        };

    private static ICreateSensorDetailRequest DeserializeSensorDetailRuntime(
        JToken json,
        Type sensorType
    ) =>
        (ICreateSensorDetailRequest)
            Array
                .Find(
                    typeof(JToken).GetMethods(BindingFlags.Public | BindingFlags.Instance),
                    method =>
                        method.Name == "ToObject"
                        && method.GetParameters().Length == 0
                        && method.IsGenericMethod
                )
                ?.MakeGenericMethod(sensorType)
                .Invoke(json, null)! ?? throw new InvalidOperationException();
}
