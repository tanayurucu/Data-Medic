using DataMedic.Domain.Common.Abstractions;
using DataMedic.Domain.Common.Errors;

using ErrorOr;

namespace DataMedic.Domain.Devices.ValueObjects;

public sealed class DeviceName : ValueObject
{
    public const int MaxLength = 64;

    public string Value { get; private set; }

    private DeviceName(string value) => Value = value;

    private DeviceName() { }

    public static ErrorOr<DeviceName> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Errors.Device.Name.Empty;
        }

        if (value.Length > MaxLength)
        {
            return Errors.Device.Name.TooLong;
        }

        return new DeviceName(value);
    }
    public static explicit operator string(DeviceName instance) => instance.Value;
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}