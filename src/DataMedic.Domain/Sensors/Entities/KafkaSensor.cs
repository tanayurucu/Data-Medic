using DataMedic.Domain.Common.Abstractions;
using DataMedic.Domain.Hosts.ValueObjects;
using DataMedic.Domain.Sensors.ValueObjects;

namespace DataMedic.Domain.Sensors.Entities;

public class KafkaSensor : Entity<KafkaSensorId>, ISensorDetail
{
    public TopicName TopicName { get; set; }
    public TimeSpan TimeToLiveInSeconds { get; set; }
    public string? IdentifierKey { get; set; }
    public string? IdentifierValue { get; set; }

    private KafkaSensor(
        KafkaSensorId id,
        TopicName topicName,
        TimeSpan timeToLiveInSeconds,
        string identifierKey,
        string identifierValue
    )
        : base(id)
    {
        TopicName = topicName;
        TimeToLiveInSeconds = timeToLiveInSeconds;
        IdentifierKey = identifierKey;
        IdentifierValue = identifierValue;
    }

    private KafkaSensor() { }

    public static KafkaSensor Create(
        TopicName topicName,
        TimeSpan timeToLiveInSeconds,
        string identifierKey,
        string identifierValue
    ) =>
        new(
            KafkaSensorId.CreateUnique(),
            topicName,
            timeToLiveInSeconds,
            identifierKey,
            identifierValue
        );

    public void SetTopicName(TopicName topicName) => TopicName = topicName;

    public void SetTimeToLiveInSeconds(TimeSpan timeToLiveInSeconds) =>
        TimeToLiveInSeconds = timeToLiveInSeconds;

    public void SetIdentifierKey(string identifierKey) => IdentifierKey = identifierKey;

    public void SetIdentifierValue(string identifierValue) => IdentifierValue = identifierValue;
}
