using DataMedic.Domain.Common.Abstractions;

namespace DataMedic.Domain.Sensors.ValueObjects;

public class KafkaSensorId : ValueObject
{
    public Guid Value { get; private set; }

    private KafkaSensorId(Guid value) => Value = value;

    private KafkaSensorId() { }

    public static KafkaSensorId CreateUnique() => new(Guid.NewGuid());

    public static KafkaSensorId Create(Guid value) => new(value);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static explicit operator Guid(KafkaSensorId instance) => instance.Value;
}
