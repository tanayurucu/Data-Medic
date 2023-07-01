using DataMedic.Domain.Common.Abstractions;

namespace DataMedic.Domain.DeviceGroups.ValueObjects;

public sealed class DeviceGroupId : ValueObject
{
    public Guid Value { get; private set; }

    private DeviceGroupId(Guid value) => Value = value;

    private DeviceGroupId() { }

    public static DeviceGroupId CreateUnique() => new(Guid.NewGuid());

    public static DeviceGroupId Create(Guid value) => new(value);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}