using DataMedic.Domain.Common.Abstractions;

namespace DataMedic.Domain.Sensors.ValueObjects;

public class SensorId : ValueObject
{
    public Guid Value { get; private set; }

    private SensorId(Guid value) => Value = value;

    private SensorId() { }

    public static SensorId CreateUnique() => new(Guid.NewGuid());

    public static SensorId Create(Guid value) => new(value);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
