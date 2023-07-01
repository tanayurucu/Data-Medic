using DataMedic.Domain.Common.Abstractions;

namespace DataMedic.Domain.Sensors.ValueObjects;

public sealed class PingSensorId : ValueObject
{
    public Guid Value { get; private set; }

    private PingSensorId(Guid value) => Value = value;

    private PingSensorId() { }

    public static PingSensorId CreateUnique() => new(Guid.NewGuid());

    public static PingSensorId Create(Guid value) => new(value);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static explicit operator Guid(PingSensorId instance) => instance.Value;
}
