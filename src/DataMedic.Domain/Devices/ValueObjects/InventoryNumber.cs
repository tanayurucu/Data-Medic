using DataMedic.Domain.Common.Abstractions;
using DataMedic.Domain.Common.Errors;

using ErrorOr;

namespace DataMedic.Domain.Devices.ValueObjects;

public sealed class InventoryNumber : ValueObject
{
    public const int MaxLength = 64;
    public string Value { get; private set; }

    private InventoryNumber(string value) => Value = value;

    private InventoryNumber() { }

    public static ErrorOr<InventoryNumber> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Errors.Device.InventoryNumber.Empty;
        }

        if (value.Length > MaxLength)
        {
            return Errors.Device.InventoryNumber.TooLong;
        }

        return new InventoryNumber(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static explicit operator string(InventoryNumber instance) => instance.Value;
}