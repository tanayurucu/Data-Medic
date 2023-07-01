using DataMedic.Domain.Common.Abstractions;

namespace DataMedic.Domain.Projects.ValueObjects;

public class ProjectId : ValueObject
{
    public Guid Value { get; private set; }

    private ProjectId(Guid value) => Value = value;

    private ProjectId() { }

    public static ProjectId CreateUnique() => new(Guid.NewGuid());

    public static ProjectId Create(Guid value) => new(value);
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}