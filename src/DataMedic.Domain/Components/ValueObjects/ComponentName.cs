using DataMedic.Domain.Common.Abstractions;
using DataMedic.Domain.Common.Errors;

using ErrorOr;

namespace DataMedic.Domain.Components.ValueObjects;

public sealed class ComponentName : ValueObject
{
    public const int MaxLength = 64;
    public string Value { get; private set; }

    private ComponentName(string value) => Value = value;

    public static ErrorOr<ComponentName> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Errors.Component.Name.Empty;
        }

        if (value.Length > MaxLength)
        {
            return Errors.Component.Name.TooLong;
        }

        return new ComponentName(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static explicit operator string(ComponentName instance) => instance.Value;
}
