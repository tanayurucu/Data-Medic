namespace DataMedic.Contracts.Sensors;

public record UpdateSensorRequest(
    Guid HostId,
    string Description,
    bool IsActive,
    IUpdateSensorDetailRequest SensorDetail
);

public interface IUpdateSensorDetailRequest
{
    public int Type { get; init; }
}

public record UpdatePingSensorDetailRequest(int ScanPeriodInMinutes, int Type)
    : IUpdateSensorDetailRequest;

public record UpdateNodeRedSensorDetailRequest(string FlowId, int ScanPeriodInMinutes, int Type)
    : IUpdateSensorDetailRequest;

public record UpdateMqttSensorDetailRequest(string TopicName, int TimeToLiveInSeconds, int Type)
    : IUpdateSensorDetailRequest;

public record UpdateKafkaSensorDetailRequest(
    string TopicName,
    int TimeToLiveInSeconds,
    string IdentifierKey,
    string IdentifierValue,
    int Type
) : IUpdateSensorDetailRequest;

public record UpdateDockerSensorDetailRequest(
    int PortainerId,
    string ContainerId,
    int ScanPeriodInMinutes,
    int Type
) : IUpdateSensorDetailRequest;
