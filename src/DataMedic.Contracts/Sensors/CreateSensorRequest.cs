using Newtonsoft.Json;

namespace DataMedic.Contracts.Sensors;

public record CreateSensorRequest(
    Guid HostId,
    Guid DeviceComponentId,
    string? Description,
    Guid DeviceId,
    ICreateSensorDetailRequest SensorDetail
);

public interface ICreateSensorDetailRequest
{
    public int Type { get; init; }
}

public record CreatePingSensorDetailRequest(int ScanPeriodInMinutes, int Type)
    : ICreateSensorDetailRequest;

public record CreateNodeRedSensorDetailRequest(string FlowId, int ScanPeriodInMinutes, int Type)
    : ICreateSensorDetailRequest;

public record CreateMqttSensorDetailRequest(string TopicName, int TimeToLiveInSeconds, int Type)
    : ICreateSensorDetailRequest;

public record CreateKafkaSensorDetailRequest(
    string TopicName,
    int TimeToLiveInSeconds,
    string IdentifierKey,
    string IdentifierValue,
    int Type
) : ICreateSensorDetailRequest;

public record CreateDockerSensorDetailRequest(
    int PortainerId,
    string ContainerId,
    int ScanPeriodInMinutes,
    int Type
) : ICreateSensorDetailRequest;
