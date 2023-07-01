using DataMedic.Domain.Common.Abstractions;
using DataMedic.Domain.Common.Errors;

using ErrorOr;

namespace DataMedic.Domain.Sensors.ValueObjects;

public sealed class SensorDetail : ValueObject
{
    public SensorType Type { get; private set; }
    public Guid DetailId { get; private set; }

    private SensorDetail(SensorType type, Guid detailId)
    {
        Type = type;
        DetailId = detailId;
    }

    private SensorDetail() { }

    public static SensorDetail CreateDockerSensorDetail(DockerSensorId id) =>
        new(SensorType.DOCKER, id.Value);

    public static SensorDetail CreateKafkaSensorDetail(KafkaSensorId id) =>
        new(SensorType.KAFKA, id.Value);

    public static SensorDetail CreateMqttSensorDetail(MqttSensorId id) =>
        new(SensorType.MQTT, id.Value);

    public static SensorDetail CreateNodeRedSensorDetail(NodeRedSensorId id) =>
        new(SensorType.NODE_RED, id.Value);

    public static SensorDetail CreatePingSensorDetail(PingSensorId id) =>
        new(SensorType.PING, id.Value);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Type;
        yield return DetailId;
    }
}
