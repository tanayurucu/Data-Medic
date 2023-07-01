using System.Reflection;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DataMedic.Contracts.Sensors;

/// <summary>
/// JSON Converter for Sensor Detail
/// </summary>
public class UpdateSensorDetailConverter : JsonConverter<IUpdateSensorDetailRequest>
{
    /// <inheritdoc/>
    public override IUpdateSensorDetailRequest? ReadJson(
        JsonReader reader,
        Type objectType,
        IUpdateSensorDetailRequest? existingValue,
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
        IUpdateSensorDetailRequest? value,
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
            0 => typeof(UpdateDockerSensorDetailRequest),
            1 => typeof(UpdateKafkaSensorDetailRequest),
            2 => typeof(UpdateMqttSensorDetailRequest),
            3 => typeof(UpdateNodeRedSensorDetailRequest),
            4 => typeof(UpdatePingSensorDetailRequest),
            _ => throw new ArgumentOutOfRangeException(nameof(type))
        };

    private static IUpdateSensorDetailRequest DeserializeSensorDetailRuntime(
        JToken json,
        Type sensorType
    ) =>
        (IUpdateSensorDetailRequest)
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
