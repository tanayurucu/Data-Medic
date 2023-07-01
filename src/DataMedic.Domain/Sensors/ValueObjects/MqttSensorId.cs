using DataMedic.Domain.Common.Abstractions;

namespace DataMedic.Domain.Sensors.ValueObjects;

public class MqttSensorId : ValueObject
{
    public Guid Value { get; private set; }

    private MqttSensorId(Guid value) => Value = value;

    private MqttSensorId() { }

    public static MqttSensorId CreateUnique()
    {
        var idGuid = new MqttSensorId(Guid.NewGuid());
        return idGuid;
    }

    public static MqttSensorId Create(Guid value) => new(value);
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
    public static explicit operator Guid(MqttSensorId instance) => instance.Value;
}