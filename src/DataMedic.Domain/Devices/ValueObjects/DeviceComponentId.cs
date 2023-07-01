using DataMedic.Domain.Common.Abstractions;

namespace DataMedic.Domain.Devices.ValueObjects;

public sealed class DeviceComponentId : ValueObject
{
    public Guid Value { get; private set; }

    private DeviceComponentId(Guid value) => Value = value;

    private DeviceComponentId() { }

    public static DeviceComponentId CreateUnique() => new(Guid.NewGuid());

    public static DeviceComponentId Create(Guid value) => new(value);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}