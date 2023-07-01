using DataMedic.Domain.Common.Abstractions;

namespace DataMedic.Domain.Components.ValueObjects;

public sealed class ComponentId : ValueObject
{
    public Guid Value { get; private set; }

    private ComponentId(Guid value) => Value = value;

    public static ComponentId CreateUnique() => new(Guid.NewGuid());

    public static ComponentId Create(Guid value) => new(value);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}