using DataMedic.Domain.Common.Abstractions;

using ErrorOr;

namespace DataMedic.Domain.Sensors.ValueObjects;

public sealed class TopicName : ValueObject
{
    public const int MaxLength = 64;
    public string Value { get; private set; }

    private TopicName(string value) => Value = value;

    private TopicName() { }

    public static ErrorOr<TopicName> Create(string value)
    {
        return new TopicName(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}