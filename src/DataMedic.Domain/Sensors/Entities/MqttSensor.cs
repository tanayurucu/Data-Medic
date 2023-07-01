using DataMedic.Domain.Common.Abstractions;
using DataMedic.Domain.Hosts.ValueObjects;
using DataMedic.Domain.Sensors.ValueObjects;

namespace DataMedic.Domain.Sensors.Entities;

public class MqttSensor : Entity<MqttSensorId>, ISensorDetail
{
    private MqttSensor(MqttSensorId id, TopicName topicName, TimeSpan timeToLiveInSeconds)
        : base(id)
    {
        TopicName = topicName;
        TimeToLiveInSeconds = timeToLiveInSeconds;
    }

    public TopicName TopicName { get; private set; }
    public TimeSpan TimeToLiveInSeconds { get; private set; }

    private MqttSensor() { }

    public static MqttSensor Create(TopicName topicName, TimeSpan timeToLiveInSeconds) =>
        new(MqttSensorId.CreateUnique(), topicName, timeToLiveInSeconds);

    public void SetTopicName(TopicName topicName) => TopicName = topicName;

    public void SetTimeToLiveInSeconds(TimeSpan timeToLiveInSeconds) => TimeToLiveInSeconds = timeToLiveInSeconds;
}
