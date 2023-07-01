using DataMedic.Domain.Common.Abstractions;

namespace DataMedic.Domain.Sensors.ValueObjects;

public sealed class NodeRedSensorId : ValueObject
{
    public Guid Value { get; private set; }

    private NodeRedSensorId(Guid value) => Value = value;

    private NodeRedSensorId() { }

    public static NodeRedSensorId CreateUnique() => new(Guid.NewGuid());

    public static NodeRedSensorId Create(Guid value) => new(value);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static explicit operator Guid(NodeRedSensorId instance) => instance.Value;
}
