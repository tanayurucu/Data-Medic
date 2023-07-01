using DataMedic.Domain.Common.Abstractions;

namespace DataMedic.Domain.Hosts.ValueObjects;

public sealed class HostId : ValueObject
{
    public Guid Value { get; private set; }

    private HostId(Guid value) => Value = value;

    private HostId() { }

    public static HostId CreateUnique() => new(Guid.NewGuid());

    public static HostId Create(Guid value) => new(value);
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}