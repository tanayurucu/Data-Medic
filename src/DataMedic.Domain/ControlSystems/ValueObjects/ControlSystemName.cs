using DataMedic.Domain.Common.Abstractions;
using DataMedic.Domain.Common.Errors;

using ErrorOr;

namespace DataMedic.Domain.ControlSystems.ValueObjects;

public sealed class ControlSystemName : ValueObject
{
    public const int MaxLength = 64;

    public string Value { get; private set; }

    private ControlSystemName(string value) => Value = value;

    private ControlSystemName() { }

    public static ErrorOr<ControlSystemName> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Errors.ControlSystem.Name.Empty;
        }

        if (value.Length > MaxLength)
        {
            return Errors.ControlSystem.Name.TooLong;
        }
        return new ControlSystemName(value);
    }
    public static explicit operator string(ControlSystemName instance) => instance.Value;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}