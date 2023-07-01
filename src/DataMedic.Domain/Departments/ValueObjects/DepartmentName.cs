using DataMedic.Domain.Common.Abstractions;
using DataMedic.Domain.Common.Errors;

using ErrorOr;

namespace DataMedic.Domain.Departments.ValueObjects;

public sealed class DepartmentName : ValueObject
{
    public const int MaxLength = 10;
    public string Value { get; private set; }

    private DepartmentName(string value) => Value = value;

    private DepartmentName() { }

    public static ErrorOr<DepartmentName> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Errors.Department.Name.Empty;
        }

        if (value.Length > MaxLength)
        {
            return Errors.Department.Name.TooLong;
        }

        return new DepartmentName(value);
    }

    public static explicit operator string(DepartmentName instance) => instance.Value;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}