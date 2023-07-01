using DataMedic.Domain.Common.Abstractions;

namespace DataMedic.Domain.Departments.ValueObjects;

public sealed class DepartmentId : ValueObject
{
    public Guid Value { get; private set; }

    private DepartmentId(Guid value) => Value = value;

    private DepartmentId() { }

    public static DepartmentId CreateUnique() => new(Guid.NewGuid());

    public static DepartmentId Create(Guid value) => new(value);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}