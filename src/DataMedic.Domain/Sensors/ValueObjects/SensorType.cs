using DataMedic.Domain.Common.Abstractions;

namespace DataMedic.Domain.Sensors.ValueObjects;

public enum SensorType
{
    DOCKER,
    KAFKA,
    MQTT,
    NODE_RED,
    PING
}
