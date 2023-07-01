using DataMedic.Domain.Common.Abstractions;

namespace DataMedic.Domain.OperatingSystems.ValueObjects;

public sealed class OperatingSystemId : ValueObject
{
    public Guid Value { get; private set; }

    private OperatingSystemId(Guid value) => Value = value;

    private OperatingSystemId() { }

    public static OperatingSystemId CreateUnique() => new(Guid.NewGuid());

    public static OperatingSystemId Create(Guid value) => new(value);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}