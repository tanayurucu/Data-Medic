using DataMedic.Domain.Common.Abstractions;
using DataMedic.Domain.Common.Errors;

using ErrorOr;

namespace DataMedic.Domain.OperatingSystems.ValueObjects;

public sealed class OperatingSystemName : ValueObject
{
    public const int MaxLength = 64;

    public string Value { get; private set; }

    private OperatingSystemName(string value) => Value = value;

    private OperatingSystemName() { }

    public static ErrorOr<OperatingSystemName> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Errors.OperatingSystem.Name.Empty;
        }

        if (value.Length > MaxLength)
        {
            return Errors.OperatingSystem.Name.TooLong;
        }
        return new OperatingSystemName(value);
    }
    public static explicit operator string(OperatingSystemName instance) => instance.Value;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}