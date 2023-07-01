using DataMedic.Domain.Common.Abstractions;

namespace DataMedic.Domain.Sensors.ValueObjects;

public class DockerSensorId : ValueObject
{
    public Guid Value { get; private set; }

    private DockerSensorId(Guid value) => Value = value;

    private DockerSensorId() { }

    public static DockerSensorId CreateUnique() => new(Guid.NewGuid());

    public static DockerSensorId Create(Guid value) => new(value);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static explicit operator Guid(DockerSensorId instance) => instance.Value;
}