using System.Net;

using DataMedic.Domain.Common.Abstractions;
using DataMedic.Domain.Common.Errors;

using ErrorOr;

namespace DataMedic.Domain.Devices.ValueObjects;

public sealed class IpAddress : ValueObject
{
    public const int MaxLength = 15;
    public string Value { get; private set; }

    private IpAddress(string value) => Value = value;

    private IpAddress() { }

    public static ErrorOr<IpAddress> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Errors.Device.IpAddress.Empty;
        }

        if (
            value.Length > MaxLength
            || value.Count(character => character == '.') != 3
            || !IPAddress.TryParse(value, out _)
        )
        {
            return Errors.Device.IpAddress.Invalid;
        }

        return new IpAddress(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static explicit operator string(IpAddress instance) => instance.Value;
}