using DataMedic.Domain.Common.Abstractions;

namespace DataMedic.Domain.ControlSystems.ValueObjects;

public sealed class ControlSystemId : ValueObject
{
    public Guid Value { get; private set; }

    public ControlSystemId(Guid value) => Value = value;

    public ControlSystemId() { }

    public static ControlSystemId CreateUnique() => new(Guid.NewGuid());

    public static ControlSystemId Create(Guid value) => new(value);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}