using DataMedic.Domain.Common.Abstractions;

namespace DataMedic.Domain.Emails.ValueObjects;

public sealed class EmailId : ValueObject
{
    private EmailId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; private set; }

    public static EmailId CreateUnique() => new(Guid.NewGuid());

    public static EmailId Create(Guid value) => new(value);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}