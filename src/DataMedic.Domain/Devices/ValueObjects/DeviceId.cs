using DataMedic.Domain.Common.Abstractions;

namespace DataMedic.Domain.Devices.ValueObjects;

public sealed class DeviceId : ValueObject
{
    public Guid Value { get; private set; }

    private DeviceId(Guid value) => Value = value;

    private DeviceId() { }

    public static DeviceId CreateUnique() => new(Guid.NewGuid());

    public static DeviceId Create(Guid value) => new(value);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}