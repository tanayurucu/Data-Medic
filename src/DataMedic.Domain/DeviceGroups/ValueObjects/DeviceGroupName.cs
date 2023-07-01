using DataMedic.Domain.Common.Abstractions;
using DataMedic.Domain.Common.Errors;

using ErrorOr;

namespace DataMedic.Domain.DeviceGroups.ValueObjects;

public sealed class DeviceGroupName : ValueObject
{
    public const int MaxLength = 64;

    public string Value { get; private set; }

    private DeviceGroupName(string value) => Value = value;

    private DeviceGroupName() { }

    public static ErrorOr<DeviceGroupName> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Errors.DeviceGroup.Name.Empty;
        }

        if (value.Length > MaxLength)
        {
            return Errors.DeviceGroup.Name.TooLong;
        }

        return new DeviceGroupName(value);
    }
    public static explicit operator string(DeviceGroupName instance) => instance.Value;
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}