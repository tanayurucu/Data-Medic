using DataMedic.Contracts.Devices;
using DataMedic.Contracts.Hosts;

namespace DataMedic.Contracts.Sensors;

public record SensorWithDetailResponse(
    Guid Id,
    ISensorDetailResponse SensorDetail,
    DeviceComponentResponse DeviceComponent,
    HostResponse Host,
    bool Status,
    bool IsActive,
    int Type,
    Guid DetailId,
    string StatusText,
    DateTime? LastCheckOnUtc,
    string Description
);

public interface ISensorDetailResponse { }

public record DockerSensorDetailResponse(
    Guid Id,
    int PortainerId,
    string ContainerId,
    int ScanPeriodInMinutes,
    string? LastLog
) : ISensorDetailResponse;

public record KafkaSensorDetailResponse(
    Guid Id,
    string TopicName,
    int TimeToLiveInSeconds,
    string IdentifierKey,
    string IdentifierValue
) : ISensorDetailResponse;

public record MqttSensorDetailResponse(Guid Id, string TopicName, int TimeToLiveInSeconds)
    : ISensorDetailResponse;

public record NodeRedSensorDetailResponse(
    string FlowId,
    string? LastErrorLog,
    Guid Id,
    int ScanPeriodInMinutes
) : ISensorDetailResponse;

public record PingSensorDetailResponse(int ScanPeriodInMinutes, Guid Id) : ISensorDetailResponse;
