using System.Resources;
using DataMedic.Domain.Common.Abstractions;

namespace DataMedic.Domain.Hosts.ValueObjects;

public enum HostType
{
    DOCKER,
    KAFKA,
    MQTT,
    NODE_RED,
    PING
}
